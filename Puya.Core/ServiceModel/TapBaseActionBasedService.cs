using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Puya.Caching;
using Puya.Data;
using Puya.Debugging;
using Puya.Logging;
using Puya.Service;
using Puya.Settings;
using Puya.Translation;

namespace Puya.ServiceModel
{
    public abstract class TapBaseActionBasedService<TConfig> : BaseActionBasedService<TConfig>, IBaseService
        where TConfig : TapBaseConfig, new()
    {
        #region Properties
        public ILogProvider LogProvider { get; set; }
        IDebugger debugger;
        public IDebugger Debugger
        {
            get { return debugger ?? new NoDebugger(); }
            set { debugger = value; }
        }
        private string name;
        public override string Name
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                {
                    var type = this.GetType();

                    while (type != null)
                    {
                        var attr = type.GetCustomAttribute<ServiceNameAttribute>();

                        if (attr != null)
                        {
                            name = attr.Name;
                            break;
                        }

                        if (type.BaseType == typeof(TapBaseActionBasedService<TConfig>))
                        {
                            name = type.Name;
                            break;
                        }

                        type = type.BaseType;
                    }

                    if (string.IsNullOrEmpty(name))
                    {
                        name = this.GetType().Name;
                    }
                }

                return name;
            }
        }
        private ILogger _logger;
        public ILogger Logger
        {
            get
            {
                if (_logger == null)
                    _logger = new NullLogger();

                return _logger;
            }
            set { _logger = value; }
        }
        private IDb _db;
        public IDb Db
        {
            get
            {
                if (_db == null)
                    _db = new NullDb();

                return _db;
            }
            set { _db = value; }
        }
        private ICacheManager _cache;
        public ICacheManager Cache
        {
            get
            {
                if (_cache == null)
                    _cache = new NullCacheManager();

                return _cache;
            }
            set { _cache = value; }
        }
        private ISettingService _settings;
        public ISettingService Settings
        {
            get
            {
                if (_settings == null)
                    _settings = new InMemorySettingService();

                return _settings;
            }
            set { _settings = value; }
        }
        private ITranslator _translator;
        public ITranslator Translator
        {
            get
            {
                if (_translator == null)
                    _translator = new NullTranslator();

                return _translator;
            }
            set { _translator = value; }
        }
        #endregion
        #region Constructors
        public TapBaseActionBasedService() : this(null, null, null, null, null)
        { }
        public TapBaseActionBasedService(TConfig config) : this(config, null, null, null, null)
        { }
        public TapBaseActionBasedService(TConfig config, ILogger logger) : this(config, logger, null, null, null)
        { }
        public TapBaseActionBasedService(TConfig config, ILogger logger, IDb db) : this(config, logger, db, null, null)
        { }
        public TapBaseActionBasedService(TConfig config, ILogger logger, IDb db, ICacheManager cache) : this(config, logger, db, cache, null)
        { }
        public TapBaseActionBasedService(TConfig config, ILogger logger, IDb db, ICacheManager cache, ISettingService settings) : this(config, logger, db, cache, settings, null)
        { }
        public TapBaseActionBasedService(TConfig config, ILogger logger, IDb db, ICacheManager cache, ISettingService settings, ITranslator translator) : this(config, logger, db, cache, settings, translator, null)
        { }
        public TapBaseActionBasedService(TConfig config, ILogger logger, IDb db, ICacheManager cache, ISettingService settings, ITranslator translator, ILogProvider logProvider) : this(config, logger, db, cache, settings, translator, logProvider, null)
        { }
        public TapBaseActionBasedService(TConfig config, ILogger logger, IDb db, ICacheManager cache, ISettingService settings, ITranslator translator, ILogProvider logProvider, IDebugger debugger) : base(config)
        {
            Config.Logger = Logger = logger ?? Config.Logger;
            Config.Db = Db = db ?? Config.Db;
            Config.Cache = Cache = cache ?? Config.Cache;
            Config.Settings = Settings = settings ?? Config.Settings;
            Config.Translator = Translator = translator ?? Config.Translator;

            LogProvider = logProvider;
            Debugger = debugger;
        }
        #endregion
    }
    public abstract class TapBaseServiceAction<TBaseService, TConfig, TRequest, TResponse> : ServiceAction<TBaseService, TConfig, TRequest, TResponse>
        where TConfig : TapBaseConfig, new()
        where TBaseService : TapBaseActionBasedService<TConfig>, IService<TConfig>
        where TRequest : ServiceRequest
        where TResponse : ServiceResponse, new()
    {
        #region Props
        private string name;
        public override string Name
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                {
                    var type = this.GetType();

                    while (type != null)
                    {
                        var attr = type.GetCustomAttribute<ActionNameAttribute>();

                        if (attr != null)
                        {
                            name = attr.Name;
                            break;
                        }

                        if (type.BaseType == typeof(TapBaseServiceAction<TBaseService, TConfig, TRequest, TResponse>))
                        {
                            name = type.Name;
                            break;
                        }

                        type = type.BaseType;
                    }

                    if (string.IsNullOrEmpty(name))
                    {
                        name = this.GetType().Name.Replace(Owner.Name, "");
                    }
                }

                return name;
            }
        }
        private ILogger _logger;
        public ILogger Logger
        {
            get
            {
                if (_logger == null)
                    _logger = new NullLogger();

                return _logger;
            }
            set { _logger = value; }
        }
        private IDb _db;
        public IDb Db
        {
            get
            {
                if (_db == null)
                    _db = new NullDb();

                return _db;
            }
            set { _db = value; }
        }
        private ICacheManager _cache;
        public ICacheManager Cache
        {
            get
            {
                if (_cache == null)
                    _cache = new NullCacheManager();

                return _cache;
            }
            set { _cache = value; }
        }
        private ISettingService _settings;
        public ISettingService Settings
        {
            get
            {
                if (_settings == null)
                    _settings = new InMemorySettingService();

                return _settings;
            }
            set { _settings = value; }
        }
        private ITranslator _translator;
        public ITranslator Translator
        {
            get
            {
                if (_translator == null)
                    _translator = new NullTranslator();

                return _translator;
            }
            set { _translator = value; }
        }
        #endregion
        public TapBaseServiceAction(TBaseService owner) : base(owner)
        {
            Logger = owner.Logger;
            Db = owner.Db;
            Cache = owner.Cache;
            Settings = owner.Settings;
            Translator = owner.Translator;
        }
        public string ActionName => $"Action {Owner.Name}.{Name}";
        #region Logging
        protected override void OnError(TRequest request, TResponse response, Exception e)
        {
            Owner.Error("Action execution failed", e, new { State = "OnEror" });

            Owner.Logger.Danger(e, ActionName, new { State = "OnEror" });
        }
        public virtual string GetMessageKey(TResponse response)
        {
            return string.Empty;
        }
        protected override bool OnBeforeRun(TRequest request, TResponse response)
        {
            Owner.LogProvider?.EnterScope();

            if (string.IsNullOrEmpty(response.MessageKey))
            {
                response.MessageKey = GetMessageKey(response);

                if (string.IsNullOrEmpty(response.MessageKey))
                {
                    response.MessageKey = Owner.Name + "-" + Name;
                }
            }

            Owner.Debug($"{Owner.Name}.{Name}: Request", request);

            return true;
        }
        protected override void OnAfterRun(TRequest request, TResponse response)
        {
            // there is no need to log response. because response will be sent back to client.
            // Owner.Debug($"{Owner.Name}.{Name}: Response", response);

            try
            {
                Translator.Translate(response);

                if (!string.IsNullOrEmpty(response.Message))
                {
#if !DEBUG
                    // if we have a Message, we clear MessageKey (we don't need it).
                    // if we don't have a Message, we let MessageKey be sent back to client
                    // so that we can later check why Message translation did not produce anything.

                    response.MessageKey = "";
#endif
                }
            }
            catch (Exception e)
            {
                Owner.Error("translating response failed", e);
            }

            Owner.LogProvider?.ExitScope();
        }
        #endregion
    }
}
