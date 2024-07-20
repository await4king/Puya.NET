using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Extensions
{
    public static class NumberExtensions
    {
        public static String ToBinary(this Byte[] data, string separator = "")
        {
            return string.Join(separator.ToString(), data.Select(byt => Convert.ToString(byt, 2).PadLeft(8, '0')));
        }
        public static string ToBinaryString(this long x, byte zeroPadSize)
        {
            if (zeroPadSize < 0)
            {
                zeroPadSize = 0;
            }
            if (zeroPadSize > 64)
            {
                zeroPadSize = 64;
            }

            char[] bits = new char[64];
            int i = 0;

            while (x != 0)
            {
                bits[i++] = (x & 1) == 1 ? '1' : '0';
                x >>= 1;
            }

            for (var j = i; j < zeroPadSize; j++)
            {
                bits[j] = '0';
            }

            var size = zeroPadSize > 0 ? zeroPadSize : i;
            var result = new char[size];

            Array.Copy(bits, 0, result, 0, size);
            Array.Reverse(result, 0, size);

            return new string(result);
        }
        public static string ToBinaryString(this int x)
        {
            //char[] b = new char[sizeof(Int32) * 8];

            //for (int i = 0; i < b.Length; i++)
            //    b[b.Length - 1 - i] = ((n & (1 << i)) != 0) ? '1' : '0';

            //return new string(b).TrimStart('0');

            char[] bits = new char[32];
            int i = 0;

            while (x != 0)
            {
                bits[i++] = (x & 1) == 1 ? '1' : '0';
                x >>= 1;
            }

            Array.Reverse(bits, 0, i);

            return new string(bits);
        }
        public static string ToBinaryString(this short x)
        {
            //char[] b = new char[sizeof(Int32) * 8];

            //for (int i = 0; i < b.Length; i++)
            //    b[b.Length - 1 - i] = ((n & (1 << i)) != 0) ? '1' : '0';

            //return new string(b).TrimStart('0');

            char[] bits = new char[32];
            int i = 0;

            while (x != 0)
            {
                bits[i++] = (x & 1) == 1 ? '1' : '0';
                x >>= 1;
            }

            Array.Reverse(bits, 0, i);

            return new string(bits);
        }
        public static long FromBinaryString(this string binary, bool throwOnInvalidCharacters = true)
        {
            double result = 0;

            if (!string.IsNullOrEmpty(binary))
            {
                for (var i = 0; i < binary.Length; i++)
                {
                    result += Math.Pow(2, i) * (binary[binary.Length - i - 1] == '1' ? 1 : 0);
                }
            }

            return (long)result;
        }
    }
}
