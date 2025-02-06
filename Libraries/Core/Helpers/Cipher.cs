using System.Text;
using System.Text.RegularExpressions;
using ThePalace.Core.Constants;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Core.Exts.Palace;

namespace ThePalace.Core.Helpers
{
    public static class Cipher
    {
        private static readonly byte[] _gEncryptTable = new byte[512];

        private enum Rs : long
        {
            R_A = 16807,
            R_M = 2147483647,
            R_Q = 127773,
            R_R = 2836,
        }

        static Cipher()
        {
            var gSeed = 666666L;

            Func<double> LongRandom = () =>
            {
                var hi = gSeed / (long)Rs.R_Q;
                var lo = gSeed % (long)Rs.R_Q;
                var test = (long)Rs.R_A * lo - (long)Rs.R_R * hi;

                if (test > 0)
                {
                    gSeed = test;
                }
                else
                {
                    gSeed = test + (long)Rs.R_M;
                }

                return gSeed / (double)Rs.R_M;
            };

            Func<short, byte> MyRandom = (max) =>
            {
                return (byte)(LongRandom() * max);
            };

            for (var j = 0; j < _gEncryptTable.Length; j++)
            {
                _gEncryptTable[j] = MyRandom(256);
            }
        }

        public static byte[] EncryptString(this string value)
        {
            var inStr = value.GetBytes();
            return EncryptBytes(inStr);
        }
        public static byte[] EncryptBytes(this byte[] inStr)
        {
            var outBytes = new byte[inStr.Length];

            int rc = 0;
            byte lastChar = 0;

            for (var i = inStr.Length - 1; i >= 0; --i)
            {
                outBytes[i] = (byte)(inStr[i] ^ _gEncryptTable[rc++] ^ lastChar);
                lastChar = (byte)(outBytes[i] ^ _gEncryptTable[rc++]);
            }

            return outBytes;
        }

        public static string DecryptString(this string value)
        {
            var inStr = value.GetBytes();
            var outBytes = new byte[inStr.Length];

            var lastChar = (byte)0;
            var rc = 0;

            for (var i = inStr.Length - 1; i >= 0; --i)
            {
                outBytes[i] = (byte)(inStr[i] ^ _gEncryptTable[rc++] ^ lastChar);
                lastChar = (byte)(inStr[i] ^ _gEncryptTable[rc++]);
            }

            return outBytes.GetString();
        }
        public static string DecryptString(this byte[] inStr)
        {
            var outBytes = new byte[inStr.Length];

            var lastChar = (byte)0;
            var rc = 0;

            for (var i = inStr.Length - 1; i >= 0; --i)
            {
                outBytes[i] = (byte)(inStr[i] ^ _gEncryptTable[rc++] ^ lastChar);
                lastChar = (byte)(inStr[i] ^ _gEncryptTable[rc++]);
            }

            return outBytes.GetString();
        }
        public static byte[] DecryptBytes(this byte[] inStr)
        {
            var outBytes = new byte[inStr.Length];

            var lastChar = (byte)0;
            var rc = 0;

            for (var i = inStr.Length - 1; i >= 0; --i)
            {
                outBytes[i] = (byte)(inStr[i] ^ _gEncryptTable[rc++] ^ lastChar);
                lastChar = (byte)(inStr[i] ^ _gEncryptTable[rc++]);
            }

            return outBytes;
        }

        public static int GetSeedFromReg(uint counter, uint crc) =>
            (int)(counter ^ (uint)RegConstants.MAGIC_LONG ^ crc);

        public static int GetSeedFromPUID(uint counter, uint crc) =>
            (int)(counter ^ crc);

        public static uint ComputeLicenseCrc(uint seed) =>
            ComputeCrc(
                seed.SwapUInt32()
                    .WriteUInt32());

        private static uint GetCrc(uint crc, uint ptr) =>
            (crc << 1 | (uint)((crc & 0x80000000) != 0 ? 1 : 0)) ^ ptr;
        public static uint ComputeCrc(byte[] ptr, uint offset = 0, bool isAsset = false)
        {
            if (ptr == null ||
                ptr.Length == 0) return 0;

            var len = ptr.Length - offset;
            var crc = (uint)0;
            var j = offset;

            if (isAsset)
            {
                crc = (uint)AssetConstants.Values.CRC_MAGIC;
            }
            else
            {
                crc = (uint)RegConstants.CRC_MAGIC;
            }

            while (len-- > 0)
            {
                if (isAsset)
                {
                    crc = GetCrc(crc, ptr[j++]);
                }
                else
                {
                    crc = GetCrc(crc, CrcMagic.gCrcMask[ptr[j++]]);
                }
            }

            return crc;
        }

        public static bool ValidUserSerialNumber(uint crc, uint counter)
        {
            var seed = counter ^ (uint)RegConstants.MAGIC_LONG ^ crc;
            return crc == ComputeLicenseCrc(seed);
        }

        public static byte[] ReadPalaceString(this string source)
        {
            var srcBytes = source.GetBytes();
            var destBytes = new List<byte>();

            for (var j = 0; j < srcBytes.Length; j++)
            {
                if (srcBytes[j] == (byte)'\\')
                {
                    var byte1 = (char)srcBytes[++j];
                    var byte2 = (char)srcBytes[++j];
                    var hex = $"0x{byte1}{byte2}";

                    destBytes.Add(Convert.ToByte(hex, 16));
                }
                else
                {
                    destBytes.Add(srcBytes[j]);
                }
            }

            return destBytes.ToArray();
        }

        public static string WritePalaceString(this byte[] source)
        {
            var dest = new StringBuilder();

            for (var j = 0; j < source.Length; j++)
            {
                if (Regex.IsMatch($"{(char)source[j]}", @"[a-z0-9]", RegexOptions.IgnoreCase | RegexOptions.Singleline))
                {
                    dest.Append(source[j]);
                }
                else
                {
                    dest.AppendFormat(@"\{0:X2}", source[j]);
                }
            }

            return dest.ToString();
        }

        public static string RegRectoSeed(RegistrationRec regInfo, bool puid = false)
        {
            var seedCounter = (uint)0;

            if (puid)
            {
                seedCounter = regInfo.PuidCtr ^ regInfo.PuidCRC;
            }
            else
            {
                seedCounter = regInfo.Counter ^ (uint)RegConstants.MAGIC_LONG ^ regInfo.Crc;
            }

            return SeedToWizKey(seedCounter, puid);
        }

        public static string SeedToWizKey(uint seedCounter, bool puid = false)
        {
            var sb = new StringBuilder();

            sb.Append('{');

            if (puid)
            {
                sb.Append('Z');
            }

            while (seedCounter > 0)
            {
                sb.Append((char)((byte)'A' + (seedCounter % 13 ^ 4)));
                seedCounter /= 13;
            }

            sb.Append('}');

            return sb.ToString();
        }

        public static int WizKeytoSeed(string wizKey)
        {
            var str = wizKey.GetBytes();
            int ctr = 0, mag = 1;

            for (var j = 0; j < str.Length; j++)
            {
                if (str[j] == (byte)'{' || str[j] == (byte)'Z') continue;
                else if (str[j] == (byte)'}') break;
                else if (str[j] < (byte)'A' || str[j] > (byte)'Q')
                    return -1;

                ctr += (str[j] - (byte)'A' ^ 4) * mag;
                mag *= 13;
            }

            return ctr;
        }
    }
}