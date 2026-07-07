using Puya.Caching;
using Puya.Data;
using Puya.Debugging;
using Puya.Logging;
using Puya.Service;
using Puya.ServiceModel;
using Puya.Translation;

namespace Puya.Settings.Service.Db
{
	public abstract partial class TapDbSettingsBase : TapBaseActionBasedService, ITapDbSettings
    {
        public abstract TapDbSettingsAddBaseAction Add { get; protected set; }
        public abstract TapDbSettingsUpdateByPKBaseAction UpdateByPK { get; protected set; }
        public abstract TapDbSettingsUpdateByKeyBaseAction UpdateByKey { get; protected set; }
        public abstract TapDbSettingsDeleteByPKBaseAction DeleteByPK { get; protected set; }
        public abstract TapDbSettingsClearAllBaseAction ClearAll { get; protected set; }
        public abstract TapDbSettingsDeleteByKeyBaseAction DeleteByKey { get; protected set; }
        public abstract TapDbSettingsGetPageBaseAction GetPage { get; protected set; }
        public abstract TapDbSettingsGetByPKBaseAction GetByPK { get; protected set; }
		public TapDbSettingsBase
            (
                ILogger logger,
                IDb dbContext,
                ICacheManager cache,
                ISettingService settings,
                ITranslator translator,
                IServiceInterceptor interceptor,
                ILogProvider logProvider,
                IDebugger debugger
            ) : base(logger, dbContext, cache, settings, translator, interceptor, logProvider, debugger)
		{
			Init(logger, dbContext, cache, settings, translator, interceptor, logProvider, debugger);
        }
		partial void Init
            (
                ILogger logger,
                IDb dbContext,
                ICacheManager cache,
                ISettingService settings,
                ITranslator translator,
                IServiceInterceptor interceptor,
                ILogProvider logProvider,
                IDebugger debugger
            );
    }
}

