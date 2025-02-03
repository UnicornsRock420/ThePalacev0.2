using sint32 = System.Int32;

namespace ThePalace.Core.Attributes
{
    public class ByteSizeAttribute(int byteSize) : Attribute
    {
        private readonly sint32 _byteSize = byteSize;

        public sint32 ByteSize => _byteSize;
    }
}