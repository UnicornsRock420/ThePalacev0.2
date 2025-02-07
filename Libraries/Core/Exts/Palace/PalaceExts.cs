using System.Buffers;
using System.Drawing;
using System.Reflection;
using System.Runtime.Serialization;
using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Shared.Network;
using ThePalace.Core.Enums;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Exts.Palace;
using ThePalace.Core.Helpers;
using ThePalace.Core.Interfaces.Data;

namespace ThePalace.Core.Exts.Palace
{
    public static class PalaceExts
    {
        #region Byte Operations/Helpers
        public static short SwapInt16(this short value) =>
            value.WriteInt16().Reverse().ReadSInt16();
        public static ushort SwapUInt16(this ushort value) =>
            value.WriteUInt16().Reverse().ReadUInt16();

        public static int SwapInt32(this int value) =>
            value.WriteInt32().Reverse().ReadSInt32();
        public static uint SwapUInt32(this uint value) =>
            value.WriteUInt32().Reverse().ReadUInt32();

        public static long SwapInt64(this long value) =>
            value.WriteInt64().Reverse().ReadSInt64();
        public static ulong SwapUInt64(this ulong value) =>
            value.WriteUInt64().Reverse().ReadUInt64();

        public static short ReadSInt16(this IEnumerable<byte> value, int offset = 0)
        {
            if (offset < 1)
            {
                offset = 0;
            }

            if (offset + 1 > value.Count())
            {
                return 0;
            }

            return BitConverter.ToInt16(value.Skip(offset).Take(2).ToArray());
        }
        public static short ReadSInt16(this byte[] value, int offset = 0)
        {
            if (offset < 1)
            {
                offset = 0;
            }
            else if (offset + 1 > value.Length)
            {
                return 0;
            }

            return BitConverter.ToInt16(value.Skip(offset).Take(2).ToArray());
        }

        public static int ReadSInt32(this IEnumerable<byte> value, int offset = 0)
        {
            if (offset < 1)
            {
                offset = 0;
            }
            else if (offset + 3 > value.Count())
            {
                return 0;
            }

            return BitConverter.ToInt32(value.Skip(offset).Take(4).ToArray());
        }
        public static int ReadSInt32(this byte[] value, int offset = 0)
        {
            if (offset < 1)
            {
                offset = 0;
            }
            else if (offset + 3 > value.Length)
            {
                return 0;
            }

            return BitConverter.ToInt32(value.Skip(offset).Take(4).ToArray());
        }

        public static long ReadSInt64(this IEnumerable<byte> value, int offset = 0)
        {
            if (offset < 1)
            {
                offset = 0;
            }
            else if (offset + 3 > value.Count())
            {
                return 0;
            }

            return BitConverter.ToInt64(value.Skip(offset).Take(8).ToArray());
        }
        public static long ReadSInt64(this byte[] value, int offset = 0)
        {
            if (offset < 1)
            {
                offset = 0;
            }
            else if (offset + 3 > value.Length)
            {
                return 0;
            }

            return BitConverter.ToInt64(value.Skip(offset).Take(8).ToArray());
        }

        public static ushort ReadUInt16(this IEnumerable<byte> value, int offset = 0)
        {
            if (offset < 1)
            {
                offset = 0;
            }
            else if (offset + 1 > value.Count())
            {
                return 0;
            }

            return BitConverter.ToUInt16(value.Skip(offset).Take(2).ToArray());
        }
        public static ushort ReadUInt16(this byte[] value, int offset = 0)
        {
            if (offset < 1)
            {
                offset = 0;
            }
            else if (offset + 1 > value.Length)
            {
                return 0;
            }

            return BitConverter.ToUInt16(value.Skip(offset).Take(2).ToArray());
        }

        public static uint ReadUInt32(this IEnumerable<byte> value, int offset = 0)
        {
            if (offset < 1)
            {
                offset = 0;
            }
            else if (offset + 3 > value.Count())
            {
                return 0;
            }

            return BitConverter.ToUInt32(value.Skip(offset).Take(4).ToArray());
        }
        public static uint ReadUInt32(this byte[] value, int offset = 0)
        {
            if (offset < 1)
            {
                offset = 0;
            }
            else if (offset + 3 > value.Length)
            {
                return 0;
            }

            return BitConverter.ToUInt32(value.Skip(offset).Take(4).ToArray());
        }

