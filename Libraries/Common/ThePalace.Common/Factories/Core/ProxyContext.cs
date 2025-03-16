using System.Collections.Concurrent;
using System.Dynamic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using ThePalace.Common.Entities.EventArgs;

namespace ThePalace.Common.Factories.Core;

public interface IORMapper
{
    void PopulateObject(object entity, object data);
}

public class ProxyContext : DynamicObject, IDisposable
{
    protected ProxyContext()
    {
    }

    protected class ProxyItem(
        Type sourceType,
        Type proxyType) : IDisposable
    {
        public Type SourceType { get; } = sourceType;
        public Type ProxyType { get; } = proxyType;

        private object _instance;
        public object Instance => _instance ??= Activator.CreateInstance(ProxyType);

        public void Dispose()
        {
            if (_instance is IDisposable disposable)
            {
                disposable.Dispose();
            }

            _instance = null;

            GC.SuppressFinalize(this);
        }
    }

    ~ProxyContext()
    {
        Dispose();
    }

    public void Dispose()
    {
        _proxies.Values.ToList().ForEach(p => p.Dispose());
        _proxies.TryRemove(GetType(), out _);

        GC.SuppressFinalize(this);
    }

    protected static readonly ConcurrentDictionary<Type, ProxyItem> _proxies = new();

    public static event EventHandler MethodInvoked;
    public static event EventHandler PropertyAccessed;
    public static event EventHandler PropertyChanged;
    public static event EventHandler ExceptionOccurred;

    public static object GetInstance(Type sourceType, params object[] args)
    {
        var proxyType = (Type?)null;
        var @ref = (object?)null;

        try
        {
            var proxyItem = new ProxyItem(sourceType, proxyType);
            _proxies.TryAdd(sourceType, proxyItem);

            return @ref = proxyItem.Instance;
        }
        catch (TaskCanceledException ex)
        {
            return null;
        }
        catch (Exception ex)
        {
            ExceptionOccurred?.Invoke(sourceType.Name, new ExceptionEventArgs
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Exception = ex,
            });
        }

