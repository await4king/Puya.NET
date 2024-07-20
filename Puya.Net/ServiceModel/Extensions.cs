using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
