using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Cryptography
{
    public class Base64
    {
        public IBase64Encoder Encoder { get; set; }
        public IBase64Decoder Decoder { get; set; }
        public Base64()
        {
            Encoder = new Puya.Cryptography.Base64Encoder();
            Decoder = new Puya.Cryptography.Base64Decoder();
        }
    }
}
