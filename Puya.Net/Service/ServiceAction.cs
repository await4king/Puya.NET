using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Base;
using Puya.Data;
using Puya.Extensions;
using Puya.Reflection;

namespace Puya.Service
{
    public interface IServiceAction
    {
        object Owner { get; }
        ServiceResponse Run(object request);
        Task<ServiceResponse> RunAsync(object request, CancellationToken token);
    }
    public interface IServiceAction<TService, TRequest, TResponse>: IServiceAction
        where TService: class
    {
        new TService Owner { get; }
        TResponse Run(TRequest request);
        Task<TResponse> RunAsync(TRequest request, CancellationToken token);
    }
    public abstract class ServiceAction<TService, TRequest, TResponse> : IServiceAction<TService, TRequest, TResponse>
        where TService: class, IService
        where TRequest : ServiceRequest
        where TResponse : ServiceResponse, new()
    {
        public TService Owner { get; private set; }
        object IServiceAction.Owner
        {
            get { return this.Owner; }
        }
        public virtual string Name { get { return this.GetType().Name; } }
        public ServiceAction(TService owner)
        {
            Owner = owner ?? throw new ArgumentException(nameof(owner));
        }
        protected abstract void RunInternal(TRequest request, TResponse response);
        protected abstract Task RunInternalAsync(TRequest request, TResponse response, CancellationToken token);
        protected virtual void OnError(TRequest request, TResponse response, Exception e)
        {
        }
        protected virtual bool OnBeforeRun(TRequest request, TResponse response)
        {
            return true;
        }
        protected virtual void OnAfterRun(TRequest request, TResponse response)
        {
        }
        public virtual TResponse Run(TRequest request)
        {
            var response = new TResponse();

            try
            {
                if (OnBeforeRun(request, response))
                {
                    RunInternal(request, response);

                    OnAfterRun(request, response);
                }
            }
            catch (Exception e)
            {
                response.Flawed(e);

                OnError(request, response, e);
            }

            return response;
        }
        protected virtual TRequest MapRequest(object request)
        {
            var result = ObjectActivator.Instance.Activate<TRequest>() as TRequest;
            var props = ReflectionHelper.GetPublicInstanceProperties<TRequest>();

            ReflectionHelper.ForEachPublicInstanceReadableProperty(request.GetType(), prop =>
            {
                var targetProp = props.FirstOrDefault(p => string.Compare(p.Name, prop.Name, StringComparison.Ordinal) == 0);

                if (targetProp != null
                    && targetProp.CanWrite
                    && targetProp.CustomAttributes.Count(a => a.AttributeType == typeof(IgnoreAttribute)) == 0
                    && !targetProp.PropertyType.DescendsFrom(typeof(CommandParameter)))
                {
                    var value = prop.GetValue(request);

                    targetProp.SetValue(result, value);
                }
            });

            return result;
        }
        public virtual ServiceResponse Run(object request)
        {
            var res = new TResponse();

            if (request == null)
            {
                res.SetStatus("NoRequest");
            }
            else
            {
                var req = request as TRequest;

                if (req != null)
                {
                    res = Run(req);
                }
                else
                {
                    var jobj = request as JObject;

                    if (jobj != null)
                    {
                        req = jobj.ToObject<TRequest>();
                        res = Run(req);
                    }
                    else
                    {
                        req = MapRequest(request);
                        res = Run(req);
                    }
                }
            }

            return res;
        }
        public virtual async Task<ServiceResponse> RunAsync(object request, CancellationToken token)
        {
            var res = new TResponse();

            if (request == null)
            {
                res.SetStatus("NoRequest");
            }
            else
            {
                var req = request as TRequest;

                if (req != null)
                {
                    res = Run(req);
                }
                else
                {
                    var jobj = request as JObject;

                    if (jobj != null)
                    {
                        req = jobj.ToObject<TRequest>();
                        res = await RunAsync(req);
                    }
                    else
                    {
                        req = MapRequest(request);
                        res = await RunAsync(req);
                    }
                }
            }

            return res;
        }
        public Task<TResponse> RunAsync(TRequest request)
        {
            return RunAsync(request, CancellationToken.None);
        }
        public virtual async Task<TResponse> RunAsync(TRequest request, CancellationToken token)
        {
            var response = new TResponse();

            try
            {
                if (OnBeforeRun(request, response))
                {
                    await RunInternalAsync(request, response, token);

                    OnAfterRun(request, response);
                }
            }
            catch (Exception e)
            {
                response.Flawed(e);

                OnError(request, response, e);
            }

            return response;
        }
    }
    public abstract class ServiceAction<TService, TConfig, TRequest, TResponse>: ServiceAction<TService, TRequest, TResponse>
        where TConfig : class, IServiceConfig, new()
        where TService : class, IService<TConfig>
        where TRequest : ServiceRequest
        where TResponse : ServiceResponse, new()
    {
        public ServiceAction(TService owner): base(owner)
        {

        }
    }
}
