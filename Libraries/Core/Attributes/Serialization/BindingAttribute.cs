using System.Reflection;

namespace ThePalace.Core.Attributes.Serialization
{
    public abstract class BindingAttribute : Attribute
    {
        public BindingAttribute(Type type, params string[] propertyNames)
        {
            _properties = type
                .GetProperties()
                .Where(p => propertyNames.Contains(p.Name))
                .Cast<MemberInfo>()
                .Union(type
                    .GetFields()
                    .Where(f => propertyNames.Contains(f.Name))
                    .Cast<MemberInfo>())
                .ToArray();
        }

        private readonly MemberInfo[] _properties;

        public MemberInfo[] Properties => _properties;
    }
}