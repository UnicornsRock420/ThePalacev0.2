using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;
using uint8 = System.Byte;

namespace ThePalace.Core.Types
{
    [ByteSize(MaxLength + LengthSize)]
    [ComVisible(true)]
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Str127 : IString, IComparable, IFormattable, IConvertible
    {
        [IgnoreDataMember]
        public const uint8 LengthSize = 1;
        [IgnoreDataMember]
        public const uint8 MaxLength = 0x80 - LengthSize;

        [ByteSize(127)]
        [IgnoreDataMember]
        private uint8[] _value;

        public static explicit operator uint8[](Str127 p) => p.Value ?? [];
        public static explicit operator char[](Str127 p) => p.ToString()?.ToCharArray() ?? [];
        public static explicit operator string(Str127 p) => p.ToString() ?? string.Empty;
        public static explicit operator Str127(uint8[] v) => new(v);
        public static explicit operator Str127(char[] v) => new(v);
        public static explicit operator Str127(string v) => new(v);

        public Str127(uint8[] value)
        {
            var length = value?.Length ?? 0;
            var max = MaxLength - 2;

            if (length >= max - LengthSize)
            {
                length = max - LengthSize;
            }

            var _value = new List<uint8>();
            _value.Add((byte)length);
            if (length > 0)
            {
                _value.AddRange(value.GetRange(length));
            }
            this._value = _value.ToArray();
        }
        public Str127(char[] value)
        {
            var length = value?.Length ?? 0;
            var max = MaxLength - 2;

            if (length >= max - LengthSize)
            {
                length = max - LengthSize;
            }

            var _value = new List<uint8>();
            _value.Add((byte)length);
            if (length > 0)
            {
                _value.AddRange(value.GetBytes(length));
            }
            this._value = _value.ToArray();
        }
        public Str127(string value)
        {
            var length = value?.Length ?? 0;
            var max = MaxLength - 2;

            if (length >= max - LengthSize)
            {
                length = max - LengthSize;
            }

            var _value = new List<uint8>();
            _value.Add((byte)length);
            if (length > 0)
            {
                _value.AddRange(value.GetBytes(length));
            }
            this._value = _value.ToArray();
        }

        public uint8 Length => (uint8)(_value?.Length ?? 0);

        public uint8[] Value
        {
            get
            {
                return _value?.ToArray() ?? [];
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));

                if (value.Length > MaxLength) throw new ArgumentOutOfRangeException(nameof(value), string.Format("[].Length too long - Max is {0}.", MaxLength));

                _value = value;
            }
        }

        public string? ToString(string? format, IFormatProvider? formatProvider) => string.Concat(_value.GetChars(MaxLength, LengthSize));
        public string? ToString(IFormatProvider? provider) => string.Concat(_value.GetChars(MaxLength, LengthSize));
        public string? ToString() => string.Concat(_value.GetChars(MaxLength, LengthSize));

        public readonly TypeCode GetTypeCode() => TypeCode.Byte;

        public readonly object ToType(Type conversionType, IFormatProvider? provider) => ByteExts.Types.ByteArray;

        public readonly int CompareTo(object? obj) => 0;

        public readonly bool ToBoolean(IFormatProvider? provider) => true;

        public DateTime ToDateTime(IFormatProvider? provider)
        {
            try
            {
                return DateTime.Parse(string.Concat(_value.GetChars(MaxLength, LengthSize)));
            }
            catch
            {
                return DateTime.UnixEpoch;
            }
        }

        public byte ToByte(IFormatProvider? provider) => (byte)(_value?.FirstOrDefault() ?? 0);

        public char ToChar(IFormatProvider? provider) => (char)(_value?.FirstOrDefault() ?? 0);

        public decimal ToDecimal(IFormatProvider? provider) => (decimal)(_value?.FirstOrDefault() ?? 0);

        public double ToDouble(IFormatProvider? provider) => (double)(_value?.FirstOrDefault() ?? 0);

        public short ToInt16(IFormatProvider? provider) => (short)(_value?.FirstOrDefault() ?? 0);

        public int ToInt32(IFormatProvider? provider) => (int)(_value?.FirstOrDefault() ?? 0);

        public long ToInt64(IFormatProvider? provider) => (long)(_value?.FirstOrDefault() ?? 0);

        public sbyte ToSByte(IFormatProvider? provider) => (sbyte)(_value?.FirstOrDefault() ?? 0);

        public float ToSingle(IFormatProvider? provider) => (char)(_value?.FirstOrDefault() ?? 0);

        public ushort ToUInt16(IFormatProvider? provider) => (ushort)(_value?.FirstOrDefault() ?? 0);

        public uint ToUInt32(IFormatProvider? provider) => (uint)(_value?.FirstOrDefault() ?? 0);

        public ulong ToUInt64(IFormatProvider? provider) => (ulong)(_value?.FirstOrDefault() ?? 0);
    }
}