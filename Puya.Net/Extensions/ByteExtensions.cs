using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Extensions
{
    public static class ByteExtensions
    {
        public static char SixBitAsChar(this byte b)
        {
            var lookupTable = new char[64]
            {
                'A','B','C','D','E','F','G','H','I','J','K','L','M',
                'N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
                'a','b','c','d','e','f','g','h','i','j','k','l','m',
                'n','o','p','q','r','s','t','u','v','w','x','y','z',
                '0','1','2','3','4','5','6','7','8','9','+','/'
            };

            if ((b >= 0) && (b <= 63))
            {
                return lookupTable[(int)b];
            }
            else
            {
                //should not happen;
                return ' ';
            }
        }
        public static string ToHex(this byte[] bytes, char? separator = null)
        {
            if (separator == null || separator.Value == default(char))
            {
                var builder = new StringBuilder();

                if (bytes != null && bytes.Length > 0)
                {
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                }

                return builder.ToString();
            }
            else
            {
                return BitConverter.ToString(bytes).Replace('-', separator.Value);
            }
        }
    }
}
