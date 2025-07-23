using Puya.Logging.Services.WebLogManager.GetPage;

namespace Puya.Logging.Web.Abstractions.Services.WebLogManager
{
	public partial class PuyaWebLogManagerSqlGetPageAction : PuyaWebLogManagerGetPageBaseAction
    {
        public PuyaWebLogManagerSqlGetPageAction(PuyaWebLogManagerSql owner) : base(owner)
        {
        }
	}
}
