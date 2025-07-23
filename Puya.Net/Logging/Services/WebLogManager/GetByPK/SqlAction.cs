using Puya.Logging.Services.WebLogManager.GetByPK;

namespace Puya.Logging.Web.Abstractions.Services.WebLogManager
{
	public partial class PuyaWebLogManagerSqlGetByPKAction : PuyaWebLogManagerGetByPKBaseAction
    {
        public PuyaWebLogManagerSqlGetByPKAction(PuyaWebLogManagerSql owner) : base(owner)
        {
        }
	}
}
