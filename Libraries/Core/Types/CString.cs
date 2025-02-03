using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;
using uint16 = System.UInt16;
using uint8 = System.Byte;

namespace ThePalace.Core.Types
{
    [DynamicSize]
    [ComVisible(true)]
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct CString : IString, IComparable, IFormattable, IConvertible
    {
        [IgnoreDataMember]
        public const uint16 MaxLength = 0xFFFF - 1;

        [IgnoreDataMember]
        private List<uint8>? _value = null;

        public static explicit operator uint8[](CString p) => p.Value ?? [];
        public static explicit operator char[](CString p) => p.ToString()?.ToCharArray() ?? [];
        public static explicit operator string(CString p) => p.ToString() ?? string.Empty;
        public static explicit operator CString(uint8[] v) => new(v);
        public static explicit operator CString(char[] v) => new(v);
        public static explicit operator CString(string v) => new(v);

        public CString(uint8[] value)
        {
            var length = value?.Length ?? 0;
            if (length >= MaxLength)
            {
                length = MaxLength;
            }

            this._value = [];
            if (length > 0)
            {
                this._value.AddRange(value.GetRange(length));
            }
            this._value.Add(0);
        }
        public CString(char[] value)
        {
            var length = value?.Length ?? 0;
            if (length >= MaxLength)
            {
                length = MaxLength;
            }

            this._value = [];
            if (length > 0)
            {
                this._value.AddRange(value.GetBytes(length));
            }
            this._value.Add(0);
        }
        public CString(string? value = null)
        {
            var length = value?.Length ?? 0;
            if (length >= MaxLength)
            {
                length = MaxLength;
            }

            this._value = [];
            if (length > 0)
            {
                this._value.AddRange(value.GetBytes(length));
            }
            this._value.Add(0);
        }

        public readonly uint8 Length => (uint8)(_value?.Count ?? 0);

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

        public string? ToString(string? format, IFormatProvider? formatProvider) => string.Concat(_value.GetChars(Length - 1));
        public string? ToString(IFormatProvider? provider) => string.Concat(_value.GetChars(Length - 1));
        public string? ToString() => string.Concat(_value.GetChars(Length - 1));

        public readonly TypeCode GetTypeCode() => TypeCode.Byte;

        public readonly object ToType(Type conversionType, IFormatProvider? provider) => ByteExts.Types.ByteArray;

        public readonly int CompareTo(object? obj) => 0;

        public readonly bool ToBoolean(IFormatProvider? provider) => true;

        public DateTime ToDateTime(IFormatProvider? provider)
        {
            try
            {
                return DateTime.Parse(string.Concat(_value.GetChars(Length - 1)));
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