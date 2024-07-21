using Puya.Service;
using Puya.ServiceModel;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Puya.Settings.Service.Models;

namespace Puya.Settings.Service.Db
{
	public abstract partial class TapDbSettingsGetByPKBaseAction:
        TapBaseServiceAction<TapDbSettingsBase, TapDbSettingsBaseConfig, TapDbSettingsGetByPKRequest, TapDbSettingsGetByPKResponse>
    {
        public TapDbSettingsGetByPKBaseAction(TapDbSettingsBase owner) : base(owner)
        {
        }
    }
}
