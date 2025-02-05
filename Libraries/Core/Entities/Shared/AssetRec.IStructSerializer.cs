using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces.Data;

namespace ThePalace.Core.Entities.Shared
{
    public partial class AssetRec : IStructSerializer
    {
        public void Deserialize(ref int refNum, Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            throw new NotImplementedException();
        }

        public void Serialize(ref int refNum, Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            throw new NotImplementedException();
        }
    }
}