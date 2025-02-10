namespace ThePalace.Core.Attributes.Serialization
{
    public class SizeDependencyAttribute : BindingAttribute
    {
        public SizeDependencyAttribute(Type type, string propertyName) : base(type, propertyName)
        {
        }
    }
}