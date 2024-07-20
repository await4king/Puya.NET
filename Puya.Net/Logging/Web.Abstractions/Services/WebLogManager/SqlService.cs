using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Data;

namespace Puya.Logging.Web.Abstractions.Services.WebLogManager
{
	public partial class TapWebLogManagerSql : TapWebLogManagerBase
    {
	public TapWebLogManagerSqlConfig StrongConfig
        {
            get { return Config as TapWebLogManagerSqlConfig; }
        }
        public override TapWebLogManagerClearBaseAction Clear { get; protected set; }
        public override TapWebLogManagerGetByPKBaseAction GetByPK { get; protected set; }
        public override TapWebLogManagerGetPageBaseAction GetPage { get; protected set; }
        public override TapWebLogManagerDeleteByPKBaseAction DeleteByPK { get; protected set; }
		public TapWebLogManagerSql(TapWebLogManagerSqlConfig config, IDb db) : base(config)
		{
        	Clear = new TapWebLogManagerSqlClearAction(this);
        	Actions.Add("Clear", Clear);
        	GetByPK = new TapWebLogManagerSqlGetByPKAction(this);
        	Actions.Add("GetByPK", GetByPK);
        	GetPage = new TapWebLogManagerSqlGetPageAction(this);
        	Actions.Add("GetPage", GetPage);
        	DeleteByPK = new TapWebLogManagerSqlDeleteByPKAction(this);
        	Actions.Add("DeleteByPK", DeleteByPK);
			Init(config, db);
        }
		partial void Init(TapWebLogManagerSqlConfig config, IDb db);
    }
}

