using Puya.Service;
using Puya.Data;

namespace Puya.Logging.Services.LogManager
{
	public partial class TapLogManagerRegistration : ServiceRegistery
    {
        public TapLogManagerRegistration()
        {
			Add(typeof(TapLogManagerSqlConfig), typeof(TapLogManagerSqlConfig));
			Add(typeof(TapLogManagerBaseConfig), typeof(TapLogManagerBaseConfig));
			Add(typeof(TapLogManagerBase), typeof(TapLogManagerSql));
			Add(typeof(ITapLogManager), typeof(TapLogManagerSql));
			Add(typeof(TapLogManagerSql), typeof(TapLogManagerSql));

            Add(typeof(TapLogManagerClearBaseAction), typeof(TapLogManagerSqlClearAction));
            Add(typeof(TapLogManagerGetByPKBaseAction), typeof(TapLogManagerSqlGetByPKAction));
            Add(typeof(TapLogManagerGetPageBaseAction), typeof(TapLogManagerSqlGetPageAction));
            Add(typeof(TapLogManagerDeleteByPKBaseAction), typeof(TapLogManagerSqlDeleteByPKAction));
		}
	}
}