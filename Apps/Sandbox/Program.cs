using System.Reflection;
using System.Text;
using Lib.Core.Entities.Core;
using Lib.Core.Interfaces.Data;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Mod.Media.SoundPlayer.Singletons;
using static Mod.Media.SoundPlayer.Singletons.SoundManager;

namespace Sandbox;

public class Test123
{
    public int IntField;

    public int IntProperty
    {
        get => IntField;
        set => IntField = value;
    }
}

public partial class Program : Form
{
    public Program()
    {
        InitializeComponent();
    }
    //private static readonly ManualResetEvent _event = new(false);

    private static string GetParams(params ParameterInfo[]? @params)
    {
        return (@params?.Length ?? 0) < 1
            ? string.Empty
            : string.Join(", ", @params.Select(p => $"{p.ParameterType.Name.ToLowerInvariant() switch
            {
                "boolean" => "bool",
                "string" => "string",
                "int16" => "short",
                "int32" => "int",
                _ => p.ParameterType.Name,
            }} {p.Name}"));
    }

    private static string GetConstructor(ConstructorInfo ci, string className)
    {
        return string.Join(Environment.NewLine + "\t",
            string.Join(" ",
                (ci.Attributes & MethodAttributes.Public) == MethodAttributes.Public ? "\tpublic" : "\tprivate",
                $"{className}({GetParams(ci.GetParameters())})"),
            "{",
            "}");
    }

    private static string GetMethod(MethodInfo mi)
    {
        return string.Join(Environment.NewLine + "\t",
            string.Join(" ",
                (mi.Attributes & MethodAttributes.Public) == MethodAttributes.Public ? "\tpublic" : "\tprivate",
                (mi.ReturnType?.Name?.ToLowerInvariant() ?? "void") switch
                {
                    "void" => "void",
                    "boolean" => "bool",
                    "string" => "string",
                    "int16" => "short",
                    "int32" => "int",
                    _ => mi.ReturnType.FullName,
                },
                $"{mi.Name}({GetParams(mi.GetParameters())})"),
            "{",
            (mi.ReturnType?.Name?.ToLowerInvariant() ?? "void") switch
            {
                "void" => string.Empty,
                _ => $"return default({mi.ReturnType.FullName});"
            },
            "}");
    }

    private static string GetField(FieldInfo fi)
    {
        return string.Join(" ",
            (fi.Attributes & FieldAttributes.Public) == FieldAttributes.Public ? "\tpublic" : "\tprivate",
            fi.FieldType.Name.ToLowerInvariant() switch
            {
                "boolean" => "bool",
                "string" => "string",
                "int16" => "short",
                "int32" => "int",
                _ => fi.FieldType.FullName,
            },
            $"{fi.Name};");
    }

    private static string GetProperty(PropertyInfo pi)
    {
        return string.Join(Environment.NewLine + "\t",
            string.Join(" ",
                "\tpublic",
                pi.PropertyType.Name.ToLowerInvariant() switch
                {
                    "boolean" => "bool",
                    "string" => "string",
                    "int16" => "short",
                    "int32" => "int",
                    _ => pi.PropertyType.FullName,
                },
                pi.Name),
            "{",
            "get;",
            "set;",
            "}");
    }

    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    [STAThread]
    public static void Main()
    {
        //// To customize application configuration such as set high DPI settings or default 

        //// see https://aka.ms/applicationconfiguration.
        //ApplicationConfiguration.Initialize();
        //Application.Run(new Program());

        //var data = File.ReadAllText(@"Data\MSG_ROOMDESC.data").FromHex();
        //var roomDesc = new RoomDesc(data);
        //roomDesc.Deserialize(roomDesc.Stream);

        //Experiment4();
        //Experiment5();
        //Experiment6();

        var sourceType = typeof(Test123);

        var sb = new StringBuilder("""
                                   using System;

                                   namespace RoslynCompileSample;

                                   public class Writer
                                   {
                                       public void Write(string message)
                                       {
                                           Console.WriteLine(message);
                                       }
                                   }

                                   """);

        var memberInfos = new List<MemberInfo>();
        memberInfos.AddRange(sourceType.GetMembers(BindingFlags.Public | BindingFlags.Instance));

        sb.AppendLine($"public class {sourceType.Name} : {sourceType.FullName}");
        sb.AppendLine("{");

        foreach (var memberInfo in memberInfos)
        {
            switch (memberInfo)
            {
                case ConstructorInfo ci:
                    sb.AppendLine(GetConstructor(ci, sourceType.Name));
                    break;
                case MethodInfo mi:
                    sb.AppendLine(GetMethod(mi));
                    break;
                case FieldInfo fi:
                    sb.AppendLine(GetField(fi));
                    break;
                case PropertyInfo pi:
                    sb.AppendLine(GetProperty(pi));
                    break;
            }
        }

        sb.AppendLine("}");

        var code = sb.ToString();

        // define source code, then parse it (to the type used for compilation)
        var syntaxTree = CSharpSyntaxTree.ParseText(code);

        // define other necessary objects for compilation
        var assemblyName = Path.GetRandomFileName();
        var references = new MetadataReference[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location), MetadataReference.CreateFromFile(Assembly.Load("System.Console").Location),
            MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
        };

        // analyse and generate IL code from syntax tree
        var compilation = CSharpCompilation.Create(
            assemblyName,
            syntaxTrees: [syntaxTree],
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using (var ms = new MemoryStream())
        {
            // write IL code into memory
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                // handle exceptions
                var failures =
                    result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                foreach (var diagnostic in failures)
                {
                    Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                }
            }
            else
            {
                // load this 'virtual' DLL so that we can use
                ms.Seek(0, SeekOrigin.Begin);
                var assembly = Assembly.Load(ms.ToArray());

                // create instance of the desired class and call the desired function
                var type = assembly.GetType("RoslynCompileSample.Writer");
                var obj = Activator.CreateInstance(type);
                type.InvokeMember("Write",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null,
                    obj,
                    ["Hello World"]);
            }
        }

        Console.ReadLine();
    }

    private static void Experiment4()
    {
        Func<string, bool>? where = l => l == "123";
        var test = new List<string> { "Test", "123" }
            .Where(where)
            .ToList();
    }

    private static void Experiment5()
    {
        var iStructTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(t => t.GetTypes())
            .Where(t => !t.IsInterface && t.GetInterfaces().Contains(typeof(IStruct)));

        var container = new DIContainer();
        container.RegisterTypes(iStructTypes);
    }

    private static void Experiment6()
    {
        SoundManager.Current.Play(@"Media\Yes.mp3");
        SoundManager.Current.PlaySound(@"Media\Boing.wav",
            PlaySoundFlags.SND_ASYNC | PlaySoundFlags.SND_NOWAIT | PlaySoundFlags.SND_SYSTEM | PlaySoundFlags.SND_LOOP);
    }
}