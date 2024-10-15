using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;

namespace Puya.Core.Cryptography
{
    public class RSAKeyWriter
    {
        private RSACryptoServiceProvider _csp;
        public RSAParameters PrivateKey { get; private set; }
        public string PrivateKeyXml { get; private set; }
        public RSAParameters PublicKey { get; private set; }
        public string PubicKeyXml { get; private set; }
        public RSAKeyWriter(CspParameters parameters)
        {
            _csp = new RSACryptoServiceProvider(parameters);

            ExportKeys();
        }
        public RSAKeyWriter(int dwKeySize)
        {
            _csp = new RSACryptoServiceProvider(dwKeySize);

            ExportKeys();
        }
        public RSAKeyWriter(int dwKeySize, CspParameters parameters)
        {
            _csp = new RSACryptoServiceProvider(dwKeySize, parameters);

            ExportKeys();
        }
        void ExportKeys()
        {
            PrivateKey = _csp.ExportParameters(true);
            PublicKey = _csp.ExportParameters(false);

            PrivateKeyXml = ExportParametersToString(PrivateKey);

            PubicKeyXml = ExportParametersToString(PublicKey);
        }
        string ExportParametersToString(RSAParameters key)
        {
            using (var sw = new StringWriter())
            {
                var xs = new XmlSerializer(typeof(RSAParameters));

                xs.Serialize(sw, key);

                return sw.ToString();
            }
        }
    }
}
