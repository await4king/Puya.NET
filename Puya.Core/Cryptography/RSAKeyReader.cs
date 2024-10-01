using System;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.Xml;

namespace Puya.Core.Cryptography
{
    public class RSAKeyReader
    {
        private RSAParameters? _publicKey;
        private RSAParameters? _privateKey;
        string _publicKeyXml;
        string _privateKeyXml;
        public string PublicKeyXml
        {
            get
            {
                return _publicKeyXml;
            }
            set
            {
                _publicKeyXml = value;
                _publicKey = null;
            }
        }
        public string PrivateKeyXml
        {
            get
            {
                return _privateKeyXml;
            }
            set
            {
                _privateKeyXml = value;
                _privateKey = null;
            }
        }
        public RSAKeyReader()
        { }
        public RSAKeyReader(string privateKey, string publicKey)
        {
            PublicKeyXml = publicKey;
            PrivateKeyXml = privateKey;
        }
        public RSAParameters GetPrivateKey()
        {
            if (_privateKey == null)
            {
                _privateKey = ImportPrivateKey(PrivateKeyXml);
            }
            
            return _privateKey.Value;
        }
        public RSAParameters GetPublicKey()
        {
            if (_publicKey == null)
            {
                _publicKey = ImportPublicKey(PublicKeyXml);
            }

            return _publicKey.Value;
        }
        private RSAParameters ImportPrivateKey(string xmlString)
        {
            RSAParameters rsaParameters = new RSAParameters();

            var xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(xmlString);

            rsaParameters.Modulus = Convert.FromBase64String(xmlDoc.GetElementsByTagName("Modulus")[0].InnerText);
            rsaParameters.Exponent = Convert.FromBase64String(xmlDoc.GetElementsByTagName("Exponent")[0].InnerText);
            rsaParameters.P = Convert.FromBase64String(xmlDoc.GetElementsByTagName("P")[0]?.InnerText);
            rsaParameters.Q = Convert.FromBase64String(xmlDoc.GetElementsByTagName("Q")[0]?.InnerText);
            rsaParameters.DP = Convert.FromBase64String(xmlDoc.GetElementsByTagName("DP")[0]?.InnerText);
            rsaParameters.DQ = Convert.FromBase64String(xmlDoc.GetElementsByTagName("DQ")[0]?.InnerText);
            rsaParameters.InverseQ = Convert.FromBase64String(xmlDoc.GetElementsByTagName("InverseQ")[0]?.InnerText);
            rsaParameters.D = Convert.FromBase64String(xmlDoc.GetElementsByTagName("D")[0]?.InnerText);

            return rsaParameters;
        }
        private RSAParameters ImportPublicKey(string xmlString)
        {
            using (var sr = new StringReader(xmlString))
            {
                var xs = new XmlSerializer(typeof(RSAParameters));

                return (RSAParameters)xs.Deserialize(sr);
            }
        }
    }
}
