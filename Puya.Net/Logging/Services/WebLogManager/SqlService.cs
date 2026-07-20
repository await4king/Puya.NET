using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Data;
using Puya.Logging.Services.WebLogManager;
using Puya.Logging.Services.WebLogManager.Clear;
using Puya.Logging.Services.WebLogManager.DeleteByPK;
using Puya.Logging.Services.WebLogManager.GetByPK;
using Puya.Logging.Services.WebLogManager.GetPage;

namespace Puya.Logging.Web.Abstractions.Services.WebLogManager
{
	public partial class PuyaWebLogManagerSql : PuyaWebLogManagerBase
    {
        public override PuyaWebLogManagerClearBaseAction Clear { get; protected set; }
        public override PuyaWebLogManagerGetByPKBaseAction GetByPK { get; protected set; }
        public override PuyaWebLogManagerGetPageBaseAction GetPage { get; protected set; }
        public override PuyaWebLogManagerDeleteByPKBaseAction DeleteByPK { get; protected set; }
		public PuyaWebLogManagerSql(IDb db)
		{
        	Clear = new PuyaWebLogManagerSqlClearAction(this);
        	Actions.Add("Clear", Clear);
        	GetByPK = new PuyaWebLogManagerSqlGetByPKAction(this);
        	Actions.Add("GetByPK", GetByPK);
        	GetPage = new PuyaWebLogManagerSqlGetPageAction(this);
        	Actions.Add("GetPage", GetPage);
        	DeleteByPK = new PuyaWebLogManagerSqlDeleteByPKAction(this);
        	Actions.Add("DeleteByPK", DeleteByPK);
			Init(db);
        }
		partial void Init(IDb db);
    }
}

