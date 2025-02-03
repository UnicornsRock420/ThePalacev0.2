using System.Reflection;
using System.Runtime.InteropServices;

namespace System
{
    public static class TypeExts
    {
        private static readonly BindingFlags _bindingFlags1;

        public static class Types
        {
            public static readonly Type Type = typeof(Type);
            public static readonly Type TypeArray = typeof(Type[]);
            public static readonly Type TypeList = typeof(List<Type>);
        }

        static TypeExts()
        {
            _bindingFlags1 = BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic;
        }

        public static T StaticMethod<T>(this Type type, string methodName, params object[] args) =>
            (T)type
                ?.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static)
                ?.Invoke(null, args);

        public static T StaticProperty<T>(this Type type, string methodName) =>
            (T)type
                ?.GetProperty(methodName, BindingFlags.Public | BindingFlags.Static)
                ?.GetValue(null, null);

        public static object InvokeMethod<T>(this T source, string methodName, params string[] args) =>
            typeof(T).InvokeMember(methodName, BindingFlags.Default | BindingFlags.InvokeMethod, null, source, args);

        public static IEnumerable<T> FromHierarchy<T>(this T obj, Func<T, T> next, Func<T, bool> @continue = null)
            where T : class
        {
            if (@continue == null)
                @continue = r => r != null;
            for (; @continue(obj);
                obj = next(obj))
                yield return obj;
        }

        public static FieldInfo GetEventField<T>(this T _, string eventName) =>
            GetEventField(typeof(T), eventName);
        public static FieldInfo GetEventField(this Type type, string eventName)
        {
            var _eventName = $"EVENT_{eventName.ToUpperInvariant()}";
            var fieldTypes = (Type[])null;
            var fieldInfo = (FieldInfo)null;

            for (; type != null; type = type.BaseType)
            {
                /* Find events defined as field */
                fieldInfo = type.GetField(eventName, _bindingFlags1);
                fieldTypes = new Type[] { fieldInfo?.FieldType, fieldInfo?.FieldType?.BaseType };
                if (fieldTypes.Contains(DelegateExts.Types.MulticastDelegate)) return fieldInfo;

                /* Find events defined as property { add; remove; } */
                fieldInfo = type.GetField(_eventName, _bindingFlags1);
                if (fieldInfo != null) return fieldInfo;
            }
            return null;
        }

        //public static T ClearBit<T>(this T value, byte bitIndex) where T : struct =>
        //    ClearBit<T>(value, (int)(1 << bitIndex));
        //public static T ClearBit<T>(this T value, long bitValue)
        //    where T : struct
        //{
        //    var type = typeof(T);
        //    switch (type)
        //    {
        //        case Type _t when _t == ByteExts.Types.Byte: goto default;
        //        case Type _t when _t == SByteExts.Types.SByte: goto default;
        //        case Type _t when _t == Int16Exts.Types.Int16: goto default;
        //        case Type _t when _t == UInt16Exts.Types.UInt16: goto default;
        //        case Type _t when _t == Int32Exts.Types.Int32: goto default;
        //        case Type _t when _t == UInt32Exts.Types.UInt32: goto default;
        //        case Type _t when _t == Int64Exts.Types.Int64: goto default;
        //        case Type _t when _t == UInt64Exts.Types.UInt64: goto default;
        //        default: return (value.As<long>() & ~bitValue).As<T>();
        //    }
        //    throw new NotSupportedException($"Type '{type.Name}' is not supported");
        //}
        //public static bool IsBitSet<T>(this T value, byte bitIndex) where T : struct =>
        //    IsBitSet<T>(value, (int)(1 << bitIndex));
        //public static bool IsBitSet<T>(this T value, long bitValue)
        //    where T : struct
        //{
        //    var type = typeof(T);
        //    switch (type)
        //    {
        //        case Type _t when _t == ByteExts.Types.Byte: goto default;
        //        case Type _t when _t == SByteExts.Types.SByte: goto default;
        //        case Type _t when _t == Int16Exts.Types.Int16: goto default;
        //        case Type _t when _t == UInt16Exts.Types.UInt16: goto default;
        //        case Type _t when _t == Int32Exts.Types.Int32: goto default;
        //        case Type _t when _t == UInt32Exts.Types.UInt32: goto default;
        //        case Type _t when _t == Int64Exts.Types.Int64: goto default;
        //        case Type _t when _t == UInt64Exts.Types.UInt64: goto default;
        //        default: return (value.As<long>() & bitValue) == bitValue;
        //    }
        //    throw new NotSupportedException($"Type '{type.Name}' is not supported");
        //}

