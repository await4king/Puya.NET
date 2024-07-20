using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Base;
using Puya.Collections;
using Puya.Extensions;
using Puya.Reflection;

namespace Puya.Service
{
    public static class Extensions
    {
        public static bool AddTapServices(this IServiceCollection services, params string[] assembliesToLoad)
        {
            var result = AssemblyLoader.Load(assembliesToLoad);

            var types = AppDomain.CurrentDomain.GetSubClasses<ServiceRegistery>(false).ToList();

            foreach (var type in types)
            {
                var sr = Activator.CreateInstance(type) as ServiceRegistery;

                foreach (var r in sr.Registery)
                {
                    switch (r.LifeTime)
                    {
                        case LifeTime.Transient:
                            services.AddTransient(r.Abstraction, r.Concretion);
                            break;
                        case LifeTime.Singleton:
                            services.AddSingleton(r.Abstraction, r.Concretion);
                            break;
                        case LifeTime.PerRequest:
                            services.AddScoped(r.Abstraction, r.Concretion);
                            break;
                    }
                }
            }

            return result;
        }
        public static TResponse Run<TService, TRequest, TResponse>(this ServiceAction<TService, TRequest, TResponse> action)
            where TService : class, IService
            where TRequest : ServiceRequest, new()
            where TResponse : ServiceResponse, new()
        {
            return action.Run(new TRequest());
        }
        private static TRequest CreateRequest<TRequest>(object request)
            where TRequest : ServiceRequest, new()
        {
            var result = new TRequest();

            if (request != null)
            {
                var sourceProps = ReflectionHelper.GetPublicInstanceReadableProperties(request.GetType());
                var targetProps = ReflectionHelper.GetPublicInstanceWritableProperties(result.GetType());

                // TODO
                // Check AltNames attribute

                foreach (var prop in sourceProps)
                {
                    var _prop = targetProps.FirstOrDefault(p => p.Name == prop.Name);

                    if (_prop != null)
                    {
                        _prop.SetValue(result, prop.GetValue(request));
                    }
                    else
                    {
                        // TODO: check AltNames
                    }
                }
            }

            return result;
        }
        //public static TResponse Run<TService, TRequest, TResponse>(this ServiceAction<TService, TRequest, TResponse> action, object request)
        //    where TService : class
        //    where TRequest : ServiceRequest, new()
        //    where TResponse : ServiceResponse, new()
        //{
        //    var req = CreateRequest<TRequest>(request);

        //    return action.Run(req);
        //}
        //public static Task<TResponse> RunAsync<TService, TRequest, TResponse>(this ServiceAction<TService, TRequest, TResponse> action)
        //    where TService : class
        //    where TRequest : ServiceRequest, new()
        //    where TResponse : ServiceResponse, new()
        //{
        //    return action.RunAsync(new TRequest(), CancellationToken.None);
        //}
        //public static Task<TResponse> RunAsync<TService, TRequest, TResponse>(this ServiceAction<TService, TRequest, TResponse> action, CancellationToken cancellation)
        //    where TService : class
        //    where TRequest : ServiceRequest, new()
        //    where TResponse : ServiceResponse, new()
        //{
        //    return action.RunAsync(new TRequest(), cancellation);
        //}
        //public static Task<TResponse> RunAsync<TService, TRequest, TResponse>(this ServiceAction<TService, TRequest, TResponse> action, object request)
        //    where TService : class
        //    where TRequest : ServiceRequest, new()
        //    where TResponse : ServiceResponse, new()
        //{
        //    return action.RunAsync(request, CancellationToken.None);
        //}
        //public static Task<TResponse> RunAsync<TService, TRequest, TResponse>(this ServiceAction<TService, TRequest, TResponse> action, object request, CancellationToken cancellation)
        //    where TService : class
        //    where TRequest : ServiceRequest, new()
        //    where TResponse : ServiceResponse, new()
        //{
        //    var req = CreateRequest<TRequest>(request);

