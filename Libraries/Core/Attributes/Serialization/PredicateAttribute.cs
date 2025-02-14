using System.Reflection;
using ThePalace.Core.Enums.App;

namespace ThePalace.Core.Attributes.Serialization
{
    public class PredicateAttribute : Attribute
    {
        public PredicateAttribute(Type type, string propertyName, PredicateOperators @operator, object value)
        {
            _property = type
                .GetProperties()
                .Where(p => p.Name == propertyName)
                .Cast<MemberInfo>()
                .Union(type
                    .GetFields()
                    .Where(f => f.Name == propertyName)
                    .Cast<MemberInfo>())
                .FirstOrDefault();
            _operator = @operator;
            _value = value;
        }

        private readonly MemberInfo? _property;
        private readonly PredicateOperators _operator;
        private readonly object _value;

        public MemberInfo? Property => _property;
        public PredicateOperators Operator => _operator;
        public object Value => _value;
    }
}