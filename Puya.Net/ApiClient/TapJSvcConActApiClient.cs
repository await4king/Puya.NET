using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Puya.Extensions;
using Puya.Serialization;
using Puya.Service;

namespace Puya.ApiClient
{
    public class TapJSvcConActApiClient
    {
        public TapJSvcConActApiClient(TapJSvcConActApiClientConfig config)
        {
            Config = config;
        }

        public TapJSvcConActApiClientConfig Config { get; }
        public async Task<TResponse> InvokeAsync<TRequest, TResponse>(TRequest request, Func<HttpClient, CancellationToken, Task<HttpResponseMessage>> fnInvoke, CancellationToken cancellation)
            where TRequest : ServiceRequest
            where TResponse : ServiceResponse, new()
        {
            var response = new TResponse();
            var client = new HttpClient();
            
            try
            {
                var rm = await fnInvoke(client, cancellation);

                if (rm.IsSuccessStatusCode)
                {
                    var body = await rm.Content.ReadAsStringAsync();
                    var sr = body.SafeDeserialize<TResponse>();

                    if (sr == null)
                    {
                        response.SetStatus("ResponseError");
                        response.Info = body;
                    }
                    else
                    {
                        response = sr;
                    }
                }
                else
                {
                    response.SetStatus("NotOk");
                    response.Info = rm.StatusCode.ToString();
                }
            }
            catch (Exception e)
            {
                response.SetStatus("InvokeError", e);
            }

            return response;
        }
        public Task<TResponse> PostAsync<TRequest, TResponse>(string path, TRequest request, CancellationToken cancellation)
            where TRequest : ServiceRequest
            where TResponse : ServiceResponse, new()
        {
            return InvokeAsync<TRequest, TResponse>(request, (client, CancellationToken) =>
            {
                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                return client.PostAsync(Config.EndPoint + path, content);
            }, cancellation);
        }
        public Task<TResponse> PutAsync<TRequest, TResponse>(string path, TRequest request, CancellationToken cancellation)
            where TRequest : ServiceRequest
            where TResponse : ServiceResponse, new()
        {
            return InvokeAsync<TRequest, TResponse>(request, (client, CancellationToken) =>
            {
                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                return client.PutAsync(Config.EndPoint + path, content);
            }, cancellation);
        }
        public Task<TResponse> DeleteAsync<TRequest, TResponse>(string path, CancellationToken cancellation)
            where TRequest : ServiceRequest
            where TResponse : ServiceResponse, new()
        {
            return InvokeAsync<TRequest, TResponse>(null, (client, CancellationToken) =>
            {
                return client.DeleteAsync(Config.EndPoint + path, cancellation);
            }, cancellation);
        }
        public Task<TResponse> GetAsync<TRequest, TResponse>(string path, TRequest request, CancellationToken cancellation)
            where TRequest : ServiceRequest
            where TResponse : ServiceResponse, new()
        {
            return InvokeAsync<TRequest, TResponse>(request, (client, CancellationToken) =>
            {
                var builder = new UriBuilder(Config.EndPoint + path);

                if (request != null)
                {
                    builder.Query = request.ToQuerystring();
                }

                var url = builder.ToString();

                return client.GetAsync(url);
            }, cancellation);
        }
    }
}