        public static ulong ReadUInt64(this IEnumerable<byte> value, int offset = 0)
        {
            if (offset < 1)
            {
                offset = 0;
            }
            else if (offset + 3 > value.Count())
            {
                return 0;
            }

            return BitConverter.ToUInt64(value.Skip(offset).Take(8).ToArray());
        }
        public static ulong ReadUInt64(this byte[] value, int offset = 0)
        {
            if (offset < 1)
            {
                offset = 0;
            }
            else if (offset + 3 > value.Length)
            {
                return 0;
            }

            return BitConverter.ToUInt64(value.Skip(offset).Take(8).ToArray());
        }

        public static byte[] WriteInt16(this short value) =>
             BitConverter.GetBytes(value);
        public static byte[] WriteUInt16(this ushort value) =>
             BitConverter.GetBytes(value);

        public static byte[] WriteInt32(this int value) =>
             BitConverter.GetBytes(value);
        public static byte[] WriteUInt32(this uint value) =>
             BitConverter.GetBytes(value);

        public static byte[] WriteInt64(this long value) =>
             BitConverter.GetBytes(value);
        public static byte[] WriteUInt64(this ulong value) =>
             BitConverter.GetBytes(value);

        public static string ReadPString(this byte[] value, int max, int offset = 0, int size = 0)
        {
            if (offset < 1)
            {
                offset = 0;
            }

            if (size < 1)
            {
                size = 1;
            }

            var length = 0;
            switch (size)
            {
                case 4:
                    length = value.ReadSInt32(offset);

                    break;
                case 2:
                    length = value.ReadSInt16(offset);

                    break;
                default:
                    length = value[offset];
                    size = 1;

                    break;
            }

            if (length > max)
            {
                length = max;
            }

            return value
                .ToList()
                .Skip(size + offset)
                .Take(length)
                .ToArray()
                .GetString();
        }

        public static string ReadCString(this byte[] value, int offset = 0)
        {
            if (offset < 1)
            {
                offset = 0;
            }

            var length = value
                .Skip(offset)
                .ToList()
                .IndexOf(0);

            return value
                .ToList()
                .Skip(offset)
                .Take(length - 1)
                .ToArray()
                .GetString();
        }

        public static byte[] WritePString(this string value, int max, int size, bool padding = true)
        {
            var data = new List<byte>();

            if (size < 1)
            {
                size = 1;
            }

            var length = value.Length;
            if (length >= max - size)
            {
                length = max - size;
            }

            switch (size)
            {
                case 4:
                    data.AddRange(length.WriteInt32());

                    break;
                case 2:
                    data.AddRange(((short)length).WriteInt16());

                    break;
                default:
                    data.Add((byte)length);
                    size = 1;

                    break;
            }

            data.AddRange(value.GetBytes());

            if (padding)
            {
                var padSize = max - (size + length);
                if (padSize > 0)
                {
                    data.AddRange(new byte[padSize]);
                }
            }

            return data.ToArray();
        }

        public static byte[] WriteCString(this string value)
        {
            var data = new List<byte>();

            data.AddRange(value.GetBytes());
            data.Add(0);

            return data.ToArray();
        }
        #endregion

