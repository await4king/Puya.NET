using Puya.Extensions;

namespace Puya.Cryptography.v2
{
    public class Base64Encryption : IBase64Encryption
    {
        public string Encode(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return string.Empty;
            }

            var source = data;
            int length, length2;
            int blockCount;
            int paddingCount;

            length = source.Length;

            if ((length % 3) == 0)
            {
                paddingCount = 0;
                blockCount = length / 3;
            }
            else
            {
                paddingCount = 3 - (length % 3);//need to add padding
                blockCount = (length + paddingCount) / 3;
            }

            length2 = length + paddingCount;//or blockCount *3

            byte[] source2;

            source2 = new byte[length2];

            //copy data over insert padding

            for (int x = 0; x < length2; x++)
            {
                if (x < length)
                {
                    source2[x] = source[x];
                }
                else
                {
                    source2[x] = 0;
                }
            }

            byte b1, b2, b3;
            byte temp, temp1, temp2, temp3, temp4;
            byte[] buffer = new byte[blockCount * 4];
            char[] result = new char[blockCount * 4];

            for (int x = 0; x < blockCount; x++)
            {
                b1 = source2[x * 3];
                b2 = source2[x * 3 + 1];
                b3 = source2[x * 3 + 2];

                temp1 = (byte)((b1 & 252) >> 2);//first

                temp = (byte)((b1 & 3) << 4);
                temp2 = (byte)((b2 & 240) >> 4);
                temp2 += temp; //second

                temp = (byte)((b2 & 15) << 2);
                temp3 = (byte)((b3 & 192) >> 6);
                temp3 += temp; //third

                temp4 = (byte)(b3 & 63); //fourth

                buffer[x * 4] = temp1;
                buffer[x * 4 + 1] = temp2;
                buffer[x * 4 + 2] = temp3;
                buffer[x * 4 + 3] = temp4;

            }

            for (int x = 0; x < blockCount * 4; x++)
            {
                result[x] = buffer[x].SixBitAsChar();
            }

            //covert last "A"s to "=", based on paddingCount

            switch (paddingCount)
            {
                case 0: break;
                case 1: result[blockCount * 4 - 1] = '='; break;
                case 2:
                    result[blockCount * 4 - 1] = '=';
                    result[blockCount * 4 - 2] = '=';
                    break;
                default: break;
            }

            return new string(result);
        }
        public byte[] Decode(string data)
        {

            if (data == null || data.Length == 0)
            {
                return new byte[] { };
            }

            int length, length2, length3;
            int blockCount;
            int paddingCount;
            int temp = 0;
            var source = data.ToCharArray();

            length = source.Length;

            //find how many padding are there
            for (int x = 0; x < 2; x++)
            {
                if (source[length - x - 1] == '=')
                    temp++;
            }

            paddingCount = temp;

            //calculate the blockCount;
            //assuming all whitespace and carriage returns/newline were removed.

            blockCount = length / 4;
            length2 = blockCount * 3;

            byte[] buffer = new byte[length];   //first conversion result
            byte[] buffer2 = new byte[length2]; //decoded array with padding

            for (int x = 0; x < length; x++)
            {
                buffer[x] = source[x].As6Bit();
            }

            byte b, b1, b2, b3;
            byte temp1, temp2, temp3, temp4;

            for (int x = 0; x < blockCount; x++)
            {
                temp1 = buffer[x * 4];
                temp2 = buffer[x * 4 + 1];
                temp3 = buffer[x * 4 + 2];
                temp4 = buffer[x * 4 + 3];

                b = (byte)(temp1 << 2);
                b1 = (byte)((temp2 & 48) >> 4);
                b1 += b;

                b = (byte)((temp2 & 15) << 4);
                b2 = (byte)((temp3 & 60) >> 2);
                b2 += b;

                b = (byte)((temp3 & 3) << 6);
                b3 = temp4;
                b3 += b;

                buffer2[x * 3] = b1;
                buffer2[x * 3 + 1] = b2;
                buffer2[x * 3 + 2] = b3;
            }

            //remove paddings
            length3 = length2 - paddingCount;

            byte[] result = new byte[length3];

            for (int x = 0; x < length3; x++)
            {
                result[x] = buffer2[x];
            }

            return result;
        }
    }
}
