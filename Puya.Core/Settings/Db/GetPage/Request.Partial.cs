using Puya.Service;
using System.Data;

namespace Puya.Settings.Service.Db
{
	public partial class TapDbSettingsGetPageRequest : ServiceRequest
    {
		public TapDbSettingsGetPageRequest()
		{
			RecordCount = Puya.Data.CommandParameter.Output(SqlDbType.Int, "SqlDbType");
			PageCount = Puya.Data.CommandParameter.Output(SqlDbType.Int, "SqlDbType");
		}
	}
}
