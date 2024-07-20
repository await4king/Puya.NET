using Puya.Service;
using Puya.Settings.Service.Db;
using Puya.Settings.Service.Models;
using System;
using System.Collections.Generic;

namespace Puya.Settings.Service.Db
{
	public partial class TapDbSettingsUpdateByKeyRequest : ServiceRequest
    {
		public TapDbSettingsUpdateByKeyRequest()
		{
			Result = Puya.Data.CommandParameter.Output(System.Data.SqlDbType.VarChar, "SqlDbType", 50);
			Message = Puya.Data.CommandParameter.Output(System.Data.SqlDbType.NVarChar, "SqlDbType", 300);
		}
	}
}
