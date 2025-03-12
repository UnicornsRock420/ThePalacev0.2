using System.Runtime.Serialization;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using ThePalace.Core.Constants;
using ThePalace.Logging.Entities;
using Point = ThePalace.Core.Entities.Shared.Types.Point;
using sint16 = short;

namespace ThePalace.Client.Desktop.Entities.Shared.Assets;

public class AssetDesc : ThePalace.Core.Entities.Shared.Assets.AssetDesc
{
    [IgnoreDataMember] public string? Format;

    [IgnoreDataMember] public sint16 Height;

    [IgnoreDataMember] public Bitmap Image;

    [IgnoreDataMember] public Point Offset;

    [IgnoreDataMember] public sint16 Width;

    ~AssetDesc()
    {
        Dispose();
    }

    public override void Dispose()
    {
        try
        {
            Image?.Dispose();
            Image = null;
        }
        catch
        {
        }

        base.Dispose();

        GC.SuppressFinalize(this);
    }

    public static async Task<Bitmap> Render(AssetDesc asset)
    {
        if (asset.AssetRec.IsCustom32Bit)
            return RenderCustom32bit(asset);
        if (asset.AssetRec.IsLegacy32Bit)
            return RenderLegacy32bit(asset);
        if (asset.AssetRec.IsLegacy16Bit)
            return RenderLegacy16bit(asset);
        if (asset.AssetRec.IsLegacyS20Bit)
            return RenderLegacyS20bit(asset);
        if (asset.AssetRec.IsLegacy20Bit)
            return RenderLegacy20bit(asset);
        return RenderLegacy8bit(asset);
    }

    private static Bitmap RenderLegacy8bit(AssetDesc asset)
    {
        var result = new Bitmap(asset.Width, asset.Height);
        if (result == null) throw new OutOfMemoryException();

        var pixelIndex = 0;
        var counter = 0;
        var ofst = 0;

        for (var y = asset.Height - 1; ofst < asset.Data.Length && y >= 0; y--)
        for (var x = (int)asset.Width; ofst < asset.Data.Length && x > 0;)
        {
            var cb = asset.Data[ofst++];
            var pc = (byte)(cb & 0x0F);
            var mc = (byte)(cb >> 4);
            x -= mc + pc;

            if (x < 0 ||
                counter++ > 6000)
                throw new Exception("Bad Prop");

            pixelIndex += mc;

            while (pc-- > 0 &&
                   ofst < asset.Data.Length)
            {
                cb = asset.Data[ofst++];

                var _x = pixelIndex % asset.Width;
                var _y = pixelIndex / asset.Height % asset.Height;
                var colour = Color.FromArgb((int)AssetConstants.PalacePalette[cb]);

                result.SetPixel(_x, _y, colour);

                pixelIndex++;
            }
        }

        return result;
    }

    private static Bitmap RenderLegacy16bit(AssetDesc asset)
    {
        var result = new Bitmap(asset.Width, asset.Height);
        if (result == null) throw new OutOfMemoryException();

        var inflatedData = InflateData(asset.Data) ?? asset.Data;

        if (inflatedData == null ||
            inflatedData.Length < 1936 * 2)
            throw new Exception("Bad Prop");

        // Implementation thanks to Phalanx team
        // Translated from C++ implementation
        // Translated from ActionScript implementation (Turtle)

        var ditherS20bit = 255 / 31;
        var colour = Color.White;
        var ofst = 0;
        var x = 0;
        var y = 0;
        var a = 0;
        var r = 0;
        var g = 0;
        var b = 0;
        var C = 0;

        for (x = 0; x < 1936; x++)
        {
            ofst = x * 2;

            C = (inflatedData[ofst] << 8) | inflatedData[ofst + 1];
            r = (((inflatedData[ofst] >> 3) & 31) * ditherS20bit) & 0xFF;
            g = (((C >> 6) & 31) * ditherS20bit) & 0xFF;
            b = (((C >> 1) & 31) * ditherS20bit) & 0xFF;
            a = ((C & 1) * 255) & 0xFF;

            colour = Color.FromArgb(a, r, g, b);
            var _x = y % asset.Width;
            var _y = y / asset.Height;
            result.SetPixel(_x, _y, colour);
            y++;
        }

        return result;
    }

