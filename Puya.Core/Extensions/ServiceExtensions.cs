using Puya.Base;
using Puya.Collections;
using Puya.Conversion;
using Puya.Data;
using Puya.Reflection;
using Puya.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Extensions
{
    public static class ServiceExtensions
    {
        public static async Task<ReportData<List<object>>> GetSchemaBasedList(this IDb db, string query, bool sproc, object args, bool async, CancellationToken cancellation)
        {
            var count = 0;
            var schema = new List<ReportDataSchemaItem>();
            var response = new ReportData<List<object>>();

            if (async)
            {
                if (sproc)
                {
                    response.Items = await db.ExecuteReaderCommandAsync(query, reader => MapReader(reader, schema, ref count), args, cancellation);
                }
                else
                {
                    response.Items = await db.ExecuteReaderSqlAsync(query, reader => MapReader(reader, schema, ref count), args, cancellation);
                }
            }
            else
            {
                if (sproc)
                {
                    response.Items = db.ExecuteReaderCommand(query, reader => MapReader(reader, schema, ref count), args);
                }
                else
                {
                    response.Items = db.ExecuteReaderSql(query, reader => MapReader(reader, schema, ref count), args);
                }
            }

            response.Schema = schema;

            return response;
        }
        public static List<object> MapReader(this IDataReader reader, List<ReportDataSchemaItem> schema, ref int count)
        {
            var items = new List<object>();

            if (count++ == 0)
            {
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    schema.Add(new ReportDataSchemaItem(reader, i));

                    if (reader.GetFieldType(i) == TypeHelper.TypeOfDateTime)
                    {
                        schema.Add(new ReportDataSchemaItem(reader.GetName(i) + "Fa", "varchar", "string", "String", 20));
                        schema.Add(new ReportDataSchemaItem(reader.GetName(i) + "Time", "varchar", "string", "String", 8));
                    }
                }
            }

            for (var i = 0; i < reader.FieldCount; i++)
            {
                var value = reader.GetValue(i);

                if (reader.IsDBNull(i))
                {
                    items.Add(null);
                }
                else
                {
                    items.Add(value);
                }

                if (reader.GetFieldType(i) == TypeHelper.TypeOfDateTime)
                {
                    if (reader.IsDBNull(i))
                    {
                        items.Add(null);
                        items.Add(null);
                    }
                    else
                    {
                        var d = (DateTime)value;
                        var pc = new PersianCalendar();

                        items.Add(d.ToPersian());
                        items.Add($"{pc.GetHour(d)}:{pc.GetMinute(d)}:{pc.GetSecond(d)}");
                    }
                }
            }

            return items;
        }
        public static void Finalize(this ServiceResponse response, object args)
        {
            if (response != null && args != null)
            {
                var props = ReflectionHelper.GetPublicInstanceReadableProperties(args.GetType());
                var resultProp = props.FirstOrDefault(p => p.Name.Equalz("Result"));
                var fieldProp = props.FirstOrDefault(p => p.Name.Equalz("Field"));
                var messageProp = props.FirstOrDefault(p => p.Name.Equalz("Message"));

                if (resultProp != null)
                {
                    var resultParam = resultProp.GetValue(args) as CommandParameter;

                    if (resultParam != null)
                    {
                        response.Finalize(resultParam);
                    }
                }
                else
                {
                    var statusProp = props.FirstOrDefault(p => p.Name.Equalz("Status"));

                    if (statusProp != null)
                    {
                        var statusParam = statusProp.GetValue(args) as CommandParameter;

                        if (statusParam != null)
                        {
                            response.Finalize(statusParam);
                        }
                    }
                }

                if (fieldProp != null)
                {
                    var fieldParam = fieldProp.GetValue(args) as CommandParameter;

                    if (fieldParam != null)
                    {
                        response.Info = SafeClrConvert.ToString(fieldParam.Value);
                    }
                }

                if (messageProp != null)
                {
                    var messageParam = messageProp.GetValue(args) as CommandParameter;

                    if (messageParam != null)
                    {
                        response.Message = SafeClrConvert.ToString(messageParam.Value);
                    }
                }
            }
        }
        public static void Finalize(this ServiceResponse response, CommandParameter resultParam)
        {
            if (response != null && resultParam != null)
            {
                var result = SafeClrConvert.ToString(resultParam.Value);

                if (result.IsJsonObject())
                {
                    var res = result.SafeDeserialize<ServiceResponse>();

                    if (res != null)
                    {
                        response.Copy(res);
                    }
                    else
                    {
                        response.SetStatus("ProblematicResult");
                        response.Info = result;
                    }
                }
                else
                {
                    response.SetStatus(result);

                    if (string.IsNullOrEmpty(response.Status))
                    {
                        response.Succeeded();
                    }
                }
            }
            else if (response != null)
            {
                response.SetStatus("NoResultParam");
            }
        }
        public static void GetPagination<T>(this PagingResult<T> pagination, IDictionary<string, object> args)
        {
            pagination.Page = args.GetInt("Page");
            pagination.PageSize = args.GetInt("PageSize");
            pagination.PageCount = args.GetInt("PageCount");
            pagination.RecordCount = args.GetInt("RecordCount");
        }
        public static TResponse Run<TService, TRequest, TResponse>(this ServiceAction<TService, TRequest, TResponse> action)
            where TService : class, IService
            where TRequest : class, ServiceRequest, new()
            where TResponse : ServiceResponse, new()
        {
            return action.Run(new TRequest());
        }
        private static TRequest CreateRequest<TRequest>(object request)
            where TRequest : class, ServiceRequest, new()
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
        //    where TRequest : class, ServiceRequest, new()
        //    where TResponse : ServiceResponse, new()
        //{
        //    var req = CreateRequest<TRequest>(request);

        //    return action.Run(req);
        //}
        //public static Task<TResponse> RunAsync<TService, TRequest, TResponse>(this ServiceAction<TService, TRequest, TResponse> action)
        //    where TService : class
        //    where TRequest : class, ServiceRequest, new()
        //    where TResponse : ServiceResponse, new()
        //{
        //    return action.RunAsync(new TRequest(), CancellationToken.None);
        //}
        //public static Task<TResponse> RunAsync<TService, TRequest, TResponse>(this ServiceAction<TService, TRequest, TResponse> action, CancellationToken cancellation)
        //    where TService : class
        //    where TRequest : class, ServiceRequest, new()
        //    where TResponse : ServiceResponse, new()
        //{
        //    return action.RunAsync(new TRequest(), cancellation);
        //}
        //public static Task<TResponse> RunAsync<TService, TRequest, TResponse>(this ServiceAction<TService, TRequest, TResponse> action, object request)
        //    where TService : class
        //    where TRequest : class, ServiceRequest, new()
        //    where TResponse : ServiceResponse, new()
        //{
        //    return action.RunAsync(request, CancellationToken.None);
        //}
        //public static Task<TResponse> RunAsync<TService, TRequest, TResponse>(this ServiceAction<TService, TRequest, TResponse> action, object request, CancellationToken cancellation)
        //    where TService : class
        //    where TRequest : class, ServiceRequest, new()
        //    where TResponse : ServiceResponse, new()
        //{
        //    var req = CreateRequest<TRequest>(request);

        //    return action.RunAsync(req, cancellation);
        //}
        public static void Finalize(this ServiceResponse response)
        {
            if (response.Message.IsJson())
            {
                if (response.Message.IsJsonArray())
                {
                    var messages = response.Message.SafeDeserialize<List<ServiceResponse>>();

                    if (messages?.Count > 0)
                    {
                        foreach (var msg in messages)
                        {
                            response.InnerResponses.Add(msg);
                        }
                    }
                }
                else if (response.Message.IsJsonObject())
                {
                    var message = response.Message.SafeDeserialize<ServiceResponse>();

                    if (message != null)
                    {
                        if (!string.IsNullOrEmpty(message.MessageKey))
                        {
                            response.MessageKey = message.MessageKey;
                        }
                        if (!string.IsNullOrEmpty(message.MessageKeyParam))
                        {
                            response.MessageKeyParam = message.MessageKeyParam;
                        }
                        if (message.MessageArgs != null)
                        {
                            response.MessageArgs = message.MessageArgs;
                        }
                    }
                }

                response.Message = "";
            }
        }
        #region Is
        public static bool HasStatus(this ServiceResponse sr)
        {
            return !string.IsNullOrEmpty(sr.Status);
        }
        public static bool HasStatus(this ServiceResponse sr, string status)
        {
            return sr.Status.Equalz(status);
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
        public static ServiceResponse ChangeStatus(this ServiceResponse response, string status)
        {
            response.SetStatus(status);

            return response;
        }
        public static ServiceResponse SetException(this ServiceResponse response, Exception e)
        {
            response.Exception = e;

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
