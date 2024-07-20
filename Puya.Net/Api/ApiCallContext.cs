using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Puya.Service;

namespace Puya.Api
{
    public class ApiCallContext
    {
        internal ApiState _state { get; set; }
        public string State => _state.ToString();
        public Api Api { get; set; }
        public ApiRequest ApiRequest { get; set; }
        public bool ApiRequestDecrypted { get; set; }
        public Application App { get; set; }
        [JsonIgnore]
        public HttpContext HttpContext { get; set; }
        public Type ServiceType { get; set; }
        public IService Service { get; set; }
        public IServiceAction Action { get; set; }
        public object ServiceCallRequest { get; set; }
        public ServiceResponse ServiceCallResponse { get; set; }
        public ServiceResponse Response { get; set; }
        public Dictionary<string, object> Data { get; set; }
        public bool RevealExceptions { get; set; }
        public bool ShowDetailedEnginePipeline { get; set; }
        public IServiceScope Scope { get; set; }
        public ApiCallContext()
        {
            Data = new Dictionary<string, object>();
        }
    }
}