        public static int TypeID<T>() =>
            (int)TypeCode(typeof(T));
        public static int TypeID<T>(this T _) =>
            (int)TypeCode(typeof(T));
        public static int TypeID(this Type type)
        {
            switch (type)
            {
                case Type _t when _t == Int128Exts.Types.Int128: return (int)TypeCodeEnum.Int128;
                case Type _t when _t == UInt128Exts.Types.UInt128: return (int)TypeCodeEnum.UInt128;
                default: return (int)TypeCode(type);
            }
        }

        public static TypeCodeEnum TypeCode<T>() =>
            TypeCode(typeof(T));
        public static TypeCodeEnum TypeCode<T>(this T _) =>
            TypeCode(typeof(T));
        public static TypeCodeEnum TypeCode(this Type type)
        {
            var result = Type.GetTypeCode(type).As<TypeCodeEnum>();
            switch (result)
            {
                case TypeCodeEnum.Object:
                    if (type == GuidExts.Types.Guid) return TypeCodeEnum.Guid;
                    else if (type == TimeSpanExts.Types.TimeSpan) return TypeCodeEnum.TimeSpan;
                    else goto default;
                default:
                    if (type.IsEnum) return TypeCodeEnum.Enum;
                    else return result;
            }
        }

        public static int SizeOf<T>() =>
            Marshal.SizeOf(typeof(T));
        public static int SizeOf<T>(this T obj) =>
            Marshal.SizeOf(obj);
        public static int SizeOf(this Type type) =>
            Marshal.SizeOf(type);

        public static T[] GetArray<T>(this T value) => new T[] { value };
        public static T[] GetArray<T>(this T value, params T[] values) =>
            (new T[] { value }).Union(values).ToArray();
        public static T[] GetArray<T>(this T value, IEnumerable<T> values) =>
            (new T[] { value }).Union(values).ToArray();
        public static T[] GetArray<T>(this T[] values1, IEnumerable<T> values2) =>
            values1.Union(values2).ToArray();
        public static T[] GetArray<T>(this IEnumerable<T> values1, IEnumerable<T> values2) =>
            values1.Union(values2).ToArray();

        public static List<T> GetList<T>(this T value) =>
            [value];
        public static List<T> GetList<T>(this T value, params T[] values) =>
            new List<T> { value }.Union(values).ToList();
        public static List<T> GetList<T>(this T value, IEnumerable<T> values) =>
            new List<T> { value }.Union(values).ToList();
        public static List<T> GetList<T>(this IEnumerable<T> values1, IEnumerable<T> values2) =>
            values1.Union(values2).ToList();

        public static IReadOnlyList<T> IReadOnlyList<T>(this T value) =>
            new List<T> { value }.AsReadOnly();
        public static IEnumerable<T> IEnumerable<T>(this T value) =>
            new List<T> { value }.AsEnumerable();
        public static IQueryable<T> IQueryable<T>(this T value) =>
            new List<T> { value }.AsQueryable();
        public static void RaiseEvent<T>(this T obj, string eventName, params object[] args)
        {
            args = (args?.Length ?? 0) < 1 ? obj.GetArray(args.AsEnumerable()) : obj.GetArray((object)new EventArgs());

            if (typeof(T).GetField(eventName, BindingFlags.Instance | BindingFlags.NonPublic) is FieldInfo fieldInfo && fieldInfo.GetValue(obj) is MulticastDelegate multicastDelegate)
                foreach (var @delegate in multicastDelegate.GetInvocationList())
                    @delegate.DynamicInvoke(@delegate.Target, args);
        }

        public static T GetInstance<T>()
            where T : class =>
            GetInstance<T>(typeof(T));

        public static T GetInstance<T>(this Type type, params object[] args)
            where T : class =>
            (T)Activator.CreateInstance(type, args);

        public static object GetInstance(this Type type, params object[] args) =>
            Activator.CreateInstance(type, args);


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
            {
                return true;
            }

            return type.IsAssignableFrom(tCompare) ||
                type.IsAssignableTo(tCompare);
        }

        public static bool IsAny(this Type type, params Type[] types)
        {
            if (types.Contains(type))
            {
                return true;
            }

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
            {
                if (!types.Contains(type) &&
                    !(type.IsAssignableFrom(t) || type.IsAssignableTo(t)) &&
                    !(!t.IsInterface || interfaces.Contains(t)))
                    return false;
            }

            return true;
        }
    }
}