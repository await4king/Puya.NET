using Puya.Service;
using Puya.ServiceModel;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Puya.Data;
using Puya.Logging;
using Puya.Caching;
using Puya.Settings;

namespace Puya.Settings.Service.Db
{
	public abstract partial class TapDbSettingsBase : TapBaseActionBasedService<TapDbSettingsBaseConfig>, ITapDbSettings
    {
        public abstract TapDbSettingsAddBaseAction Add { get; protected set; }
        public abstract TapDbSettingsUpdateByPKBaseAction UpdateByPK { get; protected set; }
        public abstract TapDbSettingsUpdateByKeyBaseAction UpdateByKey { get; protected set; }
        public abstract TapDbSettingsDeleteByPKBaseAction DeleteByPK { get; protected set; }
        public abstract TapDbSettingsClearAllBaseAction ClearAll { get; protected set; }
        public abstract TapDbSettingsDeleteByKeyBaseAction DeleteByKey { get; protected set; }
        public abstract TapDbSettingsGetPageBaseAction GetPage { get; protected set; }
        public abstract TapDbSettingsGetByPKBaseAction GetByPK { get; protected set; }
		public TapDbSettingsBase(TapDbSettingsBaseConfig config, ILogger logger, IDb dbContext, ICacheManager cache, ISettingService settings) : base(config, logger, dbContext, cache, settings)
		{
			Init(config, logger, dbContext, cache, settings);
        }
		partial void Init(TapDbSettingsBaseConfig config, ILogger logger, IDb dbContext, ICacheManager cache, ISettingService settings);
    }
}

