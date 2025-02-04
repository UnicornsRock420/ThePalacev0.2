using sint32 = System.Int32;

namespace ThePalace.Core.Attributes
{
    public class CStringAttribute(sint32 maxStringLength = 0) : Attribute
    {
        private readonly sint32 _maxStringLength = maxStringLength;

        public sint32 MaxStringLength => _maxStringLength;
    }
}