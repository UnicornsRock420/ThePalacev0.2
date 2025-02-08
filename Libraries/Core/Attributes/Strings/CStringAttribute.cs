using sint32 = System.Int32;

namespace ThePalace.Core.Attributes.Strings
{
    public class CStringAttribute(sint32 maxStringLength = 0x7FFF) : Attribute
    {
        private readonly sint32 _maxStringLength = maxStringLength;

        public sint32 MaxStringLength => _maxStringLength;
    }
}