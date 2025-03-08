using System.Globalization;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using ThePalace.Common.Constants;
using ThePalace.Common.Enums.System;
using ThePalace.Common.Exts.System.Collections.Generic;
using ThePalace.Common.Exts.System.ComponentModel;

namespace ThePalace.Common.Exts.System;

#region Aliases

using c = Convert;
using db = double;
using i = int;
using l = long;
using o = object;
using s = string;
using tc = TypeCodeEnum;
using ts = TimeSpan;

#endregion

public static class StringExts
{
    private const string CONST_STR_KEY_CANNOT_BE_NULL = "Key cannot be null";
    private const char CHAR_UNDERSCORE = '_';
    private const char CHAR_PERIOD = '.';

    //static StringExts() { }

    #region TryParse<T> Static Methods

    public static T? TryParse<T>(this s? value, T? defaultValue = default, s? format = null)
    {
        if (s.IsNullOrWhiteSpace(value)) return defaultValue;
        var type = typeof(T);
        switch (type)
        {
            case Type _t when _t == Types.String: return (T)(object)value;
            case Type _t when _t == EnumExts.Types.Enum:
                if (Enum.TryParse(type, value, true, out var enumValue)) return (T)(object)enumValue;
                if (!DICTIONARY_ENUM_TYPE_CONVERSION_CACHE.ContainsKey(type))
                {
                    var enumValues = Enum
                        .GetValues(type)
                        .Cast<T>()
                        .Where(v => v.ToString() != v.GetDescription())
                        .Distinct()
                        .ToDictionary(
                            v => v.GetDescription(),
                            v => (o)v);
                    if (enumValues.Count < 1) return defaultValue;
                    DICTIONARY_ENUM_TYPE_CONVERSION_CACHE.Add(type, enumValues);
                }

                if (!DICTIONARY_ENUM_TYPE_CONVERSION_CACHE[type].ContainsKey(value)) return defaultValue;
                return (T)DICTIONARY_ENUM_TYPE_CONVERSION_CACHE[type][value];
            case Type _t when _t == DateTimeExts.Types.DateTime:
                if (!s.IsNullOrWhiteSpace(format) &&
                    DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None,
                        out var datetimeValue)) return (T)(object)datetimeValue;
                goto default;
            case Type _t when _t == TimeSpanExts.Types.TimeSpan:
                var chars = value.Trim().ToLowerInvariant().ToCharArray();
                var cLength = chars.Length;
                if (cLength < 2) goto default;
                var units = chars[(cLength - 2)..].Where(c => CHARARRAY_TIMESPAN_UNITS.Contains(c)).GetString();
                var uLength = units.Length;
                if (uLength < 1) goto default;
                var units0 = units[0];
                value = chars[..(cLength - 1)].TakeWhile(c => (units0 != 't' && c == '.') || char.IsDigit(c))
                    .GetString();
                if (units0 == 't' && l.TryParse(value, out var longValue)) return (T)(object)ts.FromTicks(longValue);
                if (db.TryParse(value, out var doubleValue))
                    switch (units0)
                    {
                        case 'd': return (T)(object)ts.FromDays(doubleValue);
                        case 'h': return (T)(object)ts.FromHours(doubleValue);
                        case 'm':
                            if (uLength > 1 && units[1] == 's') return (T)(object)ts.FromMilliseconds(doubleValue);
                            return (T)(object)ts.FromMinutes(doubleValue);
                        case 's': return (T)(object)ts.FromSeconds(doubleValue);
                        case 'w': return (T)(object)ts.FromDays(doubleValue * 7);
                    }

                goto default;
            default:
                var typeID = type.TypeID();
                if (IREADONLYDICTIONARY_CONVERT_DELEGATES.ContainsKey(typeID))
                    foreach (var func in IREADONLYDICTIONARY_CONVERT_DELEGATES[typeID])
                        try
                        {
                            return (T)(object)func(value);
                        }
                        catch
                        {
                        }

                try
                {
                    return (T)(object)c.ChangeType(value, type, CultureInfo.InvariantCulture);
                }
                catch
                {
                }

                return defaultValue;
        }
    }

    #endregion

    public static string Format(this string format, params object[] args)
    {
        return string.Format(format, args);
    }

    public static string Join(this IEnumerable<string> values, char delimiter)
    {
        return string.Join(delimiter, values);
    }

    public static string Join(this string[] values, char delimiter)
    {
        return string.Join(delimiter, values);
    }

    public static string Join(this IEnumerable<string> values, string delimiter = "")
    {
        return string.Join(delimiter, values);
    }

    public static string Join(this string[] values, string delimiter = "")
    {
        return string.Join(delimiter, values);
    }

    public static string SanitizeKey(this string key, bool toLower = false, bool toUpper = false)
    {
        key = key?.Trim();

        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key), CONST_STR_KEY_CANNOT_BE_NULL);

        key = string.Concat(key
            .ToCharArray()
            .Where(c => char.IsLetterOrDigit(c) ||
                        c == CHAR_UNDERSCORE ||
                        c == CHAR_PERIOD)
            .ToArray());

        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key), CONST_STR_KEY_CANNOT_BE_NULL);

        if (toLower) key = key.ToLowerInvariant();
        else if (toUpper) key = key.ToUpperInvariant();

        return key;
    }

    public static byte[] GZCompress(this string value)
    {
        var buffer = value.GetBytes();

        using (var memOutput = new MemoryStream())
        {
            using (var gZipStream = new GZipStream(memOutput, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memOutput.Position = 0;

            return memOutput.GetBuffer();
        }
    }

    public static byte[] GetBytes(this string value, int limit = 0, int offset = 0)
    {
        if (string.IsNullOrWhiteSpace(value)) return [];
        if (limit < 0) throw new ArgumentOutOfRangeException(nameof(limit), nameof(limit) + " cannot be less than 0");
        if (offset < 0)
            throw new ArgumentOutOfRangeException(nameof(offset), nameof(offset) + " cannot be less than 0");
        if (offset + limit > value.Length)
            throw new ArgumentOutOfRangeException(nameof(offset),
                nameof(offset) + " is out of bounds of the " + nameof(value));

        if (limit < 1 || limit > value.Length)
            limit = value.Length;

        return value
            .ToCharArray()
            .Skip(offset)
            .Take(limit)
            .Select(c => (byte)c)
            .ToArray();
    }

    public static string GetEmbeddedResource(this string name)
    {
        var assembly = Assembly.GetCallingAssembly();
        using (var stream = assembly.GetManifestResourceStream(name))
        using (var reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }

    public static byte[] FromHex(this string value)
    {
        value = RegexConstants.REGEX_HEX_FILTER.Replace(value, string.Empty);

        var hexBytes = RegexConstants.REGEX_HEX_SPLIT
            .Split(value)
            .Where(v => !string.IsNullOrWhiteSpace(v))
            .ToArray();
        if (hexBytes.Length % 2 != 0) return [];

        return hexBytes
            .Select(s => Convert.ToByte(s, 16))
            .ToArray();
    }

    public static string ToHex(this string value)
    {
        return string.Concat(
            value
                .GetBytes()
                .Select(b => string.Format("{0:X2}", b)));
    }

    public static string ToBase64(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return string.Empty;

        return value.GetBytes()
            .ToBase64();
    }

    public static T FromBase64<T>(this string value)
        where T : class
    {
        if (string.IsNullOrWhiteSpace(value)) return null;

        var type = typeof(T);
        var bytes = c.FromBase64String(value);
        if (type == ByteExts.Types.ByteArray) return (T)(object)bytes;
        if (type == Types.String) return (T)(object)bytes.GetString();
        return null;
    }

    public static string Substr(this string value, int startIndex, int endIndex)
    {
        return value.Substring(startIndex, endIndex - startIndex);
    }

    public static string ComputeMd5(this string value)
    {
        var inputBytes = Encoding.ASCII.GetBytes(value);

        return inputBytes.ComputeMd5();
    }

    public static class Types
    {
        public static readonly Type String = typeof(string);
        public static readonly Type StringArray = typeof(string[]);
        public static readonly Type StringList = typeof(List<string>);
    }

    #region TryParse<T> Constants

    private delegate o d(s value);

    private static readonly char[] CHARARRAY_TIMESPAN_UNITS = "dhmstw".ToCharArray();
    private static readonly Dictionary<Type, Dictionary<s, o>> DICTIONARY_ENUM_TYPE_CONVERSION_CACHE = [];

    private static readonly IReadOnlyDictionary<i, d[]> IREADONLYDICTIONARY_CONVERT_DELEGATES =
        new Dictionary<i, d[]>
        {
            { (i)tc.Char, new d[] { v => char.Parse(v), v => c.ToChar(v) } },
            { (i)tc.Byte, new d[] { v => byte.Parse(v), v => c.ToByte(v) } },
            { (i)tc.SByte, new d[] { v => sbyte.Parse(v), v => c.ToSByte(v) } },
            { (i)tc.Int16, new d[] { v => short.Parse(v), v => c.ToInt16(v) } },
            { (i)tc.UInt16, new d[] { v => ushort.Parse(v), v => c.ToUInt16(v) } },
            { (i)tc.Int32, new d[] { v => i.Parse(v), v => c.ToInt32(v) } },
            { (i)tc.UInt32, new d[] { v => uint.Parse(v), v => c.ToUInt32(v) } },
            { (i)tc.Int64, new d[] { v => l.Parse(v), v => c.ToInt64(v) } },
            { (i)tc.UInt64, new d[] { v => ulong.Parse(v), v => c.ToUInt64(v) } },
            { (i)tc.Single, new d[] { v => float.Parse(v), v => c.ToSingle(v) } },
            { (i)tc.Double, new d[] { v => db.Parse(v), v => c.ToDouble(v) } },
            { (i)tc.Decimal, new d[] { v => decimal.Parse(v), v => c.ToDecimal(v) } },
            { (i)tc.Boolean, new d[] { v => bool.Parse(v), v => c.ToBoolean(v) } },
            { (i)tc.DateTime, new d[] { v => DateTime.Parse(v), v => c.ToDateTime(v) } },
            { (i)tc.Guid, new d[] { v => Guid.Parse(v) } },
            { (i)tc.TimeSpan, new d[] { v => ts.Parse(v) } },
            { (i)tc.Int128, new d[] { v => Int128.Parse(v) } },
            { (i)tc.UInt128, new d[] { v => UInt128.Parse(v) } }
        }.IReadOnlyDictionary();

    #endregion
}