        return null;
    }

    public static T GetInstance<T>(params object[] args)
    {
        return (T)GetInstance(typeof(T), args);
    }

    internal static object? Do(Func<object[], object?> cb, params object[] args)
    {
        ArgumentNullException.ThrowIfNull(cb, nameof(cb));

        try
        {
            return cb(args);
        }
        catch (TaskCanceledException ex)
        {
            return null;
        }
        catch (Exception ex)
        {
            ExceptionOccurred?.Invoke(null, new ExceptionEventArgs
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Exception = ex,
            });
        }

        return null;
    }

    internal static object? Invoke(MethodInfo nfo, object @ref, params object[] args)
    {
        ArgumentNullException.ThrowIfNull(nfo, nameof(nfo));
        ArgumentNullException.ThrowIfNull(@ref, nameof(@ref));

        try
        {
            nfo.Invoke(@ref, args);
        }
        catch (TaskCanceledException ex)
        {
            return null;
        }
        catch (Exception ex)
        {
            ExceptionOccurred?.Invoke(null, new ExceptionEventArgs
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Exception = ex,
            });
        }
        finally
        {
            MethodInvoked?.Invoke(null, new MethodInvokedEventArgs
            {
                Member = nfo,
                Args = args,
            });
        }

        return null;
    }

    internal static object? Get(MemberInfo nfo, object @ref, params object[] args)
    {
        ArgumentNullException.ThrowIfNull(nfo, nameof(nfo));
        ArgumentNullException.ThrowIfNull(@ref, nameof(@ref));

        try
        {
            var value = (object?)null;

            switch (nfo)
            {
                case MethodInfo mi: return Invoke(mi, @ref, args);
                case PropertyInfo pi:
                    break;
                case FieldInfo fi:
                    break;
            }
        }
        catch (TaskCanceledException ex)
        {
            return null;
        }
        catch (Exception ex)
        {
            ExceptionOccurred?.Invoke(@ref, new ExceptionEventArgs
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Exception = ex,
            });
        }
        finally
        {
            PropertyAccessed?.Invoke(@ref, new PropertyAccessedEventArgs
            {
                Member = nfo,
            });
        }

        return null;
    }

    internal static void Set(MemberInfo nfo, object @ref, object? value)
    {
        ArgumentNullException.ThrowIfNull(nfo, nameof(nfo));
        ArgumentNullException.ThrowIfNull(@ref, nameof(@ref));

        var oldValue = (object?)null;

        switch (nfo)
        {
            case MethodInfo mi: throw new ArgumentException(nameof(nfo));
            case PropertyInfo pi:
                oldValue = pi.GetValue(@ref);

                break;
            case FieldInfo fi:
                oldValue = fi.GetValue(@ref);

                break;
        }

        try
        {
            switch (nfo)
            {
                case PropertyInfo pi:
                    pi.SetValue(@ref, value, null);

                    break;
                case FieldInfo fi:
                    fi.SetValue(@ref, value);

                    break;
            }
        }
        catch (TaskCanceledException ex)
        {
            return;
        }
        catch (Exception ex)
        {
            ExceptionOccurred?.Invoke(@ref, new ExceptionEventArgs
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Exception = ex,
            });
        }
        finally
        {
            try
            {
                var newValue = nfo switch
                {
                    MethodInfo mi => throw new ArgumentException(nameof(nfo)),
                    PropertyInfo pi => pi.GetValue(@ref),
                    FieldInfo fi => fi.GetValue(@ref),
                    _ => null
                };

                if ((oldValue != null || newValue != null) &&
                    oldValue != newValue)
                    PropertyChanged?.Invoke(@ref, new PropertyChangedEventArgs
                    {
                        Member = nfo,
                        OldValue = oldValue,
                        NewValue = newValue,
                    });
            }
            catch (TaskCanceledException ex)
            {
            }
            catch (Exception ex)
            {
                ExceptionOccurred?.Invoke(@ref, new ExceptionEventArgs
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    Exception = ex,
                });
            }
        }
    }

    private static string SanitizeName(string? name)
    {
        name = name?.Trim();

        if (string.IsNullOrWhiteSpace(name)) return string.Empty;

        name = CONST_REGEX_Filter_LegalNames.Replace(name, "_");
        return CONST_REGEX_Sequence.Aggregate(name, (current, regex) => regex.Replace(current, string.Empty));
    }

    private static readonly Regex CONST_REGEX_Filter_LegalNames = new Regex(@"[^\w\d_]+", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex CONST_REGEX_Trim_StringStart = new Regex(@"^[\d_]+", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex CONST_REGEX_Trim_StringEnd = new Regex(@"[_]+$", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex[] CONST_REGEX_Sequence = [CONST_REGEX_Trim_StringStart, CONST_REGEX_Trim_StringEnd];

    private AssemblyName _asmName;
    private AssemblyBuilder _asmBuilder;
    private ModuleBuilder _modBuilder;

    private static void CreateAssemblyBuilder(
        string? assemblyName,
        out AssemblyName asmName,
        out AssemblyBuilder asmBuilder)
    {
        if (string.IsNullOrWhiteSpace(assemblyName)) throw new ArgumentNullException(nameof(assemblyName));

        asmName = new AssemblyName(assemblyName);
        asmBuilder = AssemblyBuilder.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
    }

    private static void CreateModuleBuilder(
        AssemblyName asmName,
        AssemblyBuilder asmBuilder,
        out ModuleBuilder modBuilder)
    {
        modBuilder = asmBuilder.DefineDynamicModule(asmName.Name);
    }

    private static TypeBuilder CreateTypeBuilder(
        ModuleBuilder modBuilder,
        Type sourceType,
        Type? parentType = null,
        TypeAttributes attribs =
            TypeAttributes.Public |
            TypeAttributes.Class |
            TypeAttributes.AutoClass |
            TypeAttributes.AnsiClass |
            TypeAttributes.BeforeFieldInit |
            TypeAttributes.AutoLayout,
        params Type[] interfaces)
    {
        return modBuilder.DefineType(
            sourceType.Name,
            attribs,
            parentType ??= ObjectExts.Types.Object,
            interfaces);
    }

    private static ConstructorBuilder CreateConstructorBuilder(
        TypeBuilder tb,
        MethodAttributes attribs =
            MethodAttributes.Public |
            MethodAttributes.SpecialName |
            MethodAttributes.RTSpecialName,
        params Type[]? parameterTypes)
    {
        return tb.DefineConstructor(
            attribs,
            CallingConventions.Standard,
            parameterTypes ?? Type.EmptyTypes);
    }

    private static MethodBuilder CreateMethodBuilder(
        TypeBuilder tb,
        string methodName,
        Type returnType,
        MethodAttributes attribs =
            MethodAttributes.Public |
            MethodAttributes.SpecialName |
            MethodAttributes.HideBySig,
        params Type[]? parameterTypes)
    {
        if (attribs.HasFlag(MethodAttributes.Public))
        {
            attribs &= ~MethodAttributes.Private;
        }

        var _methodName = new List<string> { SanitizeName(methodName) };

        if (attribs.HasFlag(MethodAttributes.Private))
        {
            _methodName.Insert(0, "_");
        }

        return tb.DefineMethod(
            string.Concat(_methodName),
            attribs,
            returnType,
            parameterTypes ?? Type.EmptyTypes);
    }

    private static FieldBuilder CreateFieldBuilder(
        TypeBuilder tb,
        string fieldName,
        Type fieldType,
        FieldAttributes attribs =
            FieldAttributes.Public)
    {
        if (attribs.HasFlag(FieldAttributes.Public))
        {
            attribs &= ~FieldAttributes.Private;
        }

        var _fieldName = new List<string> { SanitizeName(fieldName) };

        if (attribs.HasFlag(FieldAttributes.Private))
        {
            _fieldName.Insert(0, "_");
        }

        return tb.DefineField(string.Concat(_fieldName), fieldType, attribs);
    }

    private static PropertyBuilder CreatePropertyBuilder(
        TypeBuilder tb,
        string propertyName,
        Type propertyType,
        PropertyAttributes attribs =
            PropertyAttributes.HasDefault,
        bool isPrivate = false)
    {
        var _propertyName = new List<string> { SanitizeName(propertyName) };

        if (isPrivate)
        {
            _propertyName.Insert(0, "_");
        }

        return tb.DefineProperty(string.Concat(_propertyName), attribs, propertyType, null);
    }

    private static void CreateProperty(
        TypeBuilder tb,
        string propertyName,
        Type propertyType,
        MethodAttributes attribs =
            MethodAttributes.Public |
            MethodAttributes.SpecialName |
            MethodAttributes.HideBySig,
        params Type[]? parameterTypes)
    {
        if (attribs.HasFlag(MethodAttributes.Public))
        {
            attribs &= ~MethodAttributes.Private;
        }

        var _propertyName = new List<string> { propertyName };

        if (attribs.HasFlag(MethodAttributes.Private))
        {
            _propertyName.Insert(0, "_");
        }

        var fieldBuilder = tb.DefineField(string.Concat(_propertyName), propertyType, FieldAttributes.Private);
        var propertyBuilder = tb.DefineProperty(string.Concat(_propertyName), PropertyAttributes.HasDefault, propertyType, null);
        var getPropMthdBldr = tb.DefineMethod(
            "get_" + propertyName,
            MethodAttributes.Public |
            MethodAttributes.SpecialName |
            MethodAttributes.HideBySig,
            propertyType,
            parameterTypes ?? Type.EmptyTypes);
        var getIl = getPropMthdBldr.GetILGenerator();

        getIl.Emit(OpCodes.Ldarg_0);
        getIl.Emit(OpCodes.Ldfld, fieldBuilder);
        getIl.Emit(OpCodes.Ret);

        var setPropMthdBldr =
            tb.DefineMethod(
                "set_" + propertyName,
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.HideBySig,
                null, [propertyType]);

        var setIl = setPropMthdBldr.GetILGenerator();
        var modifyProperty = setIl.DefineLabel();
        var exitSet = setIl.DefineLabel();

        setIl.MarkLabel(modifyProperty);
        setIl.Emit(OpCodes.Ldarg_0);
        setIl.Emit(OpCodes.Ldarg_1);
        setIl.Emit(OpCodes.Stfld, fieldBuilder);

        setIl.Emit(OpCodes.Nop);
        setIl.MarkLabel(exitSet);
        setIl.Emit(OpCodes.Ret);

        propertyBuilder.SetGetMethod(getPropMthdBldr);
        propertyBuilder.SetSetMethod(setPropMthdBldr);
    }
}