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
	public partial interface ITapDbSettings :IService<TapDbSettingsBaseConfig>
	{
        TapDbSettingsAddBaseAction Add { get; }
        TapDbSettingsUpdateByPKBaseAction UpdateByPK { get; }
        TapDbSettingsUpdateByKeyBaseAction UpdateByKey { get; }
        TapDbSettingsDeleteByPKBaseAction DeleteByPK { get; }
        TapDbSettingsClearAllBaseAction ClearAll { get; }
        TapDbSettingsDeleteByKeyBaseAction DeleteByKey { get; }
        TapDbSettingsGetPageBaseAction GetPage { get; }
        TapDbSettingsGetByPKBaseAction GetByPK { get; }
    }
}
