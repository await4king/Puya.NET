using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Puya.Collections;
using Puya.Service;

namespace Puya.Api
{
    public class ApiManagerFindApiRequest
    {
        public ApiRequest Request { get; set; }
    }
    public class ApiManagerFindApiResponse : ServiceResponse<Api> { }
    public class ApiManagerFindServiceRequest
    {
        public string FullServiceName { get; set; }
    }
    public class ApiManagerFindServiceResponse : ServiceResponse<Type> { }
    public class ApiManagerFindAppByIdRequest
    {
        public int Id { get; set; }
    }
    public class ApiManagerFindAppByIdResponse : ServiceResponse<Application> { }
    public class ApiManagerFindAppByPathRequest
    {
        public string Path { get; set; }
    }
    public class ApiManagerFindAppByPathResponse : ServiceResponse<Application> { }
    public class ApiManagerCreateStoreRequest
    {
        public IDictionary<string, object> Options { get; set; }
        public ApiManagerCreateStoreRequest()
        {
            Options = new CaseInsensitiveDictionary<object>(true);
        }
    }
    public class ApiManagerCreateStoreResponse : ServiceResponse { }

    public interface IApiManager
    {
        ApiManagerFindAppByIdResponse FindAppById(ApiManagerFindAppByIdRequest request);
        Task<ApiManagerFindAppByIdResponse> FindAppByIdAsync(ApiManagerFindAppByIdRequest request, CancellationToken cancellation);
        ApiManagerFindAppByPathResponse FindAppByPath(ApiManagerFindAppByPathRequest request);
        Task<ApiManagerFindAppByPathResponse> FindAppByPathAsync(ApiManagerFindAppByPathRequest request, CancellationToken cancellation);
        ApiManagerFindApiResponse FindApi(ApiManagerFindApiRequest request);
        Task<ApiManagerFindApiResponse> FindApiAsync(ApiManagerFindApiRequest request, CancellationToken cancellation);
        ApiManagerFindServiceResponse FindService(ApiManagerFindServiceRequest request);
        Task<ApiManagerFindServiceResponse> FindServiceAsync(ApiManagerFindServiceRequest request, CancellationToken cancellation);
        ApiManagerCreateStoreResponse CreateStore(ApiManagerCreateStoreRequest request);
        Task<ApiManagerCreateStoreResponse> CreateStoreAsync(ApiManagerCreateStoreRequest request, CancellationToken cancellation);
    }
}
