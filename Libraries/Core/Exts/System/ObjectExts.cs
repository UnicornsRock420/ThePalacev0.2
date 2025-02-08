using Mapster;
using System.Dynamic;
using System.Runtime.InteropServices;

namespace System
{
    public static class ObjectExts
    {
        public static class Types
        {
            public static readonly Type Object = typeof(Object);
            public static readonly Type ObjectArray = typeof(Object[]);
            public static readonly Type ObjectList = typeof(List<Object>);
            public static readonly Type ExpandoObject = typeof(ExpandoObject);
        }

        //static ObjectExts() { }

        public static int SizeOf(this object obj) =>
            Marshal.SizeOf(obj);

        public static IntPtr ToIntPtr<T>(this T obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj), nameof(obj) + " cannot be null");

            var result = IntPtr.Zero;
            var type = typeof(T);
            if (type.IsClass)
            {
                var handle = null as GCHandle?;
                try
                {
                    handle = GCHandle.Alloc(obj);

                    if (handle.HasValue)
                        result = GCHandle.ToIntPtr(handle.Value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.Trim());
                }
                finally
                {
                    if (handle?.IsAllocated == true)
                        handle?.Free();
                }
            }
            else
            {
                var handle = IntPtr.Zero;

                try
                {
                    handle = Marshal.AllocHGlobal(obj.SizeOf());
                    Marshal.StructureToPtr(obj, result, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.Trim());
                }
                finally
                {
                    if (handle != IntPtr.Zero)
                        Marshal.FreeHGlobal(handle);
                }
            }
            return result;
        }

        public static T FromIntPtr<T>(this IntPtr obj) =>
            (T)GCHandle.FromIntPtr(obj).Target;

        public static void ClearEvents(this object obj, string eventName)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj), nameof(obj) + " cannot be null");
            else if (string.IsNullOrWhiteSpace(eventName)) throw new ArgumentNullException(nameof(eventName), nameof(eventName) + " cannot be null");

            var fi = obj.GetType()?.GetEventField(eventName);
            if (fi == null) throw new NullReferenceException($"Event field {eventName} was not found");

            fi.SetValue(obj, null);
        }

        public static T TryParse<T>(this object? value, T? defaultValue = default(T?), string? format = null) =>
            StringExts.TryParse(value?.ToString(), defaultValue, format);

        public static bool Is<TCompare>(this object obj)
        {
            var type = typeof(TCompare);
            if (type.IsInterface &&
                obj.GetType().GetInterfaces().Contains(type))
            {
                return true;
            }

            return obj is TCompare;
        }
        public static bool Is<TCompare>(this object obj, out TCompare result, TCompare defaultValue = default(TCompare))
        {
            var type = typeof(TCompare);
            if (type.IsInterface)
            {
                if (obj.GetType().GetInterfaces().Contains(type))
                {
                    result = (TCompare)obj;

                    return true;
                }
            }

            if (obj is TCompare)
            {
                result = (TCompare)obj;

                return true;
            }

            result = defaultValue;

            return false;
        }

        public static bool Is(this object obj, Type type) =>
            IsAny(obj, type);

        public static bool IsAny(this object obj, params Type[] types)
        {
            var type = obj.GetType();
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

        public static bool IsAll(this object obj, params Type[] types)
        {
            var type = obj.GetType();
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

        //public static IEnumerable<TCast> As<TCast>(this object[] obj) => obj.Cast<TCast>();
        //public static TCast As<TCast>(this object obj) =>
        //    (new object[] { obj })
        //        .Cast<TCast>()
        //        .FirstOrDefault() ?? default(TCast);

        public static T Clone<T>(this T obj, params string[]? ignorePropertyNames)
        {
            var mapsterConfig = TypeAdapterConfig.GlobalSettings.Clone();
            if ((ignorePropertyNames?.Length ?? 0) > 0)
            {
                foreach (var name in ignorePropertyNames)
                {
                    mapsterConfig.Default.Ignore(name);
                }
            }
            var entity = obj.Adapt<T>(mapsterConfig);
            return (T)entity;
        }
    }
}