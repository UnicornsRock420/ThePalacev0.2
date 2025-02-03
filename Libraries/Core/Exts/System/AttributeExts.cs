using System.ComponentModel;

namespace System
{
    public static class AttributeExts
    {
        public static class Types
        {
            public static readonly Type Attribute = typeof(Attribute);
            public static readonly Type AttributeArray = typeof(Attribute[]);
            public static readonly Type AttributeList = typeof(List<Attribute>);
            public static readonly Type DescriptionAttribute = typeof(DescriptionAttribute);
        }

        public static string? GetDescriptio<T>(this T? value)
        {
            var type = value?.GetType() ?? typeof(T);
            switch (type)
            {
                case Type _t when _t is Type:
                    return _t
                        ?.GetCustomAttributes(Types.DescriptionAttribute, false)
                        ?.Cast<DescriptionAttribute>()
                        ?.Select(a => a.Description)
                        ?.FirstOrDefault();
                case Type _e when _e is Enum || _e.IsEnum:
                    var key = _e?.ToString();
                    if (key == null) return null;

                    return type
                        ?.GetField(key)
                        ?.GetCustomAttributes(Types.DescriptionAttribute, false)
                        ?.Cast<DescriptionAttribute>()
                        ?.Select(a => a.Description)
                        ?.FirstOrDefault() ?? key;
            }

            return null;
        }

        //static AttributeExts() { }
    }
}