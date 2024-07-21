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
	public partial class TapLogManagerSql : TapLogManagerBase
    {
	public TapLogManagerSqlConfig StrongConfig
        {
            get { return Config as TapLogManagerSqlConfig; }
        }
        public override TapLogManagerClearBaseAction Clear { get; protected set; }
        public override TapLogManagerGetByPKBaseAction GetByPK { get; protected set; }
        public override TapLogManagerGetPageBaseAction GetPage { get; protected set; }
        public override TapLogManagerDeleteByPKBaseAction DeleteByPK { get; protected set; }
		public TapLogManagerSql(TapLogManagerSqlConfig config, IDb db) : base(config)
		{
        	Clear = new TapLogManagerSqlClearAction(this);
        	Actions.Add("Clear", Clear);
        	GetByPK = new TapLogManagerSqlGetByPKAction(this);
        	Actions.Add("GetByPK", GetByPK);
        	GetPage = new TapLogManagerSqlGetPageAction(this);
        	Actions.Add("GetPage", GetPage);
        	DeleteByPK = new TapLogManagerSqlDeleteByPKAction(this);
        	Actions.Add("DeleteByPK", DeleteByPK);
			Init(config, db);
        }
		partial void Init(TapLogManagerSqlConfig config, IDb db);
    }
}

