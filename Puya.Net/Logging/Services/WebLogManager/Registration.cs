using Puya.Service;
using Puya.Data;
using Puya.Logging.Web.Abstractions.Services.WebLogManager;
using Puya.Logging.Services.WebLogManager.Clear;
using Puya.Logging.Services.WebLogManager.DeleteByPK;
using Puya.Logging.Services.WebLogManager.GetByPK;
using Puya.Logging.Services.WebLogManager.GetPage;

namespace Puya.Logging.Services.WebLogManager
{
	public partial class PuyaWebLogManagerRegistration : ServiceRegistery
    {
        public PuyaWebLogManagerRegistration()
        {
			Add(typeof(PuyaWebLogManagerSqlConfig), typeof(PuyaWebLogManagerSqlConfig));
			Add(typeof(PuyaWebLogManagerBaseConfig), typeof(PuyaWebLogManagerBaseConfig));
			Add(typeof(PuyaWebLogManagerBase), typeof(PuyaWebLogManagerSql));
			Add(typeof(IPuyaWebLogManager), typeof(PuyaWebLogManagerSql));
			Add(typeof(PuyaWebLogManagerSql), typeof(PuyaWebLogManagerSql));

            Add(typeof(PuyaWebLogManagerClearBaseAction), typeof(PuyaWebLogManagerSqlClearAction));
            Add(typeof(PuyaWebLogManagerGetByPKBaseAction), typeof(PuyaWebLogManagerSqlGetByPKAction));
            Add(typeof(PuyaWebLogManagerGetPageBaseAction), typeof(PuyaWebLogManagerSqlGetPageAction));
            Add(typeof(PuyaWebLogManagerDeleteByPKBaseAction), typeof(PuyaWebLogManagerSqlDeleteByPKAction));
		}
	}
}