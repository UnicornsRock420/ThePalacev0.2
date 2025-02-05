using ThePalace.Core.Entities.Filesystem;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Factories;

namespace ThePalace.Core.Types
{
    public partial class PropPIDSStream : StreamBase
    {
        public PropPIDSStream() { }
        ~PropPIDSStream() =>
            Dispose(false);

        public void Read(PropPROPSStream filePROPSReader, out List<AssetRec> assets)
        {
            assets = new();

            var fileSize = filePROPSReader.Filesize;
            var _fileHeader = new FilePIDSHeaderRec();
            var data = new byte[FilePIDSHeaderRec.SizeOf];
            var read = 0;

            _fileStream.Seek(0, SeekOrigin.Begin);

            while (true)
            {
                read = _fileStream.Read(data, 0, FilePIDSHeaderRec.SizeOf);

                if (read == 0 ||
                    read < FilePIDSHeaderRec.SizeOf) break;

                else if (read == FilePIDSHeaderRec.SizeOf)
                {
                    var asset = null as AssetRec;
                    try
                    {
                        using (var tmp = Packet.FromBytes(data))
                            _fileHeader.Deserialize(tmp);

                        if (_fileHeader.dataOffset > fileSize ||
                            _fileHeader.dataOffset + _fileHeader.dataSize > fileSize)
                            continue;

                        asset = new()
                        {
                            AssetSpec = _fileHeader.AssetSpec,
                        };

                        filePROPSReader.Read(_fileHeader.dataOffset, _fileHeader.dataSize, ref asset);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                    if (asset != null)
                        assets.Add(asset);

                    continue;
                }

                else break;
            }
        }

        public void Write(PropPROPSStream filePROPSReader, params AssetRec[] assets)
        {
            var dataOffset = 0;

            foreach (var asset in assets)
            {
                var dataSize = filePROPSReader.Write(asset);
                _fileStream.Write(
                    new FilePIDSHeaderRec(asset.AssetSpec)
                    {
                        dataSize = dataSize,
                        dataOffset = dataOffset,
                    }.Serialize());
                dataOffset += dataSize;
            }
        }
    }
}
