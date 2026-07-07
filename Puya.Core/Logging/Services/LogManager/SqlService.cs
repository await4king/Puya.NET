using Puya.Data;

namespace Puya.Logging.Services.LogManager
{
	public partial class PuyaLogManagerSql : PuyaLogManagerBase
    {
        public override PuyaLogManagerClearBaseAction Clear { get; protected set; }
        public override PuyaLogManagerGetByPKBaseAction GetByPK { get; protected set; }
        public override PuyaLogManagerGetPageBaseAction GetPage { get; protected set; }
        public override PuyaLogManagerDeleteByPKBaseAction DeleteByPK { get; protected set; }
		public PuyaLogManagerSql(IDb db)
		{
        	Clear = new PuyaLogManagerSqlClearAction(this);
        	Actions.Add("Clear", Clear);
        	GetByPK = new PuyaLogManagerSqlGetByPKAction(this);
        	Actions.Add("GetByPK", GetByPK);
        	GetPage = new PuyaLogManagerSqlGetPageAction(this);
        	Actions.Add("GetPage", GetPage);
        	DeleteByPK = new PuyaLogManagerSqlDeleteByPKAction(this);
        	Actions.Add("DeleteByPK", DeleteByPK);
			Init(db);
        }
		partial void Init(IDb db);
    }
}

