﻿using System.Collections.Concurrent;
using System.Dynamic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using ThePalace.Common.Entities.EventArgs;

namespace ThePalace.Common.Factories.Core;

public interface ITest123
{
    void PopulateObject(object entity, object data);
}

public class ClsTest123 : ITest123
{
    public void PopulateObject(object entity, object data)
    {
        throw new NotImplementedException();
    }
}

public interface IProxyContext : IDisposable
{
}

[Flags]
public enum ProxyOptions : uint
{
    CloneInstance = 0x0000000001,
    CloneStatic = 0x0000000002,
    ClonePublic = 0x0000000004,
    ClonePrivate = 0x0000000008,
    CloneFields = 0x0000000010,
    CloneProperties = 0x000000020,
    CloneMethods = 0x0000000040,
    CloneDeep = 0x0000000080,

    CaptureExceptions = 0x0000000100,

    // Aliases:
    CloneDefault = CloneInstance | CloneStatic | ClonePublic | CloneFields | CloneProperties | CloneMethods,
}

public class ProxyContext : DynamicObject, IProxyContext
{
    protected class ProxyInfo(
        Type sourceType,
        Type proxyType,
        TypeBuilder typeBuilder,
        ProxyOptions opts = ProxyOptions.CloneDefault) : IDisposable
    {
        public ProxyOptions Options { get; } = opts;
        public Type SourceType { get; } = sourceType;
        public Type ProxyType { get; } = proxyType;

        public TypeBuilder TypeBuilder { get; } = typeBuilder;

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

    protected ProxyContext()
    {
    }

    protected ProxyContext(ProxyInfo proxyInfo)
    {
        _proxyInfo = proxyInfo;
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

    protected ProxyInfo _proxyInfo;

    protected static readonly Type CONST_TYPE_Object = typeof(object);

    protected static readonly ConcurrentDictionary<Type, ProxyInfo> _proxies = new();

    protected static AssemblyName _assemblyName;
    protected static AssemblyBuilder _assemblyBuilder;
    protected static ModuleBuilder _moduleBuilder;

    public static event EventHandler HookEvents; //Accessed, Changed
    public static event EventHandler HookExceptions;

    protected static object Build(Type sourceType, ProxyOptions opts = ProxyOptions.CloneDefault, params object[]? constructorArgs)
    {
        var typeBuilder = (TypeBuilder?)null;

        var proxyType = (Type?)null;
        if (proxyType == null) return null;

        try
        {
            var proxyItem = new ProxyInfo(
                sourceType,
                proxyType,
                typeBuilder,
                opts);
            _proxies.TryAdd(sourceType, proxyItem);

            return proxyItem.Instance;
        }
        catch (TaskCanceledException ex)
        {
            return null;
        }
        catch (Exception ex)
        {
            HookExceptions?.Invoke(sourceType.Name, new ExceptionEventArgs
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Exception = ex,
            });
        }

        return null;
    }

    protected static T Build<T>(ProxyOptions opts = ProxyOptions.CloneDefault, params object[]? constructorArgs)
    {
        return (T)Build(typeof(T), opts, constructorArgs);
    }

    protected static object? Do(Func<object[], object?> cb, params object[] args)
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
            HookExceptions?.Invoke(null, new ExceptionEventArgs
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Exception = ex,
            });
        }

        return null;
    }

    protected static object? Invoke(MethodInfo nfo, object @ref, params object[] args)
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
            HookExceptions?.Invoke(null, new ExceptionEventArgs
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Exception = ex,
            });
        }
        finally
        {
            HookEvents?.Invoke(null, new MethodInvokedEventArgs
            {
                Member = nfo,
                Args = args,
            });
        }

        return null;
    }

    protected static object? Get(MemberInfo nfo, object @ref, params object[] args)
    {
        ArgumentNullException.ThrowIfNull(nfo, nameof(nfo));
        ArgumentNullException.ThrowIfNull(@ref, nameof(@ref));

        try
        {
            var value = (object?)null;

            switch (nfo)
            {
                case MethodInfo mi: throw new ArgumentException(null, nameof(nfo));

                case FieldInfo fi: return fi.GetValue(@ref);
                case PropertyInfo pi: return pi.GetValue(@ref);
            }
        }
        catch (TaskCanceledException ex)
        {
            return null;
        }
        catch (Exception ex)
        {
            HookExceptions?.Invoke(@ref, new ExceptionEventArgs
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Exception = ex,
            });
        }
        finally
        {
            HookEvents?.Invoke(@ref, new FieldAccessedEventArgs
            {
                Member = nfo,
            });
        }

        return null;
    }

    protected static void Set(MemberInfo nfo, object @ref, object? value)
    {
        ArgumentNullException.ThrowIfNull(nfo, nameof(nfo));
        ArgumentNullException.ThrowIfNull(@ref, nameof(@ref));

        var oldValue = (object?)null;

        switch (nfo)
        {
            case MethodInfo mi: throw new ArgumentException(null, nameof(nfo));

            case PropertyInfo pi: oldValue = pi.GetValue(@ref); break;
            case FieldInfo fi: oldValue = fi.GetValue(@ref); break;
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
            HookExceptions?.Invoke(@ref, new ExceptionEventArgs
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
                    HookEvents?.Invoke(@ref, new FieldChangedEventArgs
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
                HookExceptions?.Invoke(@ref, new ExceptionEventArgs
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    Exception = ex,
                });
            }
        }
    }

    protected static readonly Regex CONST_REGEX_Filter_LegalNames = new Regex(@"[^\w\d_]+", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
    protected static readonly Regex CONST_REGEX_Trim_StringStart = new Regex(@"^[\d_]+", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
    protected static readonly Regex CONST_REGEX_Trim_StringEnd = new Regex(@"[_]+$", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
    protected static readonly Regex[] CONST_REGEX_Sequence = [CONST_REGEX_Trim_StringStart, CONST_REGEX_Trim_StringEnd];

    protected static string SanitizeName(string? name)
    {
        name = name?.Trim();

        if (string.IsNullOrWhiteSpace(name)) return string.Empty;

        name = CONST_REGEX_Filter_LegalNames.Replace(name, "_");
        return CONST_REGEX_Sequence.Aggregate(name, (current, regex) => regex.Replace(current, string.Empty));
    }

    protected static AssemblyName CreateAssemblyName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

        return new AssemblyName(name);
    }

    protected static AssemblyBuilder CreateAssemblyBuilder(AssemblyName an)
    {
        return AssemblyBuilder.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
    }

    protected static ModuleBuilder CreateModuleBuilder(
        AssemblyName an,
        AssemblyBuilder ab)
    {
        return ab.DefineDynamicModule(an.Name);
    }

    protected static TypeBuilder CreateTypeBuilder(
        ModuleBuilder mb,
        Type sourceType,
        Type? parentType = null,
        TypeAttributes attribs =
            TypeAttributes.Public |
            TypeAttributes.Class |
            TypeAttributes.AutoClass |
            TypeAttributes.AnsiClass |
            TypeAttributes.BeforeFieldInit |
            TypeAttributes.AutoLayout,
        params Type[]? interfaces)
    {
        return mb.DefineType(
            sourceType.Name,
            attribs,
            parentType ?? CONST_TYPE_Object,
            interfaces ?? Type.EmptyTypes);
    }

    protected static ConstructorBuilder CreateConstructorBuilder(
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

    protected static MethodBuilder CreateMethodBuilder(
        TypeBuilder tb,
        MethodInfo methodInfo,
        MethodAttributes attribs =
            MethodAttributes.Public |
            MethodAttributes.SpecialName |
            MethodAttributes.HideBySig,
        params Type[]? parameterTypes)
    {
        return CreateMethodBuilder(
            tb,
            methodInfo.Name,
            methodInfo.ReturnType,
            attribs,
            parameterTypes ?? Type.EmptyTypes);
    }

    protected static MethodBuilder CreateMethodBuilder(
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

    protected static FieldBuilder CreateFieldBuilder(
        TypeBuilder tb,
        FieldInfo fieldInfo,
        FieldAttributes attribs =
            FieldAttributes.Public)
    {
        return CreateFieldBuilder(
            tb,
            fieldInfo.Name,
            fieldInfo.FieldType,
            attribs);
    }

    protected static FieldBuilder CreateFieldBuilder(
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

    protected static PropertyBuilder CreatePropertyBuilder(
        TypeBuilder tb,
        PropertyInfo propertyInfo,
        PropertyAttributes attribs =
            PropertyAttributes.HasDefault,
        bool isPrivate = false)
    {
        return CreatePropertyBuilder(
            tb,
            propertyInfo.Name,
            propertyInfo.PropertyType,
            attribs,
            isPrivate);
    }

    protected static PropertyBuilder CreatePropertyBuilder(
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

    protected static void CreateProperty(
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

        var fieldBuilder = CreateFieldBuilder(
            tb,
            propertyName,
            propertyType,
            FieldAttributes.Private);
        var propertyBuilder = CreatePropertyBuilder(
            tb,
            propertyName,
            propertyType,
            PropertyAttributes.HasDefault,
            attribs.HasFlag(MethodAttributes.Private));
        var getPropertyMB =
            CreateMethodBuilder(
                tb,
                "get_" + propertyName,
                propertyType,
                attribs,
                parameterTypes ?? Type.EmptyTypes);
        var getIl = getPropertyMB.GetILGenerator();

        getIl.Emit(OpCodes.Ldarg_0);
        getIl.Emit(OpCodes.Ldfld, fieldBuilder);
        getIl.Emit(OpCodes.Ret);

        var setPropertyMB =
            CreateMethodBuilder(
                tb,
                "set_" + propertyName,
                propertyType,
                attribs);

        var setIl = setPropertyMB.GetILGenerator();
        var modifyProperty = setIl.DefineLabel();
        var exitSet = setIl.DefineLabel();

        setIl.MarkLabel(modifyProperty);
        setIl.Emit(OpCodes.Ldarg_0);
        setIl.Emit(OpCodes.Ldarg_1);
        setIl.Emit(OpCodes.Stfld, fieldBuilder);

        setIl.Emit(OpCodes.Nop);
        setIl.MarkLabel(exitSet);
        setIl.Emit(OpCodes.Ret);

        propertyBuilder.SetGetMethod(getPropertyMB);
        propertyBuilder.SetSetMethod(setPropertyMB);
    }
}