    private static Bitmap RenderLegacy20bit(AssetDesc asset)
    {
        var result = new Bitmap(asset.Width, asset.Height);
        if (result == null) throw new OutOfMemoryException();

        var inflatedData = InflateData(asset.Data) ?? asset.Data;

        if (inflatedData == null ||
            inflatedData.Length < 968 * 5)
            throw new Exception("Bad Prop");

        // Implementation thanks to Phalanx team
        // Translated from C++ implementation
        // Translated from ActionScript implementation (Turtle)

        var dither20bit = 255 / 63;
        var colour = Color.White;
        var ofst = 0;
        var x = 0;
        var y = 0;
        var a = 0;
        var r = 0;
        var g = 0;
        var b = 0;
        var C = 0;

        for (x = 0, y = 0; x < 968; x++)
        {
            ofst = x * 5;

            r = ((inflatedData[ofst] >> 2) & 63) * dither20bit;
            C = (inflatedData[ofst] << 8) | inflatedData[ofst + 1];
            g = ((C >> 4) & 63) * dither20bit;
            C = (inflatedData[ofst + 1] << 8) | inflatedData[ofst + 2];
            b = ((C >> 6) & 63) * dither20bit;
            a = ((C >> 4) & 3) * 85;

            colour = Color.FromArgb(a, r, g, b);
            var _x = y % asset.Width;
            var _y = y / asset.Height;
            result.SetPixel(_x, _y, colour);
            y++;

            C = (inflatedData[ofst + 2] << 8) | inflatedData[ofst + 3];
            r = ((C >> 6) & 63) * dither20bit;
            g = (C & 63) * dither20bit;
            C = inflatedData[ofst + 4];
            b = ((C >> 2) & 63) * dither20bit;
            a = (C & 3) * 85;

            colour = Color.FromArgb(a, r, g, b);
            _x = y % asset.Width;
            _y = y / asset.Height;
            result.SetPixel(_x, _y, colour);
            y++;
        }

        return result;
    }

    private static Bitmap RenderLegacyS20bit(AssetDesc asset)
    {
        var result = new Bitmap(asset.Width, asset.Height);
        if (result == null) throw new OutOfMemoryException();

        var inflatedData = InflateData(asset.Data) ?? asset.Data;

        if (inflatedData == null ||
            inflatedData.Length < 968 * 5)
            throw new Exception("Bad Prop");

        // Implementation thanks to Phalanx team
        // Translated from C++ implementation
        // Translated from ActionScript implementation (Turtle)

        var ditherS20bit = 255 / 31;
        var colour = Color.White;
        var ofst = 0;
        var x = 0;
        var y = 0;
        var a = 0;
        var r = 0;
        var g = 0;
        var b = 0;
        var C = 0;

        for (x = 0, y = 0; x < 968; x++)
        {
            ofst = x * 5;

            r = (((inflatedData[ofst] >> 3) & 31) * ditherS20bit) & 0xFF;
            C = (inflatedData[ofst] << 8) | inflatedData[ofst + 1];
            g = (((C >> 6) & 31) * ditherS20bit) & 0xFF;
            b = (((C >> 1) & 31) * ditherS20bit) & 0xFF;
            C = (inflatedData[ofst + 1] << 8) | inflatedData[ofst + 2];
            a = (((C >> 4) & 31) * ditherS20bit) & 0xFF;

            colour = Color.FromArgb(a, r, g, b);
            var _x = y % asset.Width;
            var _y = y / asset.Height;
            result.SetPixel(_x, _y, colour);
            y++;

            C = (inflatedData[ofst + 2] << 8) | inflatedData[ofst + 3];
            r = (((C >> 7) & 31) * ditherS20bit) & 0xFF;
            g = (((C >> 2) & 31) * ditherS20bit) & 0xFF;
            C = (inflatedData[ofst + 3] << 8) | inflatedData[ofst + 4];
            b = (((C >> 5) & 31) * ditherS20bit) & 0xFF;
            a = ((C & 31) * ditherS20bit) & 0xFF;

            colour = Color.FromArgb(a, r, g, b);
            _x = y % asset.Width;
            _y = y / asset.Height;
            result.SetPixel(_x, _y, colour);
            y++;
        }

        return result;
    }

    private static Bitmap RenderLegacy32bit(AssetDesc asset)
    {
        var inflatedData = InflateData(asset.Data) ?? asset.Data;

        var imageDataSize = asset.Width * asset.Height * 4;
        if (inflatedData == null ||
            inflatedData.Length != imageDataSize)
            throw new Exception("Bad Prop");

        // Implementation thanks to Phalanx team
        // Translated from C++ implementation
        // Translated from ActionScript implementation (Turtle)

        return RenderByteArray(inflatedData);
    }

    private static Bitmap RenderCustom32bit(AssetDesc asset)
    {
        var result = RenderByteArray(asset.Data);
        if (result != null)
            return result;

        return null;
    }

    private static Bitmap RenderByteArray(byte[] data)
    {
        using (var ms = new MemoryStream(data))
        {
            try
            {
                var result = new Bitmap(ms);
                if (result == null) throw new OutOfMemoryException(nameof(AssetDesc) + "." + nameof(RenderByteArray));

                return result;
            }
            catch (Exception ex)
            {
                LoggerHub.Current.Error(ex);

                return null;
            }
        }
    }

    private static byte[] InflateData(byte[] byteInput)
    {
        var types = new[]
        {
            typeof(InflaterInputStream),
            typeof(ZipInputStream),
            typeof(GZipInputStream)
        };

        foreach (var type in types)
            try
            {
                using (var memOutput = new MemoryStream())
                {
                    using (var memInput = new MemoryStream(byteInput))
                    using (var zipInput = type.GetInstance(memInput) as InflaterInputStream)
                    {
                        zipInput.CopyTo(memOutput);
                    }

                    return memOutput.ToArray();
                }
            }
            catch (Exception ex)
            {
                LoggerHub.Current.Error(ex);
            }

        return null;
    }
}