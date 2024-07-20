using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Base;
using Puya.Conversion;
using Puya.Extensions;
using Puya.Logging;
using Puya.Service;

namespace Puya.Api
{
    public class ApiEngineDefault : IApiEngine
    {
        protected readonly IServiceProvider serviceProvider;
        protected readonly IApiManager apis;
        protected readonly IApiCryptor cryptor;
        protected readonly ILogger logger;
        protected readonly IApiResponseSerializer serializer;
        protected readonly IMiddlewaresStore middlewaresStore;
        public ApiEngineDefault(IServiceProvider serviceProvider,
                                IApiManager apis,
                                IApiCryptor cryptor,
                                IApiResponseSerializer serializer,
                                ILogger logger,
                                IMiddlewaresStore middlewaresStore)
        {
            this.serviceProvider = serviceProvider;
            this.apis = apis;
            this.cryptor = cryptor;
            this.logger = logger;
            this.serializer = serializer;
            this.middlewaresStore = middlewaresStore;
        }
        public string DefaultApp { get; set; }
        void Danger(Exception e, object data = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            try
            {
                logger?.Danger(e, null, data, memberName, sourceFilePath, sourceLineNumber);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString("\n"));
            }
        }
        void RevealException(ApiCallContext context, Exception e)
        {
            if (ApiEngineConstants.RevealExceptions || context.RevealExceptions)
            {
                if (context.Response.Exception == null)
                {
                    context.Response.Exception = e;
                }
                else
                {
                    context.Response.Exception = new AggregateException("Api failed. See details.", context.Response.Exception, e);
                }

                context.Response.Message = e.ToString("\n");
            }
        }
        private bool DecryptRequest(ApiCallContext context, string body, out string decryptedBody)
        {
            var result = false;

            decryptedBody = body;

            if (IsEncryptedRequest(context))
            {
                if (string.IsNullOrEmpty(body))
                {
                    context.Response.SetStatus("NoRequest");
                }
                else
                {
                    try
                    {
                        context._state = ApiState.DecryptingRequest;

                        if (cryptor != null)
                        {
                            decryptedBody = cryptor.Decrypt(context, body);

                            result = context.ApiRequestDecrypted = true;
                        }
                        else
                        {
                            context.Response.SetStatus("NoApiDecryptorFound");
                        }
                    }
                    catch (Exception e)
                    {
                        Danger(e);

                        context.Response.SetStatus("BadRequest");

                        RevealException(context, e);
                    }
                }
            }
            else
            {
                result = true;
            }

            return result;
        }
        private bool DeserializeRequest(ApiCallContext context, string body, out ApiRequest request)
        {
            var result = false;

            request = null;

            if (!string.IsNullOrEmpty(body))
            {
                request = new ApiRequest();

                try
                {
                    context._state = ApiState.DeserializingRequest;

                    var _request = JsonConvert.DeserializeObject<ApiRequest>(body);

                    if (_request != null)
                    {
                        request.App = _request.App;
                        request.Id = _request.Id;
                        request.Data = _request.Data;
                        request.Api = _request.Api;
                        request.Lang = _request.Lang;

                        result = true;
                    }
                }
                catch (Exception e)
                {
                    Danger(e);

                    context.Response.SetStatus("InvalidRequest");

                    RevealException(context, e);
                }
            }
            else
            {
                context.Response.SetStatus("NoRequest");
            }

            return result;
        }
        private bool GetRequestFromBody(ApiCallContext context, string body, out ApiRequest request)
        {
            var result = false;

            request = null;

            do
            {
                if (string.IsNullOrEmpty(body))
                {
                    context.Response.SetStatus("NoRequest");
                    break;
                }

                if (!DecryptRequest(context, body, out body))
                {
                    break;
                }

                context._state = ApiState.ValidatingRawRequest;

                result = DeserializeRequest(context, body, out ApiRequest _request);

                if (result)
                {
                    request = _request;
                }

            } while (false);

            return result;
        }
        private bool GetRequest(ApiCallContext context, IEnumerable<KeyValuePair<string, StringValues>> data, out ApiRequest request)
        {
            var result = true;

            request = new ApiRequest();

            foreach (var item in data)
            {
                var value = item.Value.ToString();

                if (string.Compare(item.Key, "api", StringComparison.CurrentCultureIgnoreCase) == 0 || string.Compare(item.Key, "path", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    request.Api = value;
                    continue;
                }
                if (string.Compare(item.Key, "app", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    request.App = value;
                    continue;
                }
                if (string.Compare(item.Key, "id", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    request.Id = SafeClrConvert.ToInt(value);
                    continue;
                }
                if (string.Compare(item.Key, "data", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    try
                    {
                        request.Data = (JObject)JsonConvert.DeserializeObject(value);
                    }
                    catch (Exception e)
                    {
                        Danger(e);

                        context._state = ApiState.DeserializingRequestData;
                        context.Response.SetStatus("RequestDataError");
                        result = false;
                        break;
                    }

                    continue;
                }
                if (string.Compare(item.Key, "body", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    result = GetRequestFromBody(context, value, out request);

                    break;
                }
            }

            return result;
        }
        protected virtual async Task<bool> RunMiddlewares(ApiEngineEvents engineEvent, ApiCallContext context, CancellationToken cancellation)
        {
            var shouldEndPipeline = false;
            var middlewares = middlewaresStore?.GetMiddlewares(engineEvent);

            if (middlewares != null)
            {
                context._state = (ApiState)Enum.Parse(typeof(ApiState), engineEvent.ToString());

                var count = 0;

                foreach (var middleware in middlewares)
                {
                    if (middleware != null)
                    {
                        try
                        {
                            var sr = await middleware.RunAsync(context, engineEvent, cancellation);

                            if (sr != null)
                            {
                                if ((ApiEngineConstants.ShowDetailedEnginePipeline || context.ShowDetailedEnginePipeline) && !sr.Success)
                                {
                                    sr.Subject = middleware.ToString();

                                    context.Response.InnerResponses.Add(sr);
                                }

                                if (sr.ShouldEndPipeline)
                                {
                                    if (string.IsNullOrEmpty(context.Response.Status))
                                    {
                                        context.Response.SetStatus(sr.Status);
                                    }

                                    shouldEndPipeline = true;

                                    break;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            shouldEndPipeline = true;

                            context.Response.SetStatus("MiddlewareFailed");
                            context.Response.Info = middleware.GetType().Name;

                            RevealException(context, e);

                            Danger(e, new { Event = engineEvent, Middleware = middleware.GetType().Name });
                        }
                    }
                    else
                    {
                        logger.Warn("ApiEngine", $"Middleware {count} is null");
                    }

                    count++;
                }
            }

            return shouldEndPipeline;
        }
        protected virtual bool IsEncryptedRequest(ApiCallContext context)
        {
            return string.Compare(context.HttpContext.Request.Headers[ApiEngineConstants.EncryptedRequestHeaderName], "true", StringComparison.CurrentCultureIgnoreCase) == 0 ||
                   string.Compare(context.HttpContext.Request.Headers[ApiEngineConstants.EncryptedRequestHeaderName], "yes", StringComparison.CurrentCultureIgnoreCase) == 0;
        }
        protected virtual async Task<bool> GetApiRequestAsync(ApiCallContext context, CancellationToken cancellation)
        {
            var result = null as ApiRequest;
            var body = "";
            var oldStatus = context.Response.Status;

            do
            {
                context._state = ApiState.CreatingApiRequest;

                if (context.HttpContext.Request.HasFormContentType)
                {
                    context._state = ApiState.ReadingRequestForm;

                    if (!GetRequest(context, context.HttpContext.Request.Form, out result))
                    {
                        break;
                    }
                }
                else
                {
                    context._state = ApiState.ReadingRequestBody;

                    using (var reader = new StreamReader(context.HttpContext.Request.Body,
                                                    encoding: Encoding.UTF8,
                                                    detectEncodingFromByteOrderMarks: false,
                                                    bufferSize: ApiEngineConstants.BodyStreamBufferSize,
                                                    leaveOpen: true))
                    {
                        body = await reader.ReadToEndAsync();
                    }

                    // we don't care whether the content-type header has a correct value or even exists at all!
                    // so there is no need to check context-type header to see if given body is json or not and report an error
                    // if the body misses such header.
                    //    if (string.Compare(Request.ContentType, "application/json", StringComparison.CurrentCultureIgnoreCase) != 0)
                    //    {
                    //      response.SetStatus("ExpectedJson");
                    //      break;
                    //    }
                    // we read all body data. if there is no body, we fall back to querystring as a last resort.
                    // we can't fallback to form, since Request.Form only works when there is a correct Contet-Type header.
                    // this has handled at first. So, Forms have more priority both over body and querystring.

                    if (!string.IsNullOrEmpty(body)) // body has more priority over querystring.
                    {
                        if (!GetRequestFromBody(context, body, out result))
                        {
                            break;
                        }
                    }
                    else  // if we don't have body, we check querystring.
                    {
                        context._state = ApiState.ReadingRequestQueryString;

                        if (!GetRequest(context, context.HttpContext.Request.Query, out result))
                        {
                            break;
                        }
                    }
                }

                context._state = ApiState.ValidatingRequest;

                if (string.IsNullOrEmpty(result.Api) && result.Id <= 0)
                {
                    context.Response.SetStatus("NoApi");
                    break;
                }

                context.ApiRequest = result;

                context._state = ApiState.FinalizingRequest;

                if (string.IsNullOrEmpty(result.App))
                {
                    result.App = DefaultApp;
                }
            } while (false);

            return string.Compare(context.Response.Status, oldStatus, StringComparison.OrdinalIgnoreCase) == 0;
        }
        protected virtual async Task<bool> GetApiAsync(ApiCallContext context, CancellationToken cancellation)
        {
            var oldStatus = context.Response.Status;

            do
            {
                context._state = ApiState.FindingApp;

                var fapr = await apis.FindAppByPathAsync(new ApiManagerFindAppByPathRequest { Path = context.ApiRequest.App }, cancellation);

                if ((ApiEngineConstants.ShowDetailedEnginePipeline || context.ShowDetailedEnginePipeline) && !fapr.Success)
                {
                    fapr.Subject = "FindApp";

                    context.Response.InnerResponses.Add(fapr);
                }

                if (!fapr.IsSucceeded())
                {
                    if (fapr.IsNotFound())
                    {
                        context.Response.SetStatus("AppNotFound");
                    }
                    else
                    {
                        context.Response.SetStatus($"FindApp{fapr.Status}");
                    }

                    break;
                }

                context._state = ApiState.ValidatingApp;

                context.App = fapr.Data;

                if (context.App.Disabled)
                {
                    context.Response.SetStatus("AppDisabled");
                    break;
                }

                context._state = ApiState.FindingApi;

                var far = await apis.FindApiAsync(new ApiManagerFindApiRequest { Request = context.ApiRequest }, cancellation);

                if ((ApiEngineConstants.ShowDetailedEnginePipeline || context.ShowDetailedEnginePipeline) && !far.Success)
                {
                    far.Subject = "FindApi";

                    context.Response.InnerResponses.Add(far);
                }

                if (!far.IsSucceeded())
                {
                    if (far.IsNotFound())
                    {
                        context.Response.SetStatus("ApiNotFound");
                    }
                    else
                    {
                        context.Response.SetStatus($"FindApi{far.Status}");
                    }

                    break;
                }

                context._state = ApiState.ValidatingApi;

                context.Api = far.Data;

                if (context.Api.Apps.IndexOf(context.App.Id) < 0)
                {
                    context.Response.SetStatus("ApiAppDenied");
                    break;
                }

                if (context.Api.Disabled)
                {
                    context.Response.SetStatus("ApiDisabled");
                    break;
                }

                if (SafeClrConvert.ToBoolean(context.Api.Settings[ApiEngineConstants.ApiSettingsEncryptedRequestName]) && !context.ApiRequestDecrypted)
                {
                    context.Response.SetStatus("RequiresEncryptedRequest");
                    break;
                }

                if (!context.Api.Accepts(context.HttpContext.Request.Method))
                {
                    context.Response.SetStatus("IncorrectVerb");
                    break;
                }

                if (string.IsNullOrEmpty(context.Api.Service))
                {
                    context.Response.SetStatus("ApiMissingService");
                    context.Response.Message = "No service is defined for this api. Contact api provider team.";

                    break;
                }
            } while (false);

            return string.Compare(context.Response.Status, oldStatus, StringComparison.OrdinalIgnoreCase) == 0;
        }
        protected virtual async Task<bool> GetServiceAsync(ApiCallContext context, CancellationToken cancellation)
        {
            var oldStatus = context.Response.Status;

            do
            {
                context._state = ApiState.FindingService;

                var fsr = await apis.FindServiceAsync(new ApiManagerFindServiceRequest { FullServiceName = context.Api.Service }, cancellation);

                if (!fsr.IsSucceeded())
                {
                    if (fsr.IsNotFound())
                    {
                        context.Response.SetStatus("ServiceNotFound");
                    }
                    else
                    {
                        context.Response.SetStatus($"FindService{fsr.Status}");
                    }

                    break;
                }

                context.ServiceType = fsr.Data;

                try
                {
                    context._state = ApiState.ResolvingService;
                    context.Service = context.Scope.ServiceProvider.GetService(context.ServiceType) as IService;
                }
                catch (Exception e)
                {
                    Danger(e);

                    context.Response.SetStatus("ServiceResolutionFailed");

                    RevealException(context, e);

                    break;
                }

                context._state = ApiState.ValidatingService;

                if (context.Service == null)
                {
                    context.Response.SetStatus("ServiceIsNull");
                    break;
                }

                context._state = ApiState.FindingAction;

                try
                {
                    context.Action = context.Service.GetAction(context.Api.Action) as IServiceAction;
                }
                catch (Exception e)
                {
                    Danger(e);

                    context.Response.SetStatus("ServiceActionResolutionFailed");

                    RevealException(context, e);

                    break;
                }

                context._state = ApiState.ValidatingAction;

                if (context.Action == null)
                {
                    context.Response.SetStatus("ServiceActionIsNull");
                    break;
                }

                context._state = ApiState.ValidatingActionRequest;

                // all actions are sub-class of ServiceAction<TService, TRequest, TResponse>
                // the generic type of the request is the second generic argument

                var genericTypeArgs = context.Action.GetType().GetGenericTypeArguments();
                var requestType = genericTypeArgs.FirstOrDefault(t => t.DescendsFrom<ServiceRequest>());

                if (requestType == null)
                {
                    context.Response.SetStatus("ProblematicApiAction");
                    break;
                }

                var actionRequest = null as object;

                context._state = ApiState.ConvertingActionRequest;

                try
                {
                    actionRequest = context.ApiRequest.ConvertData(requestType);
                }
                catch
                {
                    try
                    {
                        actionRequest = context.ApiRequest.ConvertData(TypeHelper.TypeOfObject);
                    }
                    catch (Exception e2)
                    {
                        Danger(e2);
                    }
                }

                var actionRequestWithApiCallContext = actionRequest as IApiServiceRequest;

                if (actionRequestWithApiCallContext != null)
                {
                    actionRequestWithApiCallContext.CallContext = context;
                }

                context.ServiceCallRequest = actionRequest;
            } while (false);

            return string.Compare(context.Response.Status, oldStatus, StringComparison.OrdinalIgnoreCase) == 0;
        }
        protected virtual async Task<bool> RunActionAsync(ApiCallContext context, CancellationToken cancellation)
        {
            var oldStatus = context.Response.Status;

            try
            {
                context._state = ApiState.RunningAction;

                if (context.Api.Async)
                {
                    context.ServiceCallResponse = await context.Action.RunAsync(context.ServiceCallRequest, cancellation);
                }
                else
                {
                    context.ServiceCallResponse = context.Action.Run(context.ServiceCallRequest);
                }

                context._state = ApiState.CopyingActionResponse;

                context.Response = context.ServiceCallResponse;
            }
            catch (Exception e)
            {
                Danger(e);

                context.Response.SetStatus("ActionFailed");

                RevealException(context, e);
            }

            if (context.Response?.Status != "ActionFailed")
            {
                if (context.ServiceCallResponse != null)
                {
                    //    var prop = context.ServiceCallResponse.GetType().GetProperty("Data");

                    //    if (prop != null)
                    //    {
                    //        context._state = ApiState.CopyingActionResponseData;

                    //        var newResponse = new ServiceResponse<object>();

                    //        newResponse.Copy(context.Response);
                    //        newResponse.Data = prop.GetValue(context.ServiceCallResponse);
                    //        context.Response = newResponse;
                    //    }

                    return context.Response.IsSucceeded();
                }
                else
                {
                    context.Response = new ServiceResponse();
                    context.Response.SetStatus("ActionResponseNull");
                }
            }

            return false;
        }
        protected virtual string Serialize(HttpContext httpContext, ApiCallContext apiContext)
        {
            if (apiContext.Response.Exception != null && ApiEngineConstants.RevealExceptions)
            {
                apiContext.Response.Exception = null;
            }

            apiContext._state = ApiState.SerializingResponse;

            var content = "";

            try
            {
                content = serializer.Serialize(apiContext.Response);

                if (apiContext.Api != null && SafeClrConvert.ToBoolean(apiContext.Api.Settings[ApiEngineConstants.ApiSettingsEncryptedResponseName]))
                {
                    try
                    {
                        apiContext._state = ApiState.EncryptingResponse;

                        content = cryptor.Encrypt(apiContext, content);
                        httpContext.Response.Headers[ApiEngineConstants.EncryptedResponseHeaderName] = "true";
                    }
                    catch (Exception e)
                    {
                        Danger(e);

                        content = "{ \"Success\": false, \"Status\": \"ResponseEncryptionFailed\"}";
                    }
                }
            }
            catch (Exception e)
            {
                Danger(e);

                content = "{ \"Success\": false, \"Status\": \"ResponseSerializingFailed\"}";
            }

            return content;
        }
        public virtual async Task<string> Serve(HttpContext httpContext, CancellationToken cancellation)
        {
            using (var scope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope())
            {
                var apiCallContext = new ApiCallContext { HttpContext = httpContext, Response = new ServiceResponse(), Scope = scope };

                httpContext.Items.Add("Tap-ApiCallContext", apiCallContext);

                try
                {
                    do
                    {
                        if (await RunMiddlewares(ApiEngineEvents.Starting, apiCallContext, cancellation))
                        {
                            break;
                        }

                        if (!await GetApiRequestAsync(apiCallContext, cancellation))
                        {
                            break;
                        }

                        if (await RunMiddlewares(ApiEngineEvents.Loading, apiCallContext, cancellation))
                        {
                            break;
                        }

                        if (!await GetApiAsync(apiCallContext, cancellation))
                        {
                            break;
                        }

                        if (await RunMiddlewares(ApiEngineEvents.Locating, apiCallContext, cancellation))
                        {
                            break;
                        }

                        if (!await GetServiceAsync(apiCallContext, cancellation))
                        {
                            break;
                        }

                        if (await RunMiddlewares(ApiEngineEvents.Executing, apiCallContext, cancellation))
                        {
                            break;
                        }

                        await RunActionAsync(apiCallContext, cancellation);

                        if (await RunMiddlewares(ApiEngineEvents.Serializing, apiCallContext, cancellation))
                        {
                            break;
                        }
                    } while (false);
                }
                catch (Exception e)
                {
                    try
                    {
                        Danger(e, apiCallContext);
                    }
                    catch
                    {
                        Danger(e);
                    }

                    apiCallContext.Response.SetStatus("Halted");

                    RevealException(apiCallContext, e);
                }

                var content = Serialize(httpContext, apiCallContext);

                return content;
            }
        }
    }
}
