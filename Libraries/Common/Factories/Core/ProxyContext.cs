using System.Collections.Concurrent;
using System.Dynamic;
using System.Reflection;
using System.Reflection.Emit;
using Lib.Common.Entities.EventArgs;

namespace Lib.Common.Factories.Core;

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
    #region SubClasses

    protected sealed class ProxyInfo : IDisposable
    {
        private ProxyInfo()
        {
        }

        internal ProxyInfo(
            Type sourceType,
            Type proxyType,
            TypeBuilder typeBuilder,
            ProxyOptions opts = ProxyOptions.CloneDefault)
        {
            Options = opts;
            SourceType = sourceType;
            ProxyType = proxyType;
            TypeBuilder = typeBuilder;
        }

        ~ProxyInfo()
        {
            Dispose();
        }

        internal ProxyOptions Options { get; }
        internal Type SourceType { get; private set; }
        internal Type ProxyType { get; private set; }
        internal TypeBuilder TypeBuilder { get; private set; }

        public void Dispose()
        {
            SourceType = null;
            ProxyType = null;
            TypeBuilder = null;

            GC.SuppressFinalize(this);
        }
    }

    #endregion

    #region cStrs

    static ProxyContext()
    {
        _assemblyName ??= CreateAssemblyName(CONST_STR_AssemblyName);
        _assemblyBuilder ??= CreateAssemblyBuilder(_assemblyName);
        _moduleBuilder ??= CreateModuleBuilder(_assemblyName, _assemblyBuilder);
    }

    internal ProxyContext()
    {
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

    #endregion

    #region Fields/Properties

    protected static readonly Type CONST_TYPE_ProxyContext = typeof(ProxyContext);
    protected static readonly Type CONST_TYPE_Object = typeof(object);
    protected static readonly Type CONST_TYPE_Void = typeof(void);
    protected const string CONST_STR_AssemblyName = "ThePalace." + nameof(ProxyContext);
    protected const string CONST_STR_GetterPrefix = "get_";
    protected const string CONST_STR_SetterPrefix = "set_";

    protected static readonly ConcurrentDictionary<Type, ProxyInfo> _proxies = new();

    protected static AssemblyName _assemblyName;
    protected static AssemblyBuilder _assemblyBuilder;
    protected static ModuleBuilder _moduleBuilder;

    public static event EventHandler HookEvents;
    public static event EventHandler HookExceptions;

    protected readonly ProxyInfo _proxyInfo;

    #endregion

    #region Proxy Methods

    protected static ProxyInfo Build<T>(ProxyOptions opts = ProxyOptions.CloneDefault)
    {
        return Build(typeof(T), opts);
    }

    protected static ProxyInfo Build(Type sourceType, ProxyOptions opts = ProxyOptions.CloneDefault)
    {
        ArgumentNullException.ThrowIfNull(sourceType, nameof(sourceType));

        var nfo = new List<MemberInfo>();
        nfo.AddRange(sourceType.GetConstructors());
        nfo.AddRange(sourceType.GetFields());
        nfo.AddRange(sourceType.GetProperties());
        nfo.AddRange(sourceType.GetMethods());

        var typeBuilder = (TypeBuilder?)CreateTypeBuilder(
            _moduleBuilder,
            sourceType.Name,
            CONST_TYPE_ProxyContext);
        if (typeBuilder == null) return null;

        nfo.ForEach(m =>
        {
            switch (m)
            {
                case ConstructorInfo ci:
                    var cb = CreateConstructorBuilder(
                        typeBuilder,
                        (ci.Attributes & ~MethodAttributes.Private)
                        | MethodAttributes.Public
                        | MethodAttributes.Virtual
                        | MethodAttributes.HideBySig,
                        ci.CallingConvention,
                        ci.GetParameters()
                            ?.Select(p => p.ParameterType)
                            ?.ToArray());
                    break;
                case MethodInfo mi:
                    var mb = CreateMethodBuilder(
                        typeBuilder,
                        mi.Name,
                        mi.ReturnType,
                        (mi.Attributes & ~MethodAttributes.Private)
                        | MethodAttributes.Public
                        | MethodAttributes.Virtual
                        | MethodAttributes.HideBySig,
                        mi.GetParameters()
                            ?.Select(p => p.ParameterType)
                            ?.ToArray());
                    break;
                case FieldInfo fi:
                    var fb = CreateFieldBuilder(
                        typeBuilder,
                        fi.Name,
                        fi.FieldType,
                        (fi.Attributes & ~FieldAttributes.Private) | FieldAttributes.Public);
                    break;
                case PropertyInfo pi:
                    var pb = CreatePropertyBuilder(
                        typeBuilder,
                        pi.Name,
                        pi.PropertyType,
                        pi.Attributes);
                    break;
            }
        });

        var proxyType = (Type?)typeBuilder.CreateType();
        if (proxyType == null) return null;

        try
        {
            var proxyInfo = new ProxyInfo(
                sourceType,
                proxyType,
                typeBuilder,
                opts);
            if (proxyInfo == null) throw new OutOfMemoryException(string.Concat([nameof(ProxyContext) + "." + nameof(Build) + ".", sourceType.Name]));

            ProxyContext._proxies.TryAdd(sourceType, proxyInfo);

            return proxyInfo;
        }
        catch (TaskCanceledException ex)
        {
            return null;
        }
        catch (Exception ex)
        {
            HookExceptions?.Invoke(sourceType.Name, new ExceptionEventArgs
            {
                Exception = ex,
            });
        }

        return null;
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
                Exception = ex,
            });
        }
        finally
        {
            HookEvents?.Invoke(null, new MethodInvokedEventArgs
            {
                Method = nfo,
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
                Exception = ex,
            });
        }
        finally
        {
            HookEvents?.Invoke(@ref, new MemberAccessedEventArgs
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
                    HookEvents?.Invoke(@ref, new MemberChangedEventArgs
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
                    Exception = ex,
                });
            }
        }
    }

    #endregion

    #region Generate Methods

    protected static AssemblyName CreateAssemblyName(string? assemblyName)
    {
        if (string.IsNullOrWhiteSpace(assemblyName)) throw new ArgumentNullException(nameof(assemblyName));

        return new AssemblyName(assemblyName);
    }

    protected static AssemblyBuilder CreateAssemblyBuilder(AssemblyName an)
    {
        return AssemblyBuilder.DefineDynamicAssembly(
            an,
            AssemblyBuilderAccess.Run);
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
        Type[]? interfaces = null,
        Dictionary<string, Type[]?> genericParams = null)
    {
        return CreateTypeBuilder(
            mb,
            sourceType.Name,
            parentType ?? CONST_TYPE_Object,
            attribs,
            interfaces ?? Type.EmptyTypes,
            genericParams);
    }


    public static TypeBuilder CreateTypeBuilder(
        ModuleBuilder mb,
        string className,
        Type? parentType = null,
        TypeAttributes attribs =
            TypeAttributes.Public |
            TypeAttributes.Class |
            TypeAttributes.AutoClass |
            TypeAttributes.AnsiClass |
            TypeAttributes.BeforeFieldInit |
            TypeAttributes.AutoLayout,
        Type[]? interfaces = null,
        Dictionary<string, Type[]?> genericParams = null)
    {
        var tb = mb.DefineType(
            className,
            attribs,
            parentType ?? CONST_TYPE_Object,
            interfaces ?? Type.EmptyTypes);

        if ((genericParams?.Count ?? 0) < 1) return tb;

        genericParams = genericParams
            .Where(p =>
                !string.IsNullOrWhiteSpace(p.Key) &&
                (p.Value?.Length ?? 0) > 0)
            .ToDictionary(k => k.Key, v => v.Value);

        var gbs =
            tb.DefineGenericParameters(genericParams.Keys.ToArray());

        // We take each generic type T : class, new()
        foreach (var gb in gbs)
        {
            gb.SetGenericParameterAttributes(
                GenericParameterAttributes.ReferenceTypeConstraint |
                GenericParameterAttributes.DefaultConstructorConstraint);

            var args = genericParams[gb.Name];
            if ((args?.Length ?? 0) > 0)
            {
                gb.SetInterfaceConstraints(args);
            }
        }

        return tb;
    }

    protected static ConstructorBuilder CreateConstructorBuilder(
        TypeBuilder tb,
        ConstructorInfo constructorInfo)
    {
        var attribs = constructorInfo.Attributes;

        if (attribs.HasFlag(MethodAttributes.Private))
        {
            attribs &= ~MethodAttributes.Public;
        }

        return tb.DefineConstructor(
            attribs,
            constructorInfo.CallingConvention,
            constructorInfo.GetParameters()
                ?.Select(p => p.ParameterType)
                ?.ToArray());
    }

    protected static ConstructorBuilder CreateConstructorBuilder(
        TypeBuilder tb,
        MethodAttributes attribs =
            MethodAttributes.Public |
            MethodAttributes.SpecialName |
            MethodAttributes.RTSpecialName,
        CallingConventions callingConvention =
            CallingConventions.Standard,
        params Type[]? parameterTypes)
    {
        if (attribs.HasFlag(MethodAttributes.Private))
        {
            attribs &= ~MethodAttributes.Public;
        }

        return tb.DefineConstructor(
            attribs,
            CallingConventions.Standard,
            parameterTypes ?? Type.EmptyTypes);
    }

    protected static MethodBuilder CreateMethodBuilder(
        TypeBuilder tb,
        MethodInfo methodInfo)
    {
        var attribs = methodInfo.Attributes;

        if (attribs.HasFlag(MethodAttributes.Private))
        {
            attribs &= ~MethodAttributes.Public;
        }

        return CreateMethodBuilder(
            tb,
            methodInfo.Name,
            methodInfo.ReturnType,
            attribs,
            methodInfo.GetParameters()
                ?.Select(p => p.ParameterType)
                ?.ToArray());
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
        if (!attribs.HasFlag(MethodAttributes.Private))
            return tb.DefineMethod(
                methodName,
                attribs,
                returnType,
                parameterTypes ?? Type.EmptyTypes);

        attribs &= ~MethodAttributes.Public;

        return tb.DefineMethod(
            string.Concat(["_", methodName]),
            attribs,
            returnType,
            parameterTypes ?? Type.EmptyTypes);
    }

    protected static DynamicMethod CreateDynamicMethod(
        MethodInfo methodInfo)
    {
        return new DynamicMethod(
            methodInfo.Name,
            methodInfo.ReturnType,
            methodInfo.GetParameters()
                ?.Select(p => p.ParameterType)
                ?.ToArray());
    }

    protected static DynamicMethod CreateDynamicMethod(
        string methodName,
        Type returnType,
        params Type[]? parameterTypes)
    {
        return new DynamicMethod(
            methodName,
            returnType,
            parameterTypes);
    }

    protected static FieldBuilder CreateFieldBuilder(
        TypeBuilder tb,
        FieldInfo fieldInfo)
    {
        var attribs = fieldInfo.Attributes;

        if (attribs.HasFlag(FieldAttributes.Private))
        {
            attribs &= ~FieldAttributes.Public;
        }

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
        if (!attribs.HasFlag(FieldAttributes.Private))
            return tb.DefineField(
                fieldName,
                fieldType,
                attribs);

        attribs &= ~FieldAttributes.Public;

        return tb.DefineField(
            string.Concat(["_", fieldName]),
            fieldType,
            attribs);
    }

    protected static PropertyBuilder CreatePropertyBuilder(
        TypeBuilder tb,
        PropertyInfo propertyInfo,
        bool isPrivate = false)
    {
        return CreatePropertyBuilder(
            tb,
            propertyInfo.Name,
            propertyInfo.PropertyType,
            propertyInfo.Attributes,
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
        return tb.DefineProperty(
            isPrivate
                ? string.Concat(["_", propertyName])
                : propertyName,
            attribs,
            propertyType,
            null);
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
        if (attribs.HasFlag(MethodAttributes.Private))
        {
            attribs &= ~MethodAttributes.Public;
        }

        var fieldBuilder = CreateFieldBuilder(
            tb,
            string.Concat(["_", propertyName]),
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
                string.Concat([CONST_STR_GetterPrefix, propertyName]),
                propertyType,
                attribs,
                null);

        var getIl = getPropertyMB.GetILGenerator();
        getIl.Emit(OpCodes.Ldarg_0);
        getIl.Emit(OpCodes.Ldfld, fieldBuilder);
        getIl.Emit(OpCodes.Ret);

        propertyBuilder.SetGetMethod(getPropertyMB);

        var setPropertyMB =
            CreateMethodBuilder(
                tb,
                string.Concat([CONST_STR_SetterPrefix, propertyName]),
                CONST_TYPE_Void,
                attribs,
                propertyType);

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

        propertyBuilder.SetSetMethod(setPropertyMB);
    }

    #endregion
}