        #region Point-In-Polygon & Bounding Box Methods
        public static bool IsPointInPolygon(this PointF point, List<PointF> polygon) =>
            point.IsPointInPolygon(polygon.ToArray());
        public static bool IsPointInPolygon(this PointF point, params PointF[] polygon)
        {
            if (polygon.Length < 3) return false;

            var i = 0;
            var j = 0;

            var result = false;
            for (i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
                if (polygon[i].Y > point.Y != polygon[j].Y > point.Y && point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X)
                    result = !result;
            return result;
        }

        public static bool IsPointInPolygon(this Point point, List<Point> polygon) =>
            point.IsPointInPolygon(polygon.ToArray());
        public static bool IsPointInPolygon(this Point point, params Point[] polygon)
        {
            if (polygon.Length < 3) return false;

            var i = 0;
            var j = 0;

            var result = false;
            for (i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
                if (polygon[i].Y > point.Y != polygon[j].Y > point.Y && point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X)
                    result = !result;
            return result;
        }

        public static bool IsPointInPolygon(this Types.Point point, List<Types.Point> polygon) =>
            point.IsPointInPolygon(polygon.ToArray());
        public static bool IsPointInPolygon(this Types.Point point, params Types.Point[] polygon)
        {
            if (polygon.Length < 3) return false;

            var i = 0;
            var j = 0;

            var result = false;
            for (i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
                if (polygon[i].VAxis > point.VAxis != polygon[j].VAxis > point.VAxis && point.HAxis < (polygon[j].HAxis - polygon[i].HAxis) * (point.VAxis - polygon[i].VAxis) / (polygon[j].VAxis - polygon[i].VAxis) + polygon[i].HAxis)
                    result = !result;
            return result;
        }

        public static Types.Point[] GetBoundingBox(this Types.Point point, int width, int height, bool centered = false) =>
            point.GetBoundingBox(new Size(width, height), centered);
        public static Types.Point[] GetBoundingBox(this Types.Point point, Size size, bool centered = false)
        {
            var results = new List<Types.Point>();
            var w = (short)(centered ? size.Width / 2 : size.Width);
            var h = (short)(centered ? size.Height / 2 : size.Height);

            if (centered)
                results.Add(new Types.Point((short)(point.HAxis - w), (short)(point.VAxis - h)));
            else
                results.Add(point);

            if (centered)
                results.Add(new Types.Point((short)(point.HAxis + w), (short)(point.VAxis - h)));
            else
                results.Add(new Types.Point((short)(point.HAxis + w), point.VAxis));

            results.Add(new Types.Point((short)(point.HAxis + w), (short)(point.VAxis + h)));

            if (centered)
                results.Add(new Types.Point((short)(point.HAxis - w), (short)(point.VAxis + h)));
            else
                results.Add(new Types.Point(point.HAxis, (short)(point.VAxis + h)));

            return results.ToArray();
        }
        #endregion

        #region Serialization/Deserialization Methods
        public static void PalaceDeserialize(this Stream reader, ref int refNum, object? obj, Type? objType, SerializerOptions opts = SerializerOptions.None)
        {
            if (obj == null ||
                objType == null ||
                !(obj is IStruct)) return;

            if (obj.Is<IStructSerializer>(out var serializer))
            {
                serializer.Deserialize(ref refNum, reader, opts);

                return;
            }

            var doSwap = opts.IsBit<SerializerOptions, byte>(SerializerOptions.SwapByteOrder);

            var members = objType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Cast<MemberInfo>()
                .Union(objType
                    .GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .Cast<MemberInfo>())
                .ToList();

            foreach (var member in members)
            {
                var _attrs = (List<Attribute>)objType
                    .GetCustomAttributes()
                    .ToList();
                var _type = (Type?)null;
                var _name = (string?)null;
                var _cb = (Action<MemberInfo, object>)null;

                if (member is FieldInfo _fieldNfo)
                {
                    _type = _fieldNfo.FieldType;
                    _name = _fieldNfo.Name;

                    _cb = (o, v) =>
                    {
                        if (o is FieldInfo _fieldNfo)
                        {
                            _fieldNfo.SetValue(obj, v);
                        }
                    };

                    _attrs.AddRange(_fieldNfo.GetCustomAttributes());
                }
                else if (member is PropertyInfo _propNfo)
                {
                    _type = _propNfo.PropertyType;
                    _name = _propNfo.Name;

                    _cb = (o, v) =>
                    {
                        if (o is PropertyInfo _propNfo)
                        {
                            _propNfo.SetValue(obj, v);
                        }
                    };

                    _attrs.AddRange(_propNfo.GetCustomAttributes());
                }

                _attrs.AddRange(_type?.GetCustomAttributes() ?? []);

                if (_type == null ||
                    _name == null ||
                    _cb == null ||
                    _attrs.Any(a => a is IgnoreDataMemberAttribute))
                {
                    //throw new Exception(string.Format("Member (t:{0}, n:{1})", _type.Name, _name));
                    Console.WriteLine("Member (t:{0}, n:{1})", _type.Name, _name);
                    continue;
                }

                var byteSize = _attrs
                    .Where(a => a is ByteSizeAttribute)
                    .Select(a => a as ByteSizeAttribute)
                    .Select(a => a?.ByteSize ?? 0)
                    .LastOrDefault();

                //var minByteSize = 0;
                //var maxByteSize = 0;
                //var dynamicSize = _attrs
                //    .Where(a => a is DynamicSizeAttribute)
                //    .Select(a => a as DynamicSizeAttribute)
                //    .LastOrDefault();
                //if (dynamicSize != null)
                //{
                //    minByteSize = dynamicSize.MinByteSize;
                //    maxByteSize = dynamicSize.MaxByteSize;
                //}

                var buffer = (byte[]?)null;
                var result = (object?)null;

                switch (_type)
                {
                    case Type _e when _e is Enum || _e.IsEnum: break;

                    case Type _t when _t == ByteExts.Types.Byte || _t == SByteExts.Types.SByte: byteSize = 1; break;
                    case Type _t when _t == Int16Exts.Types.Int16 || _t == UInt16Exts.Types.UInt16: byteSize = 2; break;
                    case Type _t when _t == Int32Exts.Types.Int32 || _t == UInt32Exts.Types.UInt32: byteSize = 4; break;
                    case Type _t when _t == Int64Exts.Types.Int64 || _t == UInt64Exts.Types.UInt64: byteSize = 8; break;

                    case Type _t when _t == StringExts.Types.String:
                        if (_attrs.Any(a => a is PStringAttribute))
                        {
                            var pString = _attrs
                                .Where(a => a is PStringAttribute)
                                .Select(a => a as PStringAttribute)
                                .LastOrDefault();

                            switch (pString.LengthByteSize)
                            {
                                case 1: byteSize = reader.ReadByte(); break;
                                case 2: byteSize = doSwap ? (short)reader.ReadInt16().GetBytes().Reverse().ReadUInt16() : reader.ReadInt16(); break;
                                case 4: byteSize = doSwap ? (int)reader.ReadInt32().GetBytes().Reverse().ReadUInt32() : reader.ReadInt32(); break;
                            }

                            if (byteSize > pString.MaxStringLength)
                            {
                                byteSize = pString.MaxStringLength;
                            }

                            if (byteSize > 0)
                            {
                                buffer = new byte[byteSize];
                                var readCount = reader.Read(buffer, 0, buffer.Length);
                                if (readCount < 1) return;

                                if (pString is EncryptedStringAttribute encryptedString)
                                {
                                    if (encryptedString.DeserializeOptions.IsBit<EncryptedStringOptions, short>(EncryptedStringOptions.FromHex))
                                    {
                                        buffer = buffer.GetString().FromHex();
                                    }

                                    if (encryptedString.DeserializeOptions.IsBit<EncryptedStringOptions, short>(EncryptedStringOptions.DecryptString))
                                    {
                                        buffer = buffer.DecryptBytes();
                                    }
                                    else if (encryptedString.DeserializeOptions.IsBit<EncryptedStringOptions, short>(EncryptedStringOptions.EncryptString))
                                    {
                                        buffer = buffer.EncryptBytes();
                                    }

                                    if (encryptedString.DeserializeOptions.IsBit<EncryptedStringOptions, short>(EncryptedStringOptions.ToHex))
                                    {
                                        buffer = buffer.ToHex().GetBytes();
                                    }
                                }

                                _cb(member, buffer.GetString());
                            }

                            if (pString.PaddingModulo > 0 && ((pString.LengthByteSize + byteSize) % pString.PaddingModulo) != 0)
                            {
                                buffer = new byte[pString.PaddingModulo - ((pString.LengthByteSize + byteSize) % pString.PaddingModulo)];
                                var readCount = reader.Read(buffer, 0, buffer.Length);
                                if (readCount < 1) return;
                            }
                        }
                        else if (_attrs.Any(a => a is CStringAttribute))
                        {
                            var cString = _attrs
                                .Where(a => a is CStringAttribute)
                                .Select(a => a as CStringAttribute)
                                .LastOrDefault();

                            var stringBytes = new List<byte>();
                            buffer = new byte[1];

                            do
                            {
                                var readCount = reader.Read(buffer, 0, buffer.Length);
                                if (readCount < 1) return;

                                if (buffer[0] == 0) break;

                                stringBytes.Add(buffer[0]);

                            } while (stringBytes.Count <= cString.MaxStringLength);

                            if (stringBytes.Count > 0)
                            {
                                _cb(member, stringBytes.GetString());
                            }
                        }
                        else if (byteSize > 0)
                        {
                            buffer = new byte[byteSize];
                            var readCount = reader.Read(buffer, 0, buffer.Length);
                            if (readCount < 1) return;

                            _cb(member, buffer.GetString());
                        }

                        continue;

                    case Type _t when _t == ByteExts.Types.ByteArray:
                        if (byteSize > 0)
                        {
                            buffer = new byte[byteSize];
                            var readCount = reader.Read(buffer, 0, buffer.Length);
                            if (readCount < 1) return;

                            _cb(member, buffer);
                        }

                        continue;

                    case Type _t when _t is IStruct:
                        {
                            result = _t.GetInstance();

                            if (result.Is<IStructSerializer>(out serializer))
                            {
                                serializer.Deserialize(ref refNum, reader, opts);
                            }
                            else
                            {
                                reader.PalaceDeserialize(ref refNum, result, _type, opts);
                            }
                        }

                        continue;

                    default: return;
                }

                if (result == null)
                {
                    switch (byteSize)
                    {
                        case 1:
                            if (_type.IsEnum ||
                                _type == ByteExts.Types.Byte)
                                result = reader.ReadByte();
                            else if (_type == SByteExts.Types.SByte)
                                result = reader.ReadSByte();
                            break;
                        case 2:
                            if (_type.IsEnum ||
                                _type == UInt16Exts.Types.UInt16)
                                result = doSwap ? reader.ReadUInt16().SwapUInt16() : reader.ReadUInt16();
                            else if (_type == Int16Exts.Types.Int16)
                                result = doSwap ? reader.ReadInt16().SwapInt16() : reader.ReadInt16();
                            break;
                        case 4:
                            if (_type.IsEnum ||
                                _type == UInt32Exts.Types.UInt32)
                                result = doSwap ? reader.ReadUInt32().SwapUInt32() : reader.ReadUInt32();
                            else if (_type == Int32Exts.Types.Int32)
                                result = doSwap ? reader.ReadInt32().SwapInt32() : reader.ReadInt32();
                            break;
                        case 8:
                            if (_type.IsEnum ||
                                _type == UInt64Exts.Types.UInt64)
                                result = doSwap ? reader.ReadUInt64().SwapUInt64() : reader.ReadUInt64();
                            else if (_type == Int64Exts.Types.Int64)
                                result = doSwap ? reader.ReadInt64().SwapInt64() : reader.ReadInt64();
                            break;
                    }
                }

                if (result != null)
                {
                    _cb(member, result);
                }
            }
        }

        public static void PalaceSerialize(this Stream writer, ref int refNum, object? obj, Type? objType, SerializerOptions opts = SerializerOptions.None)
        {
            if (obj == null ||
                objType == null ||
                !(obj is IStruct)) return;

            var streamPosition = writer.Position;

            var doSwap = opts.IsBit<SerializerOptions, byte>(SerializerOptions.SwapByteOrder);

            var members = objType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Cast<MemberInfo>()
                .Union(objType
                    .GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .Cast<MemberInfo>())
                .ToList();

            var minByteSize = 0;
            var maxByteSize = 0;

            foreach (var member in members)
            {
                var _attrs = objType
                    .GetCustomAttributes()
                    .Cast<Attribute>()
                    .ToList();
                var _type = (Type?)null;
                var _name = (string?)null;
                var _value = (object?)null;

                if (member is FieldInfo _fieldNfo)
                {
                    _type = _fieldNfo.FieldType;
                    _name = _fieldNfo.Name;

                    try { _value = _fieldNfo.GetValue(obj); } catch { _value = null; }

                    _attrs.AddRange(_fieldNfo.GetCustomAttributes());
                }
                else if (member is PropertyInfo _propNfo)
                {
                    _type = _propNfo.PropertyType;
                    _name = _propNfo.Name;

                    try { _value = _propNfo.GetValue(obj); } catch { _value = null; }

                    _attrs.AddRange(_propNfo.GetCustomAttributes());
                }

                _attrs.AddRange(_type?.GetCustomAttributes() ?? []);

                if (_type == null ||
                    _name == null ||
                    _value == null ||
                    _attrs.Any(a => a is IgnoreDataMemberAttribute))
                {
                    //throw new Exception(string.Format("Member (t:{0}, n:{1}, v:{2})", _type.Name, _name, _value));
                    Console.WriteLine("Member (t:{0}, n:{1}, v:{2})", _type.Name, _name, _value);
                    continue;
                }

                if(_attrs
                    .Where(a => a is RefNumAttribute)
                    .Any())
                {
                    refNum = (int)(object)_value;

                    continue;
                }
                if (opts.IsBit<SerializerOptions, byte>(SerializerOptions.RefNumOnly))
                {
                    continue;
                }

                var byteSize = _attrs
                    .Where(a => a is ByteSizeAttribute)
                    .Select(a => a as ByteSizeAttribute)
                    .Select(a => a?.ByteSize ?? 0)
                    .LastOrDefault();

                if (maxByteSize == 0)
                {
                    var dynamicSize = _attrs
                        .Where(a => a is DynamicSizeAttribute)
                        .Select(a => a as DynamicSizeAttribute)
                        .LastOrDefault();
                    if (dynamicSize != null)
                    {
                        minByteSize = dynamicSize.MinByteSize;
                        maxByteSize = dynamicSize.MaxByteSize;
                    }
                }

                if (maxByteSize > 0)
                {
                    var bytesWritten = writer.Position - streamPosition;

                    if (bytesWritten > maxByteSize) throw new EndOfStreamException(nameof(writer));
                    if (bytesWritten == maxByteSize) return;
                }

                var buffer = (byte[]?)null;

                switch (_type)
                {
                    case Type _e when _e is Enum || _e.IsEnum: break;

                    case Type _t when _t == ByteExts.Types.Byte || _t == SByteExts.Types.SByte: byteSize = 1; break;
                    case Type _t when _t == Int16Exts.Types.Int16 || _t == UInt16Exts.Types.UInt16: byteSize = 2; break;
                    case Type _t when _t == Int32Exts.Types.Int32 || _t == UInt32Exts.Types.UInt32: byteSize = 4; break;
                    case Type _t when _t == Int64Exts.Types.Int64 || _t == UInt64Exts.Types.UInt64: byteSize = 8; break;

                    case Type _t when _t == StringExts.Types.String:
                        var _str = (string)_value;

                        if (_attrs.Any(a => a is PStringAttribute))
                        {
                            var pString = _attrs
                                .Where(a => a is PStringAttribute)
                                .Select(a => a as PStringAttribute)
                                .LastOrDefault();

                            byteSize = _str.Length;

                            if (byteSize > pString.MaxStringLength)
                            {
                                byteSize = pString.MaxStringLength;
                            }

                            switch (pString.LengthByteSize)
                            {
                                case 1: writer.Write([(byte)byteSize]); break;
                                case 2:
                                    if (doSwap)
                                        writer.Write(((ushort)byteSize).GetBytes().Reverse().ToArray());
                                    else
                                        writer.Write(((ushort)byteSize).GetBytes());
                                    break;
                                case 4:
                                    if (doSwap)
                                        writer.Write(((uint)byteSize).GetBytes().Reverse().ToArray());
                                    else
                                        writer.Write(((uint)byteSize).GetBytes());
                                    break;
                            }

                            if (byteSize > 0)
                            {
                                buffer = _str.GetBytes();

                                if (pString is EncryptedStringAttribute encryptedString)
                                {
                                    if (encryptedString.SerializeOptions.IsBit<EncryptedStringOptions, short>(EncryptedStringOptions.FromHex))
                                    {
                                        buffer = buffer.GetString().FromHex();
                                    }

                                    if (encryptedString.SerializeOptions.IsBit<EncryptedStringOptions, short>(EncryptedStringOptions.DecryptString))
                                    {
                                        buffer = buffer.DecryptBytes();
                                    }
                                    else if (encryptedString.SerializeOptions.IsBit<EncryptedStringOptions, short>(EncryptedStringOptions.EncryptString))
                                    {
                                        buffer = buffer.EncryptBytes();
                                    }

                                    if (encryptedString.SerializeOptions.IsBit<EncryptedStringOptions, short>(EncryptedStringOptions.ToHex))
                                    {
                                        buffer = buffer.ToHex().GetBytes();
                                    }
                                }

                                writer.Write(buffer, 0, buffer.Length);
                            }

                            if (pString.PaddingModulo > 0 && ((pString.LengthByteSize + byteSize) % pString.PaddingModulo) != 0)
                            {
                                buffer = new byte[pString.PaddingModulo - ((pString.LengthByteSize + byteSize) % pString.PaddingModulo)];
                                writer.Write(buffer, 0, buffer.Length);
                            }
                        }
                        else if (_attrs.Any(a => a is CStringAttribute))
                        {
                            var cString = _attrs
                                .Where(a => a is CStringAttribute)
                                .Select(a => a as CStringAttribute)
                                .LastOrDefault();

                            buffer = _str.GetBytes();
                            writer.Write(buffer, 0, buffer.Length);
                            writer.Write([(byte)0]);
                        }
                        else if (byteSize > 0)
                        {
                            buffer = _str.GetBytes(byteSize);

                            writer.Write(buffer, 0, buffer.Length);
                        }

                        continue;

                    case Type _t when _t == ByteExts.Types.ByteArray:
                        if (byteSize < 1)
                        {
                            buffer = (byte[])(object)_value;
                            byteSize = buffer.Length;
                        }

                        break;

                    case Type _t when _t is IStruct:
                        writer.PalaceSerialize(ref refNum, _value, _type);

                        continue;

                    default: return;
                }

                switch (byteSize)
                {
                    case 1: buffer = [(byte)_value]; break;
                    case 2: buffer = doSwap ? Convert.ToUInt16(_value).GetBytes().Reverse().ToArray() : Convert.ToUInt16(_value).GetBytes(); break;
                    case 4: buffer = doSwap ? Convert.ToUInt32(_value).GetBytes().Reverse().ToArray() : Convert.ToUInt32(_value).GetBytes(); break;
                    case 8: buffer = doSwap ? Convert.ToUInt64(_value).GetBytes().Reverse().ToArray() : Convert.ToUInt64(_value).GetBytes(); break;
                }

                if ((buffer?.Length ?? 0) > 0)
                    writer.Write(buffer, 0, buffer.Length);
            }

            if (minByteSize > 0 && (writer.Position - streamPosition) < minByteSize) throw new EndOfStreamException(nameof(writer));
        }

        public static void PalaceSerialize<TStruct>(this Stream writer, ref int refNum, TStruct? obj, SerializerOptions opts = SerializerOptions.None)
            where TStruct : IStruct
        {
            if (obj == null) return;

            var objType = typeof(TStruct);
            var objTypeName = objType.Name;
            if (string.IsNullOrWhiteSpace(objTypeName)) return;

            var msgBytes = (byte[]?)null;
            using (var ms = new MemoryStream())
            {
                if (obj.Is<IStructSerializer>(out var serializer))
                {
                    ms.PalaceSerialize(ref refNum, obj, objType, SerializerOptions.RefNumOnly);

                    serializer.Serialize(ref refNum, ms, opts);
                }
                else
                {
                    ms.PalaceSerialize(ref refNum, obj, opts);
                }

                msgBytes = ms.ToArray();
            }
            if ((msgBytes?.Length ?? 0) < 1) msgBytes = null;

            if (opts.IsBit<SerializerOptions, byte>(SerializerOptions.IncludeHeader))
            {
                var hdr = new MSG_Header
                {
                    EventType = Enum.Parse<EventTypes>(objTypeName),
                    Length = (uint)(msgBytes?.Length ?? 0),
                };
                if (obj.Is<IStructRefNum>())
                {
                    hdr.RefNum = refNum;
                }

                var hdrBytes = (byte[]?)null;
                using (var ms = new MemoryStream())
                {
                    if (hdr.Is<IStructSerializer>(out var serializer))
                    {
                        serializer.Serialize(ref refNum, ms, opts);
                    }
                    else
                    {
                        ms.PalaceSerialize(ref refNum, hdr, typeof(MSG_Header), opts);
                    }

                    hdrBytes = ms.ToArray();
                }
                if ((hdrBytes?.Length ?? 0) < 1) return;

                writer.Write(hdrBytes);
            }

            if ((msgBytes?.Length ?? 0) > 0)
                writer.Write(msgBytes);
        }
        #endregion
    }
}