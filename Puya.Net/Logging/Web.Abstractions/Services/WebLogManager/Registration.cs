using Puya.Service;
using Puya.Data;

namespace Puya.Logging.Web.Abstractions.Services.WebLogManager
{
	public partial class TapWebLogManagerRegistration : ServiceRegistery
    {
        public TapWebLogManagerRegistration()
        {
			Add(typeof(TapWebLogManagerSqlConfig), typeof(TapWebLogManagerSqlConfig));
			Add(typeof(TapWebLogManagerBaseConfig), typeof(TapWebLogManagerBaseConfig));
			Add(typeof(TapWebLogManagerBase), typeof(TapWebLogManagerSql));
			Add(typeof(ITapWebLogManager), typeof(TapWebLogManagerSql));
			Add(typeof(TapWebLogManagerSql), typeof(TapWebLogManagerSql));

            Add(typeof(TapWebLogManagerClearBaseAction), typeof(TapWebLogManagerSqlClearAction));
            Add(typeof(TapWebLogManagerGetByPKBaseAction), typeof(TapWebLogManagerSqlGetByPKAction));
            Add(typeof(TapWebLogManagerGetPageBaseAction), typeof(TapWebLogManagerSqlGetPageAction));
            Add(typeof(TapWebLogManagerDeleteByPKBaseAction), typeof(TapWebLogManagerSqlDeleteByPKAction));
		}
	}
}