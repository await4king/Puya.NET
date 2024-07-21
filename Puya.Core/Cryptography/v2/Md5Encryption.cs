using System;
using System.Security.Cryptography;

namespace Puya.Cryptography.v2
{
    public class Md5Encryption : IMd5Encryption
    {
        public byte[] Compute(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return new byte[] { };
            }

            using (var md5 = MD5.Create())
            {
                var bytes = data;
                var hash = md5.ComputeHash(bytes);

                return hash;
            }
        }
    }
}
