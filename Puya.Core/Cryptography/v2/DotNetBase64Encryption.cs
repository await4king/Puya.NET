using System;

namespace Puya.Cryptography.v2
{
    public class DotNetBase64Encryption : IBase64Encryption
    {
        public string Encode(byte[] data)
        {
            return Convert.ToBase64String(data);
        }
        public byte[] Decode(string data)
        {
            return Convert.FromBase64String(data);
        }
    }
}
