using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Puya.Base;
using Puya.Caching;
using Puya.Collections;
using Puya.Conversion;
using Puya.Data;
using Puya.Extensions;
using Puya.Logging;
using Puya.Service;
using Puya.Settings;

namespace Puya.Api
{
    public class SqlServerApiManager : ApiManagerBase
    {
        public SqlServerApiManager(IDb db, ILogger logger, ICacheManager cache, ISettingService settings) : base(db, logger, cache, settings)
        { }
        private string apiTableName;
        public string ApiTableName
        {
            get
            {
                if (string.IsNullOrEmpty(apiTableName))
                {
                    apiTableName = "Apis";
                }

                return apiTableName;
            }
            set { apiTableName = value; }
        }
        private string appTableName;
        public string AppTableName
        {
            get
            {
                if (string.IsNullOrEmpty(appTableName))
                {
                    appTableName = "Apps";
                }

                return appTableName;
            }
            set { appTableName = value; }
        }
        private string appApiTableName;
        public string AppApiTableName
        {
            get
            {
                if (string.IsNullOrEmpty(appApiTableName))
                {
                    appApiTableName = "AppApis";
                }

                return appApiTableName;
            }
            set { appApiTableName = value; }
        }
        protected IList<T> GetData<T>(string table)
            where T: class, ISettingable
        {
            return Db.ExecuteReaderSql<T>($"select * from {table}", reader =>
            {
                var result = ObjectActivator.Instance.Activate<T>() as ISettingable;
                object r = result;

                reader.ConvertTo(ref r, typeof(T));

                var settings = SafeClrConvert.ToString(reader["Settings"]);

                if (!string.IsNullOrEmpty(settings))
                {
                    var x = JsonConvert.DeserializeObject<Dictionary<string, string>>(settings);

                    result.Settings.MergeWith(x);
                }

                return (T)result;
            });
        }
        protected override List<Api> GetApisInternal()
        {
            var result = new List<Api>();
            var data = Db.ExecuteReaderSql<ApiModel>($"select a.*, stuff((select ',' + cast(AppId as varchar(10)) from AppApis aa where aa.ApiId = a.Id for xml path('')), 1, 1, N'') as Apps from {ApiTableName} a");

            if (data != null)
            {
                foreach (var model in data)
                {
                    var api = new Api
                    {
                        Id = model.Id,
                        Version = model.Version,
                        Path = model.Path,
                        Service = model.Service,
                        Action = model.Action,
                        Disabled = model.Disabled,
                        Async = model.Async
                    };

                    if (!string.IsNullOrEmpty(model.Settings))
                    {
                        try
                        {
                            var x = JsonConvert.DeserializeObject<Dictionary<string, string>>(model.Settings);

                            api.Settings.MergeWith(x);
                        }
                        catch { }
                    }
                    if (!string.IsNullOrEmpty(model.Apps))
                    {
                        api.Apps = model.Apps.Split(new char[] { ',' }).Select(Int32.Parse).ToList();
                    }

                    result.Add(api);
                }
            }

            return result;
        }
        protected override List<Application> GetAppsInternal()
        {
            var result = new List<Application>();
            var data = Db.ExecuteReaderSql<ApplicationModel>($"select * from {AppTableName}");

            if (data != null)
            {
                foreach (var model in data)
                {
                    var app = new Application
                    {
                        Id = model.Id,
                        Name = model.Name,
                        NameEn = model.NameEn,
                        Description = model.Description,
                        DescriptionEn = model.DescriptionEn,
                        Version = model.Version,
                        Code = model.Code,
                        BasePath = model.BasePath,
                        Disabled = model.Disabled
                    };

                    if (!string.IsNullOrEmpty(model.Settings))
                    {
                        try
                        {
                            var x = JsonConvert.DeserializeObject<Dictionary<string, string>>(model.Settings);

                            app.Settings.MergeWith(x);
                        }
                        catch { }
                    }

                    result.Add(app);
                }
            }

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
                var query = "";
                
                query = asm.GetResourceString("Puya.Api", $"Scripts.SqlServer.Apis.xsql");

                var apiTableName = GetOption(req.Options, "ApiTableName", ApiTableName);
                var apiSchema = GetOption(req.Options, "ApiSchema", GetOption(req.Options, "Schema", "dbo"));
                var apiFileGroup = GetOption(req.Options, "ApiFileGroup", GetOption(req.Options, "FileGroup", "PRIMARY"));

                query = query
                            .Replace("@Model.TableName", apiTableName)
                            .Replace("@Model.Schema", apiSchema)
                            .Replace("@Model.FileGroup", apiFileGroup);

                Db.ExecuteNonQuerySql(query);

                query = asm.GetResourceString("Puya.Api", $"Scripts.SqlServer.Apps.xsql");

                var appTableName = GetOption(req.Options, "AppTableName", AppTableName);
                var appSchema = GetOption(req.Options, "AppSchema", GetOption(req.Options, "Schema", "dbo"));
                var appFileGroup = GetOption(req.Options, "AppFileGroup", GetOption(req.Options, "FileGroup", "PRIMARY"));

                query = query
                            .Replace("@Model.TableName", appTableName)
                            .Replace("@Model.Schema", appSchema)
                            .Replace("@Model.FileGroup", appFileGroup);

                Db.ExecuteNonQuerySql(query);

                query = asm.GetResourceString("Puya.Api", $"Scripts.SqlServer.AppApis.xsql");

                var appApiTableName = GetOption(req.Options, "AppApiTableName", AppApiTableName);
                var appApiSchema = GetOption(req.Options, "AppApiSchema", GetOption(req.Options, "Schema", "dbo"));
                var appApiFileGroup = GetOption(req.Options, "AppApiFileGroup", GetOption(req.Options, "FileGroup", "PRIMARY"));

                query = query
                            .Replace("@Model.TableName", appApiTableName)
                            .Replace("@Model.AppTableName", appTableName)
                            .Replace("@Model.ApiTableName", apiTableName)
                            .Replace("@Model.Schema", appApiSchema)
                            .Replace("@Model.FileGroup", appApiFileGroup);

                Db.ExecuteNonQuerySql(query);

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
