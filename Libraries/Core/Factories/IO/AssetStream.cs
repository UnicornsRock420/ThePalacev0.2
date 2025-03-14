using ThePalace.Core.Entities.Shared.Assets;
using ThePalace.Core.Enums;
using ThePalace.Core.Exts;
using ThePalace.Logging.Entities;
using ThePalace.Network.Constants;

namespace ThePalace.Core.Factories.IO;

public class AssetStream : MemoryStream, IDisposable
{
    public AssetRec Asset;

    private uint _chunkMaxSize;

    public AssetStream(uint chunkMaxSize = (uint)NetworkConstants.ASSET_STREAM_BUFFER_SIZE) : base()
    {
        _chunkMaxSize = Math.Max(chunkMaxSize, (uint)NetworkConstants.ASSET_STREAM_BUFFER_SIZE);
    }

    public AssetStream(AssetRec asset, uint chunkMaxSize = (uint)NetworkConstants.ASSET_STREAM_BUFFER_SIZE) : this(chunkMaxSize)
    {
        Asset = asset;

        if (Length < (int)asset.AssetDesc.Size)
        {
            Write(new byte[Length - (int)asset.AssetDesc.Size]);
        }
    }

    public AssetStream(AssetStream chunk)
    {
        if (!chunk.hasData) return;

        var size = (uint)Math.Min(chunk.Length, chunk.Asset.BlockSize);

        chunk.CopyTo(this, (int)size);
    }

    ~AssetStream() => Dispose();

    public AssetRec AsRecord
    {
        get
        {
            var result = new AssetRec
            {
                Type = Asset.Type,
                AssetSpec = Asset.AssetSpec,
                AssetDesc = Asset.AssetDesc,
                Data = ToArray(),
            };

            return result;
        }
    }

    public bool hasData
    {
        get => (Asset.AssetDesc.Size - Length) > 0 && Asset.BlockSize > 0;
    }

    public byte[] Serialize(params object[] values)
    {
        if (!hasData) return null;

        Asset.BlockSize =
            ((uint)Asset.AssetDesc.Size - (uint)Length > _chunkMaxSize)
                ? ((_chunkMaxSize > (uint)Asset.AssetDesc.Size - (uint)Length)
                    ? (uint)Asset.AssetDesc.Size - (uint)Length
                    : (uint)_chunkMaxSize)
                : ((uint)Asset.AssetDesc.Size - (uint)Length > 0)
                    ? (uint)Asset.AssetDesc.Size - (uint)Length
                    : (uint)0;

        try
        {
            using (var ms = new MemoryStream())
            {
                ms.WriteInt32((int)LegacyAssetTypes.RT_PROP);
                Asset.AssetSpec.Serialize(ms);
                ms.WriteInt32((int)Asset.BlockSize);
                ms.WriteInt32((int)Asset.BlockOffset);
                ms.WriteInt16((short)Asset.BlockNbr);
                ms.WriteInt16((short)Asset.NbrBlocks);

                if (Asset.BlockNbr < 1)
                {
                    ms.WriteInt32(Asset.AssetDesc.PropFlags);
                    ms.WriteInt32((int)Asset.AssetDesc.Size);
                    ms.WritePString(Asset.AssetDesc.Name, 32, 1);
                }
                else if (Length >= Asset.AssetDesc.Size)
                {
                    ms.Write(ToArray().Take((int)Asset.BlockSize).ToArray());
                }

                Asset.BlockNbr++;

                return ToArray();   
            }
        }
        catch (Exception ex)
        {
            LoggerHub.Current.Error(ex);
            Dispose();
        }

        return null;
    }
}