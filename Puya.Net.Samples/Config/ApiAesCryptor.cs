using Puya.Api;
using Puya.Cryptography;
using Puya.Cryptography.v2;
using System.Text;

namespace Puya.Net.Samples.Config
{
    public class ApiAesCryptor : IApiCryptor
    {
        private readonly IAesEncryption aesEncryption;
        private readonly IBase64Encryption base64Encryption;
        private readonly IConfiguration configuration;

        public ApiAesCryptor(IAesEncryption aesEncryption, IBase64Encryption base64Encryption, IConfiguration configuration)
        {
            this.aesEncryption = aesEncryption;
            this.base64Encryption = base64Encryption;
            this.configuration = configuration;
        }
        string GetKey(ApiCallContext context)
        {
            return "1234567890123456";
        }
        string GetIV()
        {
            return "0123456776543210";
        }
        public string Decrypt(ApiCallContext context, string data)
        {
            var key = GetKey(context);
            var bytes = base64Encryption.Decode(data);
            var decryptedBody = aesEncryption.Decrypt(bytes, Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(GetIV()), "CBC", "PKCS7");

            return decryptedBody;
        }

        public string Encrypt(ApiCallContext context, string data)
        {
            var key = GetKey(context);
            var bytes = aesEncryption.Encrypt(data, Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(GetIV()), "CBC", "PKCS7");
            var result = base64Encryption.Encode(bytes);

            return result;
        }
    }
}
