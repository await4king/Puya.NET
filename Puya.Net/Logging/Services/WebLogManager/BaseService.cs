using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Data;
using Puya.Logging.Web.Abstractions.Services.WebLogManager;
using Puya.Logging.Services.WebLogManager.Clear;
using Puya.Logging.Services.WebLogManager.DeleteByPK;
using Puya.Logging.Services.WebLogManager.GetByPK;
using Puya.Logging.Services.WebLogManager.GetPage;

namespace Puya.Logging.Services.WebLogManager
{
	public abstract partial class PuyaWebLogManagerBase : BaseActionBasedService<PuyaWebLogManagerBaseConfig>, IPuyaWebLogManager
    {
        public abstract PuyaWebLogManagerClearBaseAction Clear { get; protected set; }
        public abstract PuyaWebLogManagerGetByPKBaseAction GetByPK { get; protected set; }
        public abstract PuyaWebLogManagerGetPageBaseAction GetPage { get; protected set; }
        public abstract PuyaWebLogManagerDeleteByPKBaseAction DeleteByPK { get; protected set; }
		public PuyaWebLogManagerBase(PuyaWebLogManagerBaseConfig config) : base(config)
		{
			Init(config);
        }
		partial void Init(PuyaWebLogManagerBaseConfig config);
    }
}