        //    return action.RunAsync(req, cancellation);
        //}
        #region Is
        public static bool HasStatus(this ServiceResponse sr, string status)
        {
            return string.Equals(sr.Status, status, StringComparison.OrdinalIgnoreCase);
        }
        public static bool IsNotFound(this ServiceResponse sr)
        {
            return sr.HasStatus(ServiceConstants.ServiceResponse.NotFound);
        }
        public static bool IsFailed(this ServiceResponse sr)  // business error
        {
            return sr.HasStatus(ServiceConstants.ServiceResponse.Failed);
        }
        public static bool IsErrored(this ServiceResponse sr) // calling sproc or executing sql failed (invalid number of params, invalid args, missing sproc, error in sql, etc.)
        {
            return sr.HasStatus(ServiceConstants.ServiceResponse.Errored);
        }
        public static bool IsFaulted(this ServiceResponse sr)
        {
            return sr.HasStatus(ServiceConstants.ServiceResponse.Faulted);
        }
        public static bool IsFlawed(this ServiceResponse sr) // calling action failed
        {
            return sr.HasStatus(ServiceConstants.ServiceResponse.Flawed);
        }
        public static bool IsAlreadyExists(this ServiceResponse response)
        {
            return response.HasStatus(ServiceConstants.ServiceResponse.AlreadyExists);
        }
        public static bool IsAccessDenied(this ServiceResponse response)
        {
            return response.HasStatus(ServiceConstants.ServiceResponse.AccessDenied);
        }
        public static bool IsNotAuthenticated(this ServiceResponse response)
        {
            return response.HasStatus(ServiceConstants.ServiceResponse.NotAuthenticated);
        }
        public static bool IsNotAuthorized(this ServiceResponse response)
        {
            return response.HasStatus(ServiceConstants.ServiceResponse.NotAuthorized);
        }
        public static bool IsSucceeded(this ServiceResponse sr)
        {
            var result = sr.Success || ServiceConstants.ServiceResponse.SuccessKeys.Any(s => ((sr.Status?.IndexOf(s, StringComparison.OrdinalIgnoreCase)) ?? -1) >= 0);
            
            if (result && string.IsNullOrEmpty(sr.Status))
            {
                sr.Status = "Success";
            }
            if (result && !sr.Success)
            {
                sr.Success = true;
            }

            return result;
        }
        #endregion
        #region Status
        public static void Succeeded(this ServiceResponse sr)
        {
            sr.Success = true;
            sr.Status = ServiceConstants.ServiceResponse.Success;
        }
        public static void Succeeded<T>(this ServiceResponse<T> sr, T data)
        {
            sr.Data = data;
            sr.Succeeded();
        }
        public static void Failed(this ServiceResponse sr, Exception e = null)
        {
            sr.SetStatus(ServiceConstants.ServiceResponse.Failed, e);
        }
        public static void Faulted(this ServiceResponse sr, Exception e = null)
        {
            sr.SetStatus(ServiceConstants.ServiceResponse.Faulted, e);
        }
        public static void Flawed(this ServiceResponse sr, Exception e = null)
        {
            sr.SetStatus(ServiceConstants.ServiceResponse.Flawed, e);
        }
        public static void NotFound(this ServiceResponse sr)
        {
            sr.SetStatus(ServiceConstants.ServiceResponse.NotFound);
        }
        public static void Errored(this ServiceResponse sr, Exception e = null)
        {
            sr.SetStatus(ServiceConstants.ServiceResponse.Errored, e);
        }
        public static void Deleted(this ServiceResponse sr)
        {
            sr.SetStatus(ServiceConstants.ServiceResponse.Deleted);
        }
        public static void AlreadyExists(this ServiceResponse sr)
        {
            sr.SetStatus(ServiceConstants.ServiceResponse.AlreadyExists);
        }
        public static void AccessDenied(this ServiceResponse response)
        {
            response.SetStatus(ServiceConstants.ServiceResponse.AccessDenied);
        }
        public static void NotAuthenticated(this ServiceResponse response)
        {
            response.SetStatus(ServiceConstants.ServiceResponse.NotAuthenticated);
        }
        public static void NotAuthorized(this ServiceResponse response)
        {
            response.SetStatus(ServiceConstants.ServiceResponse.NotAuthorized);
        }
        #endregion
        #region Fluent
        public static ServiceResponse SetInfo(this ServiceResponse response, string info)
        {
            response.Info = info;

            return response;
        }
        public static ServiceResponse SetBag(this ServiceResponse response, object bag)
        {
            response.Bag = bag;

            return response;
        }
        public static ServiceResponse FillData(this ServiceResponse response, object data)
        {
            response.SetData(data);

            return response;
        }
        public static ServiceResponse SetMessage(this ServiceResponse response, string message)
        {
            response.Message = message;

            return response;
        }
        public static ServiceResponse SetMessageKey(this ServiceResponse response, string messageKey)
        {
            response.MessageKey = messageKey;

            return response;
        }
        public static ServiceResponse SetArgs(this ServiceResponse response, IDictionary<string, object> args)
        {
            response.MessageArgs = args;

            return response;
        }
        public static ServiceResponse AddArg(this ServiceResponse response, params KeyValuePair<string, object>[] args)
        {
            if (args?.Length > 0)
            {
                if (response.MessageArgs == null)
                {
                    response.MessageArgs = new DynamicModel();
                }

                foreach (var arg in args)
                {
                    response.MessageArgs.Add(arg);
                }
            }

            return response;
        }
        public static ServiceResponse AddArg(this ServiceResponse response, string key, object value)
        {
            if (response.MessageArgs == null)
            {
                response.MessageArgs = new DynamicModel();
            }

            response.MessageArgs.Add(key, value);

            return response;
        }
        public static ServiceResponse Add(this ServiceResponse response, ServiceResponse innerResponse)
        {
            if (response.InnerResponses == null)
            {
                response.InnerResponses = new List<ServiceResponse>();
            }

            response.InnerResponses.Add(innerResponse);

            return response;
        }
        #endregion
    }
}
