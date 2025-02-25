using System.Security.Cryptography;

namespace Puya.Core.Cryptography
{
    public class RSAHelper
    {
        public RSAParameters? PrivateKey { get; set; }
        public RSAParameters? PublicKey { get; set; }

        public RSAHelper(RSAParameters? privateKey, RSAParameters? publicKey)
        {
            this.PrivateKey = privateKey;
            this.PublicKey = publicKey;
        }

        public byte[] Encrypt(byte[] bytesPlainTextData)
        {
            if (this.PublicKey != null)
            {
                var csp = new RSACryptoServiceProvider();

                csp.ImportParameters((RSAParameters)this.PublicKey);

                byte[] bytesCypherText = csp.Encrypt(bytesPlainTextData, false);

                return bytesCypherText;
            }

            return null;
        }

        public byte[] Decript(byte[] bytesCypherText)
        {
            if (this.PrivateKey != null)
            {
                var csp = new RSACryptoServiceProvider();

                csp.ImportParameters((RSAParameters)this.PrivateKey);

                byte[] bytesPlainTextData = csp.Decrypt(bytesCypherText, false);

                return bytesPlainTextData;
            }

            return null;
        }
    }
}
