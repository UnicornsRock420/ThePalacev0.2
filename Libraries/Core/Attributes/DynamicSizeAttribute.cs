using sint32 = System.Int32;

namespace ThePalace.Core.Attributes
{
    public class DynamicSizeAttribute(sint32 maxByteSize = 0, sint32 minByteSize = 0) : Attribute
    {
        private readonly sint32 _minByteSize = minByteSize <= 0 ? 0 : minByteSize;
        private readonly sint32 _maxByteSize = maxByteSize <= minByteSize ? minByteSize : maxByteSize;

        public sint32 MaxByteSize => _maxByteSize;
        public sint32 MinByteSize => _minByteSize;
    }
}