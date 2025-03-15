using ThePalace.Core.Entities.IO;
using ThePalace.Core.Entities.Shared.Assets;
using ThePalace.Core.Factories.Filesystem;

namespace ThePalace.Core.Factories.IO
{
    public class PropPIDSStream : StreamBase
    {
        public PropPIDSStream()
        {
        }

        ~PropPIDSStream()
        {
            Dispose();
        }

        public void Read(PropPROPSStream filePROPSReader, out List<AssetRec> assets)
        {
            assets = new();

            var sizeFilePIDSHeaderRec = Exts.AttributeExts.GetByteSize<FilePIDSHeaderRec>();

            var fileSize = filePROPSReader.Filesize;
            var _fileHeader = new FilePIDSHeaderRec();
            var data = new byte[sizeFilePIDSHeaderRec];
            var read = 0;

            _fileStream.Seek(0, SeekOrigin.Begin);

            while (true)
            {
                read = _fileStream.Read(data, 0, sizeFilePIDSHeaderRec);

                if (read == 0 ||
                    read < sizeFilePIDSHeaderRec) break;

                else if (read == sizeFilePIDSHeaderRec)
                {
                    var asset = (AssetRec?)null;
                    try
                    {
                        //using (var tmp = new MemoryStream(data))
                        //    _fileHeader.Deserialize(tmp);

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
                //_fileStream.PalaceSerialize(
                //    0,
                //    new FilePIDSHeaderRec(asset.AssetSpec)
                //    {
                //        dataSize = dataSize,
                //        dataOffset = dataOffset,
                //    });
                dataOffset += dataSize;
            }
        }
    }
}