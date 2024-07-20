namespace Puya.Api
{
    public interface IApiCryptor
    {
        string Encrypt(ApiCallContext context, string data);
        string Decrypt(ApiCallContext context, string data);
    }
}
