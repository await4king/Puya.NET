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
	public partial class TapDbSettingsDefault : TapDbSettingsBase
    {
        public TapDbSettingsDefaultConfig StrongConfig
        {
            get { return Config as TapDbSettingsDefaultConfig; }
        }
        public override TapDbSettingsAddBaseAction Add { get; protected set; }
        public override TapDbSettingsUpdateByPKBaseAction UpdateByPK { get; protected set; }
        public override TapDbSettingsUpdateByKeyBaseAction UpdateByKey { get; protected set; }
        public override TapDbSettingsDeleteByPKBaseAction DeleteByPK { get; protected set; }
        public override TapDbSettingsClearAllBaseAction ClearAll { get; protected set; }
        public override TapDbSettingsDeleteByKeyBaseAction DeleteByKey { get; protected set; }
        public override TapDbSettingsGetPageBaseAction GetPage { get; protected set; }
        public override TapDbSettingsGetByPKBaseAction GetByPK { get; protected set; }
		public TapDbSettingsDefault(TapDbSettingsDefaultConfig config, ILogger logger, IDb dbContext, ICacheManager cache, ISettingService settings) : base(config, logger, dbContext, cache, settings)
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
			Init(config, logger, dbContext, cache, settings);
        }
		partial void Init(TapDbSettingsDefaultConfig config, ILogger logger, IDb dbContext, ICacheManager cache, ISettingService settings);
    }
}

