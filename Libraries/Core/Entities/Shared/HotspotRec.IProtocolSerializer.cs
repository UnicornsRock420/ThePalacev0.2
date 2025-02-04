using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Shared
{
    public partial class HotspotRec : IProtocolSerializer
    {
        public void Deserialize(int refNum, Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            throw new NotImplementedException();
        }

        public void Serialize(out int refNum, Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            throw new NotImplementedException();
        }
    }
}