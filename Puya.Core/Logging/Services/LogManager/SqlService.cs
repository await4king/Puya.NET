using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Data;

namespace Puya.Logging.Services.LogManager
{
	public partial class PuyaLogManagerSql : PuyaLogManagerBase
    {
	public PuyaLogManagerSqlConfig StrongConfig
        {
            get { return Config as PuyaLogManagerSqlConfig; }
        }
        public override PuyaLogManagerClearBaseAction Clear { get; protected set; }
        public override PuyaLogManagerGetByPKBaseAction GetByPK { get; protected set; }
        public override PuyaLogManagerGetPageBaseAction GetPage { get; protected set; }
        public override PuyaLogManagerDeleteByPKBaseAction DeleteByPK { get; protected set; }
		public PuyaLogManagerSql(PuyaLogManagerSqlConfig config, IDb db) : base(config)
		{
        	Clear = new PuyaLogManagerSqlClearAction(this);
        	Actions.Add("Clear", Clear);
        	GetByPK = new PuyaLogManagerSqlGetByPKAction(this);
        	Actions.Add("GetByPK", GetByPK);
        	GetPage = new PuyaLogManagerSqlGetPageAction(this);
        	Actions.Add("GetPage", GetPage);
        	DeleteByPK = new PuyaLogManagerSqlDeleteByPKAction(this);
        	Actions.Add("DeleteByPK", DeleteByPK);
			Init(config, db);
        }
		partial void Init(PuyaLogManagerSqlConfig config, IDb db);
    }
}

