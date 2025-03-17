using System.Reflection;
using System.Runtime.InteropServices;

namespace System;

public static class TypeExts
{
    private static readonly BindingFlags _bindingFlags1;

    static TypeExts()
    {
        _bindingFlags1 = BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic;
    }

    public static T StaticMethod<T>(this Type type, string methodName, params object[] args)
    {
        return (T)type
            ?.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static)
            ?.Invoke(null, args);
    }

    public static T StaticProperty<T>(this Type type, string methodName)
    {
        return (T)type
            ?.GetProperty(methodName, BindingFlags.Public | BindingFlags.Static)
            ?.GetValue(null, null);
    }

    public static object InvokeMethod<T>(this T source, string methodName, params string[] args)
    {
        return typeof(T).InvokeMember(methodName, BindingFlags.Default | BindingFlags.InvokeMethod, null, source, args);
    }

    public static IEnumerable<T> FromHierarchy<T>(this T obj, Func<T, T> next, Func<T, bool> @continue = null)
        where T : class
    {
        if (@continue == null)
            @continue = r => r != null;
        for (;
             @continue(obj);
             obj = next(obj))
            yield return obj;
    }

    public static FieldInfo GetEventField<T>(this T _, string eventName)
    {
        return GetEventField(typeof(T), eventName);
    }

    public static FieldInfo GetEventField(this Type type, string eventName)
    {
        var _eventName = $"EVENT_{eventName.ToUpperInvariant()}";
        var fieldTypes = (Type[])null;
        var fieldInfo = (FieldInfo)null;

        for (; type != null; type = type.BaseType)
        {
            /* Find events defined as field */
            fieldInfo = type.GetField(eventName, _bindingFlags1);
            fieldTypes = [fieldInfo?.FieldType, fieldInfo?.FieldType?.BaseType];
            if (fieldTypes.Contains(DelegateExts.Types.MulticastDelegate)) return fieldInfo;

            /* Find events defined as property { add; remove; } */
            fieldInfo = type.GetField(_eventName, _bindingFlags1);
            if (fieldInfo != null) return fieldInfo;
        }

        return null;
    }

    public static int TypeID<T>()
    {
        return (int)TypeCode(typeof(T));
    }

    public static int TypeID<T>(this T _)
    {
        return (int)TypeCode(typeof(T));
    }

    public static int TypeID(this Type type)
    {
        return type switch
        {
            Type _t when _t == Int128Exts.Types.Int128 => (int)TypeCodeEnum.Int128,
            Type _t when _t == UInt128Exts.Types.UInt128 => (int)TypeCodeEnum.UInt128,
            _ => (int)TypeCode(type)
        };
    }

    public static TypeCodeEnum TypeCode<T>()
    {
        return TypeCode(typeof(T));
    }

    public static TypeCodeEnum TypeCode<T>(this T _)
    {
        return TypeCode(typeof(T));
    }

    public static TypeCodeEnum TypeCode(this Type type)
    {
        var result = (TypeCodeEnum)Type.GetTypeCode(type);
        switch (result)
        {
            case TypeCodeEnum.Object:
                if (type == GuidExts.Types.Guid) return TypeCodeEnum.Guid;
                if (type == TimeSpanExts.Types.TimeSpan) return TypeCodeEnum.TimeSpan;
                goto default;
            default:
                return type.IsEnum ? TypeCodeEnum.Enum : result;
        }
    }

    public static int SizeOf<T>()
    {
        return Marshal.SizeOf(typeof(T));
    }

    public static int SizeOf<T>(this T obj)
    {
        return Marshal.SizeOf(obj);
    }

    public static int SizeOf(this Type type)
    {
        return Marshal.SizeOf(type);
    }

    public static T[] GetArray<T>(this T value)
    {
        return [value];
    }

    public static T[] GetArray<T>(this T value, params T[] values)
    {
        return new[] { value }.Union(values).ToArray();
    }

    public static T[] GetArray<T>(this T value, IEnumerable<T> values)
    {
        return new[] { value }.Union(values).ToArray();
    }

    public static T[] GetArray<T>(this T[] values1, IEnumerable<T> values2)
    {
        return values1.Union(values2).ToArray();
    }

    public static T[] GetArray<T>(this IEnumerable<T> values1, IEnumerable<T> values2)
    {
        return values1.Union(values2).ToArray();
    }

    public static List<T> GetList<T>(this T value)
    {
        return [value];
    }

    public static List<T> GetList<T>(this T value, params T[] values)
    {
        return new List<T> { value }.Union(values).ToList();
    }

    public static List<T> GetList<T>(this T value, IEnumerable<T> values)
    {
        return new List<T> { value }.Union(values).ToList();
    }

    public static List<T> GetList<T>(this IEnumerable<T> values1, IEnumerable<T> values2)
    {
        return values1.Union(values2).ToList();
    }

    public static IReadOnlyList<T> IReadOnlyList<T>(this T value)
    {
        return new List<T> { value }.AsReadOnly();
    }

    public static IEnumerable<T> IEnumerable<T>(this T value)
    {
        return new List<T> { value }.AsEnumerable();
    }

    public static IQueryable<T> IQueryable<T>(this T value)
    {
        return new List<T> { value }.AsQueryable();
    }

    public static void RaiseEvent<T>(this T obj, string eventName, params object[] args)
    {
        args = (args?.Length ?? 0) < 1 ? obj.GetArray(args.AsEnumerable()) : obj.GetArray((object)new EventArgs());

        if (typeof(T).GetField(eventName, BindingFlags.Instance | BindingFlags.NonPublic) is FieldInfo fieldInfo &&
            fieldInfo.GetValue(obj) is MulticastDelegate multicastDelegate)
            foreach (var @delegate in multicastDelegate.GetInvocationList())
                @delegate.DynamicInvoke(@delegate.Target, args);
    }

    public static T GetInstance<T>()
        where T : class
    {
        return GetInstance<T>(typeof(T));
    }

    public static T GetInstance<T>(this Type type, params object[] args)
        where T : class
    {
        return (T)Activator.CreateInstance(type, args);
    }

    public static object GetInstance(this Type type, params object[] args)
    {
        return Activator.CreateInstance(type, args);
    }


    public static object TypeAs(this Type type, object obj)
    {
        try
        {
            return Convert.ChangeType(obj, type);
        }
        catch
        {
            return null;
        }
    }

    public static bool Is<TCompare>(this Type type)
    {
        var tCompare = typeof(TCompare);

        if (type.IsInterface &&
            type.GetInterfaces().Contains(tCompare))
            return true;

        return type.IsAssignableFrom(tCompare) ||
               type.IsAssignableTo(tCompare);
    }

    public static bool IsAny(this Type type, params Type[] types)
    {
        if (types.Contains(type)) return true;

        var interfaces = type.GetInterfaces();
        foreach (var t in types)
        {
            if (type.IsAssignableFrom(t) ||
                type.IsAssignableTo(t))
                return true;

            if (t.IsInterface &&
                interfaces.Contains(t))
                return true;
        }

        return false;
    }

    public static bool IsAll(this Type type, params Type[] types)
    {
        var interfaces = type.GetInterfaces();
        foreach (var t in types)
            if (!types.Contains(type) &&
                !(type.IsAssignableFrom(t) || type.IsAssignableTo(t)) &&
                !(!t.IsInterface || interfaces.Contains(t)))
                return false;

        return true;
    }

    public static class Types
    {
        public static readonly Type Type = typeof(Type);
        public static readonly Type TypeArray = typeof(Type[]);
        public static readonly Type TypeList = typeof(List<Type>);
    }
}