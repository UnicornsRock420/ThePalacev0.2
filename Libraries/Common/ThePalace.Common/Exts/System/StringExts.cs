using System.ComponentModel;
using System.Globalization;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using ThePalace.Common.Constants;
using ThePalace.Common.Enums.System;

namespace System;

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
    private const s CONST_STR_KEY_CANNOT_BE_NULL = "Key cannot be null";
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
            case Type _t when _t == Types.String: return (T)(o)value;
            case Type _t when _t == EnumExts.Types.Enum:
                if (Enum.TryParse(type, value, true, out var enumValue)) return (T)(o)enumValue;
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
                        out var datetimeValue)) return (T)(o)datetimeValue;
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
                if (units0 == 't' && l.TryParse(value, out var longValue)) return (T)(o)ts.FromTicks(longValue);
                if (db.TryParse(value, out var doubleValue))
                    switch (units0)
                    {
                        case 'd': return (T)(o)ts.FromDays(doubleValue);
                        case 'h': return (T)(o)ts.FromHours(doubleValue);
                        case 'm':
                            if (uLength > 1 && units[1] == 's') return (T)(o)ts.FromMilliseconds(doubleValue);
                            return (T)(o)ts.FromMinutes(doubleValue);
                        case 's': return (T)(o)ts.FromSeconds(doubleValue);
                        case 'w': return (T)(o)ts.FromDays(doubleValue * 7);
                    }

                goto default;
            default:
                var typeID = type.TypeID();
                if (IREADONLYDICTIONARY_CONVERT_DELEGATES.TryGetValue(typeID, out var funcs))
                    foreach (var func in funcs)
                        try
                        {
                            return (T)(o)func(value);
                        }
                        catch
                        {
                        }

                try
                {
                    return (T)(o)c.ChangeType(value, type, CultureInfo.InvariantCulture);
                }
                catch
                {
                }

                return defaultValue;
        }
    }

    #endregion

    public static s Format(this s format, params o[] args)
    {
        return s.Format(format, args);
    }

    public static s Join(this IEnumerable<s> values, char delimiter)
    {
        return s.Join(delimiter, values);
    }

    public static s Join(this s[] values, char delimiter)
    {
        return s.Join(delimiter, values);
    }

    public static s Join(this IEnumerable<s> values, s delimiter = "")
    {
        return s.Join(delimiter, values);
    }

    public static s Join(this s[] values, s delimiter = "")
    {
        return s.Join(delimiter, values);
    }

    public static s SanitizeKey(this s key, bool toLower = false, bool toUpper = false)
    {
        key = key?.Trim();

        if (s.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key), CONST_STR_KEY_CANNOT_BE_NULL);

        key = s.Concat(key
            .ToCharArray()
            .Where(c => char.IsLetterOrDigit(c) ||
                        c == CHAR_UNDERSCORE ||
                        c == CHAR_PERIOD)
            .ToArray());

        if (s.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key), CONST_STR_KEY_CANNOT_BE_NULL);

        if (toLower) key = key.ToLowerInvariant();
        else if (toUpper) key = key.ToUpperInvariant();

        return key;
    }

    public static byte[] GZCompress(this s value)
    {
        var buffer = value.GetBytes();

        using (var memOutput = new MemoryStream())
        {
            using (var gZipStream = new GZipStream(memOutput, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memOutput.Position = 0;

            return memOutput.ToArray();
        }
    }

    public static byte[] GetBytes(this s value, i limit = 0, i offset = 0)
    {
        if (s.IsNullOrWhiteSpace(value)) return [];
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

    public static s GetEmbeddedResource(this s name)
    {
        var assembly = Assembly.GetCallingAssembly();
        using (var stream = assembly.GetManifestResourceStream(name))
        using (var reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }

    public static byte[] FromHex(this s value)
    {
        value = RegexConstants.REGEX_HEX_FILTER.Replace(value, s.Empty);

        var hexBytes = RegexConstants.REGEX_HEX_SPLIT
            .Split(value)
            .Where(v => !s.IsNullOrWhiteSpace(v))
            .ToArray();
        if (hexBytes.Length % 2 != 0) return [];

        return hexBytes
            .Select(s => Convert.ToByte(s, 16))
            .ToArray();
    }

    public static s ToHex(this s value)
    {
        return s.Concat(
            value
                .GetBytes()
                .Select(b => $"{b:X2}"));
    }

    public static s ToBase64(this s value)
    {
        if (s.IsNullOrWhiteSpace(value)) return s.Empty;

        return value.GetBytes()
            .ToBase64();
    }

    public static T FromBase64<T>(this s value)
        where T : class
    {
        if (s.IsNullOrWhiteSpace(value)) return null;

        var type = typeof(T);
        var bytes = c.FromBase64String(value);
        if (type == ByteExts.Types.ByteArray) return (T)(o)bytes;
        if (type == Types.String) return (T)(o)bytes.GetString();
        return null;
    }

    public static s Substr(this s value, i startIndex, i endIndex)
    {
        return value.Substring(startIndex, endIndex - startIndex);
    }

    public static s ComputeMd5(this s value)
    {
        var inputBytes = Encoding.ASCII.GetBytes(value);

        return inputBytes.ComputeMd5();
    }

    public static class Types
    {
        public static readonly Type String = typeof(s);
        public static readonly Type StringArray = typeof(s[]);
        public static readonly Type StringList = typeof(List<s>);
    }

    #region TryParse<T> Constants

    private delegate o d(s value);

    private static readonly char[] CHARARRAY_TIMESPAN_UNITS = "dhmstw".ToCharArray();
    private static readonly Dictionary<Type, Dictionary<s, o>> DICTIONARY_ENUM_TYPE_CONVERSION_CACHE = [];

    private static readonly IReadOnlyDictionary<i, d[]> IREADONLYDICTIONARY_CONVERT_DELEGATES =
        new Dictionary<i, d[]>
        {
            { (i)tc.Char, [v => char.Parse(v), v => c.ToChar(v)] },
            { (i)tc.Byte, [v => byte.Parse(v), v => c.ToByte(v)] },
            { (i)tc.SByte, [v => sbyte.Parse(v), v => c.ToSByte(v)] },
            { (i)tc.Int16, [v => short.Parse(v), v => c.ToInt16(v)] },
            { (i)tc.UInt16, [v => ushort.Parse(v), v => c.ToUInt16(v)] },
            { (i)tc.Int32, [v => i.Parse(v), v => c.ToInt32(v)] },
            { (i)tc.UInt32, [v => uint.Parse(v), v => c.ToUInt32(v)] },
            { (i)tc.Int64, [v => l.Parse(v), v => c.ToInt64(v)] },
            { (i)tc.UInt64, [v => ulong.Parse(v), v => c.ToUInt64(v)] },
            { (i)tc.Single, [v => float.Parse(v), v => c.ToSingle(v)] },
            { (i)tc.Double, [v => db.Parse(v), v => c.ToDouble(v)] },
            { (i)tc.Decimal, [v => decimal.Parse(v), v => c.ToDecimal(v)] },
            { (i)tc.Boolean, [v => bool.Parse(v), v => c.ToBoolean(v)] },
            { (i)tc.DateTime, [v => DateTime.Parse(v), v => c.ToDateTime(v)] },
            { (i)tc.Guid, [v => Guid.Parse(v)] },
            { (i)tc.TimeSpan, [v => ts.Parse(v)] },
            { (i)tc.Int128, [v => Int128.Parse(v)] },
            { (i)tc.UInt128, [v => UInt128.Parse(v)] }
        }.IReadOnlyDictionary();

    #endregion
}