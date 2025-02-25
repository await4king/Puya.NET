using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Puya.Caching;
using Puya.Collections;
using Puya.Data;
using Puya.Extensions;
using Puya.Logging;
using Puya.Service;
using Puya.Settings;
using Puya.Debugging;

namespace Puya.Api
{

    public class JsonApiManager : ApiManagerBase
    {
        public string Path { get; set; }
        public string ApiFileName { get; set; }
        public string AppFileName { get; set; }
        public JsonApiManager(IDb db, ILogger logger, ICacheManager cache, ISettingService settings, ILogProvider logProvider, IDebugger debugger) : base(db, logger, cache, settings, logProvider, debugger)
        { }
        private string GetPath(string filename = "")
        {
            if (string.IsNullOrEmpty(Path))
            {
                Path = Environment.CurrentDirectory;
            }

            var result = Path;

            result = result.EndsWith("\\debug", StringComparison.CurrentCultureIgnoreCase) ? result.Substring(0, result.Length - 6) : result;
            result = result.EndsWith("\\release", StringComparison.CurrentCultureIgnoreCase) ? result.Substring(0, result.Length - 8) : result;
            result = result.EndsWith("\\bin", StringComparison.CurrentCultureIgnoreCase) ? result.Substring(0, result.Length - 4) : result;

            if (!System.IO.Path.IsPathRooted(result))
            {
                result = System.IO.Path.Combine(Environment.CurrentDirectory, result);
            }

            if (!string.IsNullOrEmpty(result))
            {
                result = System.IO.Path.Combine(result, filename);
            }

            return result;
        }
        private List<T> Read<T>(string fileName, string defaultFileName)
            where T: ISettingable
        {
            var result = null as List<T>;

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = defaultFileName;
            }

            var path = GetPath(fileName);
            
            if (File.Exists(path))
            {
                var content = File.ReadAllText(path);

                try
                {
                    var config = JsonConvert.DeserializeObject<JsonConfig<T>>(content);

                    result = config.Data;

                    if (result != null && result.Count > 0)
                    {
                        foreach (var item in result)
                        {
                            item.Settings.MergeWith(config.Settings);
                        }
                    }
                }
                catch
                { }
            }

            return result;
        }
        protected override List<Api> GetApisInternal()
        {
            var result = Read<Api>(ApiFileName, "apis.json");

            return result;
        }
        protected override List<Application> GetAppsInternal()
        {
            var result = Read<Application>(ApiFileName, "apps.json");

            return result;
        }
        string GetOption(IDictionary<string, object> options, string key, string defaultValue)
        {
            var value = options[key]?.ToString();

            if (string.IsNullOrEmpty(value))
            {
                value = defaultValue;
            }

            return value;
        }
        public override ApiManagerCreateStoreResponse CreateStore(ApiManagerCreateStoreRequest request)
        {
            return Run<ApiManagerCreateStoreRequest, ApiManagerCreateStoreResponse>("CreateStore", (req, res) =>
            {
                var asm = Assembly.GetExecutingAssembly();
                var content = "";
                var path = "";
                var root = GetOption(req.Options, "OutputDir", GetPath());
                var apiFileName = GetOption(req.Options, "ApiFileName", "apis.json");
                var appFileName = GetOption(req.Options, "AppFileName", "apps.json");
                
                content = asm.GetResourceString("Puya.Api", $"Scripts.Json.apis.json");
                path = System.IO.Path.Combine(root, apiFileName);
                
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, content);
                }
                
                content = asm.GetResourceString("Puya.Api", $"Scripts.Json.apps.json");
                path = System.IO.Path.Combine(root, appFileName);

                if (!File.Exists(path))
                {
                    File.WriteAllText(path, content);
                }

                res.Succeeded();
            }, request);
        }

        public override Task<ApiManagerCreateStoreResponse> CreateStoreAsync(ApiManagerCreateStoreRequest request, CancellationToken cancellation)
        {
            return RunAsync<ApiManagerCreateStoreRequest, ApiManagerCreateStoreResponse>("CreateStore", (req, res, token) =>
            {
                var _res = CreateStore(req);

                res.Copy(_res);

                return Task.CompletedTask;
            }, request, cancellation);
        }
    }
}
