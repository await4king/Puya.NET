using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Extensions;

namespace Puya.ApiLogging
{
    public abstract class BaseApiLogger : IApiLogger
    {
        public BaseApiLogger(ApiClient client, ApiServer server)
        {
            Client = client;
            Server = server;
        }
        public BaseApiLogger()
        { }

        ApiClient client;
        public ApiClient Client
        {
            get
            {
                if (client == null)
                {
                    client = new ApiClient();
                }

                return client;
            }
            set { client = value; }
        }
        ApiServer server;
        public ApiServer Server
        {
            get
            {
                if (server == null)
                {
                    server = new ApiServer();
                }

                return server;
            }
            set { server = value; }
        }

        public abstract void Log(ApiLog log);

        public abstract Task LogAsync(ApiLog log, CancellationToken cancellation);
        protected string Serialize(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            try
            {
                return JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
            catch
            {
                try
                {
                    var sb = new StringBuilder();

                    foreach (var prop in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.CanRead))
                    {
                        sb.Append((sb.Length > 0 ? ", " : "") + $"{prop.Name}: {prop.GetValue(obj)?.ToString()}");
                    }

                    return $"Newtonsoft serialization failed; Manual serialization: {{{sb}}}";
                }
                catch
                {
                    return $"Serialization failed";
                }
            }
        }
    }
}
