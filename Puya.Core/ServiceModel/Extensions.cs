using System;
using System.Runtime.CompilerServices;
using Puya.Service;
using Puya.Translation;

namespace Puya.ServiceModel
{
    public static class Extensions
    {
        public static void Translate(this ITranslator translator, ServiceResponse response, string lang = "", bool recursive = true, string defaultMessageKey = "")
        {
            if (translator != null && response != null && string.IsNullOrEmpty(response.Message) && !string.IsNullOrEmpty(response.MessageKey))
            {
                if (!string.IsNullOrEmpty(response.MessageKeyParam))
                {
                    response.Message = translator.GetSingle(response.MessageKey + "-" + response.MessageKeyParam, response.Status, lang);
                    var param = "";

                    foreach (var keyParam in response.MessageKeyParam.Split('-'))
                    {
                        if (!string.IsNullOrEmpty(response.Message))
                        {
                            break;
                        }

                        param = (string.IsNullOrEmpty(param) ? "": (param + "-")) + keyParam;

                        response.Message = translator.GetSingle(response.MessageKey + "-" + param, response.Status, lang);
                    }
                }

                if (string.IsNullOrEmpty(response.Message))
                {
                    response.Message = translator.GetSingle(response.MessageKey, response.Status, lang);
                }

                if (!string.IsNullOrEmpty(response.Message))
                {
                    if (response.Message[0] == '{' && response.Message[response.Message.Length - 1] == '}') // we can refer to another message using {target-key} syntax like
                                                                                                            // ('BrowseAny', '/BrowseAny-Revoke/NotFound/Fa', N'{/BrowseAny/NotFound/Fa}'),
                    {
                        response.Message = translator.GetSingle(response.Message);
                    }

                    if (response.HasMessageArgs())
                    {
                        foreach (var arg in response.MessageArgs)
                        {
                            response.Message = response.Message.Replace($"{{{arg.Key}}}", arg.Value?.ToString());
                        }
                    }
                }

                if (recursive && response.HasInnerResponses())
                {
                    foreach (var res in response.InnerResponses)
                    {
                        if (string.IsNullOrEmpty(res.MessageKey))
                        {
                            if (string.IsNullOrEmpty(defaultMessageKey))
                            {
                                res.MessageKey = response.MessageKey;
                            }
                            else
                            {
                                res.MessageKey = defaultMessageKey;
                            }
                        }

                        translator.Translate(res, lang);
                    }
                }

                response.MessageArgs = null;
                response.MessageKey = null;
                response.MessageKeyParam = null;
            }
        }
        #region Logging
        public static void Log(this IBaseService service, string message,
                        object data = null,
                        LogSource source = LogSource.App,
                        [CallerMemberName] string memberName = "",
                        [CallerFilePath] string sourceFilePath = "",
                        [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (service.Debugger.IsDebugging)
            {
                service.LogProvider?.Info(service.Name, message, data, source, memberName, sourceFilePath, sourceLineNumber);
            }
        }
        public static void Debug(this IBaseService service, string message,
                        object data = null,
                        LogSource source = LogSource.App,
                        [CallerMemberName] string memberName = "",
                        [CallerFilePath] string sourceFilePath = "",
                        [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (service.Debugger.IsDebugging)
            {
                service.LogProvider?.Debug(service.Name, message, data, source, memberName, sourceFilePath, sourceLineNumber);
            }
        }
        public static void Warn(this IBaseService service, string message,
                        object data = null,
                        LogSource source = LogSource.App,
                        [CallerMemberName] string memberName = "",
                        [CallerFilePath] string sourceFilePath = "",
                        [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (service.Debugger.IsDebugging)
            {
                service.LogProvider?.Warn(service.Name, message, data, source, memberName, sourceFilePath, sourceLineNumber);
            }
        }
        public static void Message(this IBaseService service, string message,
                        object data = null,
                        LogSource source = LogSource.App,
                        [CallerMemberName] string memberName = "",
                        [CallerFilePath] string sourceFilePath = "",
                        [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (service.Debugger.IsDebugging)
            {
                service.LogProvider?.Message(service.Name, message, data, source, memberName, sourceFilePath, sourceLineNumber);
            }
        }
        public static void Trace(this IBaseService service, string message,
                        object data = null,
                        LogSource source = LogSource.App,
                        [CallerMemberName] string memberName = "",
                        [CallerFilePath] string sourceFilePath = "",
                        [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (service.Debugger.IsDebugging)
            {
                service.LogProvider?.Trace(service.Name, message, data, source, memberName, sourceFilePath, sourceLineNumber);
            }
        }
        public static void Error(this IBaseService service, string message,
                        Exception e,
                        object data = null,
                        LogSource source = LogSource.App,
                        [CallerMemberName] string memberName = "",
                        [CallerFilePath] string sourceFilePath = "",
                        [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (service.Debugger.IsDebugging)
            {
                service.LogProvider?.Error(service.Name, message, e, data, source, memberName, sourceFilePath, sourceLineNumber);
            }
        }
        public static void Error(this IBaseService service, Exception e,
                        object data = null,
                        LogSource source = LogSource.App,
                        [CallerMemberName] string memberName = "",
                        [CallerFilePath] string sourceFilePath = "",
                        [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (service.Debugger.IsDebugging)
            {
                service.LogProvider?.Error(service.Name, string.Empty, e, data, source, memberName, sourceFilePath, sourceLineNumber);
            }
        }
        #endregion
    }
}
