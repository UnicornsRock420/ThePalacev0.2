using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;
using uint16 = System.UInt16;
using uint8 = System.Byte;

namespace ThePalace.Core.Types
{
    [DynamicSize(MaxLength + LengthSize)]
    [ComVisible(true)]
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct PString2 : IString, IComparable, IFormattable, IConvertible
    {
        [IgnoreDataMember]
        public const uint8 LengthSize = 2;
        [IgnoreDataMember]
        public const uint16 MaxLength = 0xFFFF - LengthSize;

        [IgnoreDataMember]
        private List<uint8>? _value = null;

        public static explicit operator uint8[](PString2 p) => p.Value ?? [];
        public static explicit operator char[](PString2 p) => p.ToString()?.ToCharArray() ?? [];
        public static explicit operator string(PString2 p) => p.ToString() ?? string.Empty;
        public static explicit operator PString2(uint8[] v) => new(v);
        public static explicit operator PString2(char[] v) => new(v);
        public static explicit operator PString2(string v) => new(v);

        public PString2(uint8[] value)
        {
            var length = value?.Length ?? 0;
            if (length >= MaxLength)
            {
                length = MaxLength;
            }

            this._value = [];
            this._value.AddRange(BitConverter.GetBytes(length));
            if (length > 0)
            {
                this._value.AddRange(value.GetRange(length));
            }
        }
        public PString2(char[] value)
        {
            var length = value?.Length ?? 0;
            if (length >= MaxLength)
            {
                length = MaxLength;
            }

            this._value = [];
            this._value.AddRange(BitConverter.GetBytes(length));
            if (length > 0)
            {
                this._value.AddRange(value.GetBytes(length));
            }
        }
        public PString2(string value)
        {
            var length = value?.Length ?? 0;
            if (length >= MaxLength)
            {
                length = MaxLength;
            }

            this._value = [];
            this._value.AddRange(BitConverter.GetBytes(length));
            if (length > 0)
            {
                this._value.AddRange(value.GetBytes(length));
            }
        }

        public uint16 Length => (uint16)(_value?.Count ?? 0);

        public uint8[] Value
        {
            get => _value?.ToArray() ?? [];
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));

                if (value.Length > MaxLength) throw new ArgumentOutOfRangeException(nameof(value), string.Format("[].Length too long - Max is {0}.", MaxLength));

                _value = new List<uint8>(value);
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