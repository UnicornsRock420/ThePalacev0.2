using sint32 = System.Int32;

namespace ThePalace.Core.Attributes
{
    public class PStringAttribute(sint32 lengthByteSize = 1, sint32 maxStringLength = 255, sint32 paddingModulo = 0) : Attribute
    {
        private readonly sint32 _lengthByteSize = lengthByteSize;
        private readonly sint32 _maxStringLength = maxStringLength;
        private readonly sint32 _paddingModulo = paddingModulo;

        public sint32 LengthByteSize => _lengthByteSize;
        public sint32 MaxStringLength => _maxStringLength;
        public sint32 PaddingModulo => _paddingModulo;
    }
}