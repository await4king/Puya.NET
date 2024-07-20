using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Puya.Extensions;

namespace Puya.Sms
{
    public class DynamicSmsService : ISmsService
    {
        public DynamicSmsService(SmsConfig configs, ISmsLogger logger)
        {
            Config = configs;
            Logger = logger;

            var type = "";

            if (configs != null)
            {
                var config = Config.FirstOrDefault(cfg => cfg?.Has("Active", "true") ?? false);

                if (config == null)
                {
                    config = Config.FirstOrDefault();
                }

                config?.TryGetValue("Type", out type);
            }

            Type = type;
        }
        public DynamicSmsService(ISmsLogger logger) : this(null, logger)
        { }
        public DynamicSmsService(SmsConfig configs) : this(configs, null)
        { }
        public DynamicSmsService()
        { }
        string type;
        public string Type
        {
            get {  return type; }
            set
            {
                type = value;
                instance = GetInstance();
            }
        }
        ISmsService instance;
        public ISmsService Instance
        {
            get {  return instance; }
            set { instance = value; }
        }
        ISmsLogger _logger;
        public SmsConfig Config { get; set; }
        public virtual ISmsLogger Logger
        {
            get { return _logger; }
            set
            {
                _logger = value;

                var prop = instance?.GetType().GetProperty("Logger");

                if (prop != null)
                {
                    prop.SetValue(instance, _logger ?? new SmsLoggerNull());
                }
            }
        }
        public Dictionary<string, string> GetConfig(string type)
        {
            Dictionary<string, string> result = null;

            if (Config != null)
            {
                if (string.IsNullOrEmpty(type))
                {
                    result = Config.FirstOrDefault(cfg => cfg.Has("Active", "true")) ?? (Config.Count > 0 ? Config[0]: null);
                }
                else
                {
                    var candidates = Config.Where(cfg => cfg.Has("Type", type)).ToList();

                    result = candidates.FirstOrDefault(cfg => cfg.Has("Active", "true")) ?? (candidates.Count > 0 ? candidates[0] : null); ;
                }
            }
            
            return result;
        }
        protected void OnConfigError(Exception e, string prop)
        {
            if (string.IsNullOrEmpty(prop))
            {
                Logger?.Log(new SmsLog { Topic = "Error reading config", Error = e });
            }
            else
            {
                Logger?.Log(new SmsLog { Topic = $"Error reading config prop '{prop}'", Error = e });
            }
        }
        protected virtual ISmsService GetInstanceInternal() // provides a hook for subclasses to override intrinsic get instance mechanism
        {
            return null;
        }
        public virtual ISmsService GetInstance()
        {
            var result = GetInstanceInternal();

            if (result == null)
            {
                var config = GetConfig(Type);

                switch (Type?.ToLower())
                {
                    case "console":
                        var consoleConfig = config.ToStrongConfig<ConsoleSmsServiceConfig>(OnConfigError);

                        result = new ConsoleSmsService(consoleConfig, Logger);

                        break;
                    case "debug":
                        var debugConfig = config.ToStrongConfig<DebugSmsServiceConfig>(OnConfigError);

                        result = new DebugSmsService(debugConfig, Logger);

                        break;
                    case "memory":
                        var memoryConfig = config.ToStrongConfig<MemorySmsServiceConfig>(OnConfigError);

                        result = new MemorySmsService(memoryConfig, Logger);

                        break;
                    case "file":
                        var fileConfig = config.ToStrongConfig<FileSmsServiceConfig>(OnConfigError);

                        result = new FileSmsService(fileConfig, Logger);

                        break;
                    case "sqlserver":
                        var sqlserverConfig = config.ToStrongConfig<SqlServerSmsServiceConfig>(OnConfigError);

                        result = new SqlServerSmsService(sqlserverConfig, Logger);

                        break;
                    case null:
                    case "":
                    case "null":
                        result = new NullSmsService();
                        break;
                    default:
                        var assemblyConfig = config.ToStrongConfig<AssemblySmsServiceConfig>(OnConfigError);

                        assemblyConfig.AssemblyName = Type;
                        assemblyConfig.InnerConfig = config;

                        var assemblyService = new AssemblySmsService(assemblyConfig, Logger);

                        if (assemblyService.GetService() != null)
                        {
                            result = assemblyService;
                        }

                        break;
                }
            }

            return result;
        }
        public Task<SendResponse> SendAsync(string mobile, string message, CancellationToken cancellation)
        {
            return Instance?.SendAsync(mobile, message, cancellation);
        }
        public SendResponse Send(string mobile, string message)
        {
            return Instance?.Send(mobile, message);
        }
    }
}
