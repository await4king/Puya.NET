using System;

namespace Puya.Api
{
    public interface IApiClient
    { }
    public class ApiClient<T> : IApiClient
    {
        public T Id { get; set; }
        public string uid { get; set; }
        public bool Disabled { get; set; }
        public bool IsBanned { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class ApiClient : ApiClient<int>
    { }
    public enum ApiClientIdentityType
    {
        Mobile = 0,
        Email = 1
    }
    public interface IApiClientIdentity<T>
    {
        string Identity { get; set; }
        ApiClient<T> Client { get; set; }
        ApiClientIdentityType Type { get; set; }
        string ClientKey { get; set; }
        string ClientSecret { get; set; }
    }
    public class ApiClientIdentity<T>: IApiClientIdentity<T>
    {
        public T ApiConsumerId { get; set; }
        public ApiClient<T> Client { get; set; }
        public string Identity { get; set; }
        public ApiClientIdentityType Type { get; set; }
        public string ClientKey { get; set; }
        public string ClientSecret { get; set; }
    }
    public interface IApiClientIdentityConsumer<T>
    {
        IApiClientIdentity<T> ClientIdentity { get; set; }
        string HardwareCode { get; set; }
        DateTime RegisterDate { get; set; }
        DateTime ActivationDate { get; set; }
        DateTime RefreshDate { get; set; }
        DateTime NewRefreshDate { get; set; }
        bool Activated { get; set; }
        bool IsActive { get; set; }
        string ActivationCode { get; set; }
        string EncryptionKey { get; set; }
        string IdentityHash { get; set; }
        string AccessToken { get; set; }
        string RefreshToken { get; set; }
        string NewEncryptionKey { get; set; }
        string NewIdentityHash { get; set; }
        string NewAccessToken { get; set; }
        string NewRefreshToken { get; set; }
        int LifeLength { get; set; }
    }
}
