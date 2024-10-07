using Puya.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Web;
using System.Linq;

namespace Puya.Service
{
    public partial class ServiceResponse : IJsonSerializable
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Bag { get; set; }
        public DateTime Date { get; set; }
        public virtual bool Success { get; set; }
        public string MessageKeyParam { get; set; }
        public string Subject { get; set; }
        public string MessageKey { get; set; }
        public string Status { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Info { get; set; }
        private IDictionary<string, object> messageArgs;
        public IDictionary<string, object> MessageArgs
        {
            get
            {
                if (messageArgs == null)
                {
                    messageArgs = new Dictionary<string, object>();
                }

                return messageArgs;
            }
            set { messageArgs = value; }
        }
        public string Message { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Exception Exception { get; set; }
        private List<ServiceResponse> innerResponses;
        public List<ServiceResponse> InnerResponses
        {
            get
            {
                if (innerResponses == null)
                {
                    innerResponses = new List<ServiceResponse>();
                }

                return innerResponses;
            }
            set
            {
                innerResponses = value;
            }
        }
        public LogList Logs { get; set; }
        public bool ShouldSerializeInnerResponses()
        {
            return innerResponses?.Count > 0;
        }
        public bool ShouldSerializeMessageArgs()
        {
            return messageArgs?.Count > 0;
        }
        public virtual void SetStatus(object value, Exception e = null, string message = null)
        {
            Status = value?.ToString() ?? "";
            Success = ServiceConstants.ServiceResponse.SuccessKeys.Any(x => Status.IndexOf(x, StringComparison.OrdinalIgnoreCase) >= 0);
            Exception = e;
            Message = message;
        }
        public ServiceResponse()
        {
            Date = ServiceConstants.Now.Value;
        }
        public virtual void Copy(ServiceResponse response)
        {
            this.Bag = response.Bag;
            this.Date = response.Date;
            this.Subject = response.Subject;
            this.Success = response.Success;

            if (!string.IsNullOrEmpty(response.MessageKey) || string.IsNullOrEmpty(this.MessageKey))
            {
                this.MessageKey = response.MessageKey;
            }

            this.MessageKeyParam = response.MessageKeyParam;
            this.Status = response.Status;
            this.Info = response.Info;
            this.MessageArgs = response.messageArgs;
            this.Message = response.Message;
            this.Exception = response.Exception;
            this.InnerResponses = response.InnerResponses;
            this.Logs = response.Logs;

            var data = response.GetData();

            SetData(data);
        }
        public void AppendMessage(string message)
        {
            Message += "\n" + message;
        }
        public bool HasMessageArgs()
        {
            return messageArgs != null && messageArgs.Count > 0;
        }
        public bool HasInnerResponses()
        {
            return innerResponses != null && innerResponses.Count > 0;
        }
        public string ToJson(JsonSerializationOptions options = null)
        {
            var _innerResponses = "";

            if (HasInnerResponses())
            {
                var sb = new StringBuilder();

                foreach (var res in InnerResponses)
                {
                    if (sb.Length == 0)
                    {
                        sb.Append(",\"InnerResponses\": [" + res.ToJson());
                    }
                    else
                    {
                        sb.Append("," + res.ToJson());
                    }
                }

                sb.Append("]");

                _innerResponses = sb.ToString();
            }

            var exception = "";

            if (Exception != null)
            {
                exception = ",\"Exception\": " + JsonConvert.SerializeObject(Exception);
            }

            var info = "";

            if (Info != null)
            {
                info = ",\"Info\": " + JsonConvert.SerializeObject(Info);
            }

            var data = "";
            var prop = this.GetType().GetProperty("Data");

            if (prop != null)
            {
                var dataValue = prop.GetValue(this, null);
                if (dataValue != null)
                {
                    data = ",\"Data\": " + JsonConvert.SerializeObject(dataValue);
                }
            }

            var logs = "";

            if (Logs?.Count > 0)
            {
                logs = ",\"Logs\": " + JsonConvert.SerializeObject(Logs);
            }

            var result = $@"{{""Success"": {Success.ToString().ToLower()},""Status"": ""{Status}"", ""Date"":""{Date:yyyy/MM/dd HH:mm:ss.fffffff}"",""Message"": ""{HttpUtility.JavaScriptStringEncode(Message)}""{data}{info}{exception}{_innerResponses}{logs}}}";

            return result;
        }
        public string[] ExtractMessages()
        {
            var result = new List<string>();

            result.Add(this.Message);

            if (this.innerResponses != null && this.innerResponses.Count > 0)
            {
                foreach (var msg in this.InnerResponses)
                {
                    var _r = msg.ExtractMessages();

                    foreach (var m in _r)
                    {
                        result.Add(m);
                    }
                }
            }

            return result.ToArray();
        }
        public string FlattenMessage(string separator = "\n")
        {
            var messages = ExtractMessages();
            var sb = new StringBuilder();

            foreach (var msg in messages)
            {
                if (sb.Length == 0)
                {
                    sb.Append(msg);
                }
                else
                {
                    sb.Append(separator + msg);
                }
            }

            return sb.ToString();
        }
        public virtual object GetData()
        {
            return null;
        }
        public virtual void SetData(object data, bool throwErrors = false)
        { }
    }
    public class ServiceResponse<T> : ServiceResponse
    {
        private T data;
        private bool ignoreDefaultData { get; set; }
        public ServiceResponse()
        {
            ignoreDefaultData = true;
        }
        public T Data
        {
            get
            {
                var type = typeof(T);

                if (!ignoreDefaultData && type.IsClass && !type.IsAbstract && !type.IsArray && ((object)data) == null)
                {
                    CreateData();
                }

                return data;
            }
            set { data = value; }
        }
        public virtual void CreateData()
        {
            try
            {
                this.Data = (T)ServiceConstants.ObjectActivator.Activate(typeof(T));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        public void IgnoreDefaultData(bool value)
        {
            this.ignoreDefaultData = value;
        }
        public override object GetData()
        {
            return Data;
        }
        public override void SetData(object data, bool throwErrors = false)
        {
            try
            {
                var _data = (T)data;

                Data = _data;
            }
            catch (Exception)
            {
                if (throwErrors)
                {
                    throw;
                }
            }
        }
    }
}
