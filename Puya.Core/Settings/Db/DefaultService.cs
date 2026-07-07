using Puya.Caching;
using Puya.Data;
using Puya.Debugging;
using Puya.Logging;
using Puya.Service;
using Puya.ServiceModel;
using Puya.Settings;
using Puya.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Settings.Service.Db
{
	public partial class TapDbSettingsDefault : TapDbSettingsBase
    {
        public override TapDbSettingsAddBaseAction Add { get; protected set; }
        public override TapDbSettingsUpdateByPKBaseAction UpdateByPK { get; protected set; }
        public override TapDbSettingsUpdateByKeyBaseAction UpdateByKey { get; protected set; }
        public override TapDbSettingsDeleteByPKBaseAction DeleteByPK { get; protected set; }
        public override TapDbSettingsClearAllBaseAction ClearAll { get; protected set; }
        public override TapDbSettingsDeleteByKeyBaseAction DeleteByKey { get; protected set; }
        public override TapDbSettingsGetPageBaseAction GetPage { get; protected set; }
        public override TapDbSettingsGetByPKBaseAction GetByPK { get; protected set; }
		public TapDbSettingsDefault
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
        	Add = new TapDbSettingsDefaultAddAction(this);
        	Actions.Add("Add", Add);
        	UpdateByPK = new TapDbSettingsDefaultUpdateByPKAction(this);
        	Actions.Add("UpdateByPK", UpdateByPK);
        	UpdateByKey = new TapDbSettingsDefaultUpdateByKeyAction(this);
        	Actions.Add("UpdateByKey", UpdateByKey);
        	DeleteByPK = new TapDbSettingsDefaultDeleteByPKAction(this);
        	Actions.Add("DeleteByPK", DeleteByPK);
        	ClearAll = new TapDbSettingsDefaultClearAllAction(this);
        	Actions.Add("ClearAll", ClearAll);
        	DeleteByKey = new TapDbSettingsDefaultDeleteByKeyAction(this);
        	Actions.Add("DeleteByKey", DeleteByKey);
        	GetPage = new TapDbSettingsDefaultGetPageAction(this);
        	Actions.Add("GetPage", GetPage);
        	GetByPK = new TapDbSettingsDefaultGetByPKAction(this);
        	Actions.Add("GetByPK", GetByPK);
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

