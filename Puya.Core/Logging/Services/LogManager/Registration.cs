using Puya.Service;

namespace Puya.Logging.Services.LogManager
{
	public partial class PuyaLogManagerRegistration : ServiceRegistery
    {
        public PuyaLogManagerRegistration()
        {
			Add(typeof(PuyaLogManagerBase), typeof(PuyaLogManagerSql));
			Add(typeof(IPuyaLogManager), typeof(PuyaLogManagerSql));
			Add(typeof(PuyaLogManagerSql), typeof(PuyaLogManagerSql));

            Add(typeof(PuyaLogManagerClearBaseAction), typeof(PuyaLogManagerSqlClearAction));
            Add(typeof(PuyaLogManagerGetByPKBaseAction), typeof(PuyaLogManagerSqlGetByPKAction));
            Add(typeof(PuyaLogManagerGetPageBaseAction), typeof(PuyaLogManagerSqlGetPageAction));
            Add(typeof(PuyaLogManagerDeleteByPKBaseAction), typeof(PuyaLogManagerSqlDeleteByPKAction));
		}
	}
}