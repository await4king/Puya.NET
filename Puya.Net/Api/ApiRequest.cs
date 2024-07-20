using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Puya.Api
{
    public class ApiRequest
    {
        public int Id { get; set; }
        public string App { get; set; }
        public string Api { get; set; }
        public string Lang { get; set; }
        public JObject Data { get; set; }
        public virtual object ConvertData(Type type)
        {
            var result = Data == null ? Activator.CreateInstance(type): Data.ToObject(type);

            return result;
        }
    }
    public class ApiRequest<T> : ApiRequest
    {
        public T StrongData { get; set; }
        public ApiRequest()
        {
        }
        public ApiRequest(T data)
        {
            StrongData = data;
        }
        public static ApiRequest<T> Parse(string request)
        {
            var result = new ApiRequest<T>();

            if (!string.IsNullOrEmpty(request))
            {
                var req = JsonConvert.DeserializeObject<ApiRequest>(request);
                
                result = new ApiRequest<T>(req);
            }

            return result;
        }
        public static object Parse(Type type, string request)
        {
            var result = null as object;

            if (!string.IsNullOrEmpty(request))
            {
                result = JsonConvert.DeserializeObject(request, type);
            }

            return result;
        }
        public ApiRequest(ApiRequest request)
        {
            if (request.Data != null)
            {
                StrongData = request.Data.ToObject<T>();
                Data = request.Data;
                Api = request.Api;
                App = request.App;
            }
        }
    }
}
