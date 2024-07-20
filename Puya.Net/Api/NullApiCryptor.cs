namespace Puya.Api
{
    public class NullApiCryptor : IApiCryptor
    {
        public string Decrypt(ApiCallContext context, string data)
        {
            return data;
        }
        public string Encrypt(ApiCallContext context, string data)
        {
            return data;
        }
    }
}
