using ICSharpCode.SharpZipLib.GZip;
using System.Security.Cryptography;
using System.Text;

namespace System;

public static class ByteExts
{
    public static class Types
    {
        public static readonly Type Byte = typeof(Byte);
        public static readonly Type ByteArray = typeof(Byte[]);
        public static readonly Type ByteList = typeof(List<Byte>);
    }

    //static ByteExts() { }

    public static byte[] GetBytes(this byte value) => [value];

    public static byte[] EnsureBigEndian(this byte[] value)
    {
        if (BitConverter.IsLittleEndian)
            Array.Reverse(value);
        return value;
    }

    public static string ToHex(this byte[] value, bool prefix = false)
    {
        var max = value.Length;
        var result = new StringBuilder(max * 2);
        for (var j = 0; j < max; j++)
            result.AppendFormat(prefix ? "0x{0:X2}" : "{0:X2}", value[j]);
        return result.ToString();
    }

    public static string ToBase64(this byte[] value) =>
        (value?.Length ?? 0) < 1 ? null : Convert.ToBase64String(value);

    public static T FromBase64<T>(this byte[] value)
        where T : class
    {
        if ((value?.Length ?? 0) < 1) return null;

        return value.GetString()
            .FromBase64<T>();
    }

    public static string GZUncompress(this byte[] value)
    {
        using (var memInput = new MemoryStream(value))
        using (var zipInput = new GZipInputStream(memInput))
        using (var memOutput = new MemoryStream())
        {
            zipInput.CopyTo(memOutput);

            return memOutput.GetBuffer().GetString();
        }
    }

    public static uint FromInt31(this byte[] value, int offset = 0)
    {
        if ((value?.Length ?? 0) < sizeof(uint)) return 0;

        value[3] = EnumExts.SetBit<byte, byte>(7, value[3], false);
        return BitConverter.ToUInt32(value, offset);
    }

    public static string GetString(this IEnumerable<byte> value, int limit = 0, int offset = 0) =>
        value.ToArray().GetString(limit, offset);
    public static string GetString(this byte[] input, int limit = 0, int offset = 0)
    {
        if ((input?.Length ?? 0) < 1) return string.Empty;

        if (limit < 0) throw new ArgumentOutOfRangeException(nameof(limit), nameof(limit) + " cannot be less than 0");
        else if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), nameof(offset) + " cannot be less than 0");
        else if ((offset + limit) > input.Length) throw new IndexOutOfRangeException(nameof(offset) + " is out of bounds of the source");

        if (limit < 1 || limit > input.Length)
            limit = input.Length;

        return string.Concat(input
            .Skip(offset)
            .Take(limit)
            .Select(b => (char)b));
    }

    public static List<byte> GetRange(this IEnumerable<byte> value, int max = 0, int offset = 0) =>
        value.ToArray().GetRange(max, offset);
    public static List<byte> GetRange(this byte[] value, int max = 0, int offset = 0)
    {
        if (max < 1)
        {
            max = 0;
        }
        if (max > value.Length)
        {
            max = value.Length;
        }
        if (offset < 1)
        {
            offset = 0;
        }

        if (max > 0)
        {
            return value
                .Skip(offset)
                .Take(max)
                .ToList();
        }
        else
        {
            return value
                .Skip(offset)
                .ToList();
        }
    }

    public static void AddRange(this List<byte> dest, IEnumerable<byte> source, int max = 0, int offset = 0) =>
        dest.AddRange(source.ToArray(), max, offset);
    public static void AddRange(this List<byte> dest, byte[] source, int max = 0, int offset = 0)
    {
        if (max < 1)
        {
            max = 0;
        }
        if (max > source.Length)
        {
            max = source.Length;
        }
        if (offset < 1)
        {
            offset = 0;
        }

        var range = new List<byte>();

        if (max > 0)
        {
            range = source
                .Skip(offset)
                .Take(max)
                .ToList();
        }
        else
        {
            range = source
                .Skip(offset)
                .ToList();
        }

        dest.AddRange(range);
    }

    public static string ComputeMd5(this byte[] value)
    {
        using (var md5 = MD5.Create())
            return string.Concat(
                md5.ComputeHash(value)
                    .Select(b => b.ToString("X2")));
    }

    public static char[] GetChars(this string value, int limit = 0, int offset = 0) =>
        value.GetBytes().GetChars(limit, offset);
    public static char[] GetChars(this IEnumerable<byte> value, int limit = 0, int offset = 0) =>
        value.ToArray().GetChars(limit, offset);
    public static char[] GetChars(this byte[] value, int limit = 0, int offset = 0)
    {
        if ((value?.Length ?? 0) < 1) return [];

        if (limit < 0) throw new ArgumentOutOfRangeException(nameof(limit), nameof(limit) + " cannot be less than 0");
        if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), nameof(offset) + " cannot be less than 0");
        if ((offset + limit) > value.Length) throw new IndexOutOfRangeException(nameof(offset) + " is out of bounds of the source");

        if (limit < 1 || limit > value.Length)
            limit = value.Length;

        return value
            .Skip(offset)
            .Take(limit)
            .Select(b => (char)b)
            .ToArray();
    }
}