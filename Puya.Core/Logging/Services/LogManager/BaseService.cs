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
	public abstract partial class PuyaLogManagerBase : BaseActionBasedService<PuyaLogManagerBaseConfig>, IPuyaLogManager
    {
        public abstract PuyaLogManagerClearBaseAction Clear { get; protected set; }
        public abstract PuyaLogManagerGetByPKBaseAction GetByPK { get; protected set; }
        public abstract PuyaLogManagerGetPageBaseAction GetPage { get; protected set; }
        public abstract PuyaLogManagerDeleteByPKBaseAction DeleteByPK { get; protected set; }
		public PuyaLogManagerBase(PuyaLogManagerBaseConfig config) : base(config)
		{
			Init(config);
        }
		partial void Init(PuyaLogManagerBaseConfig config);
    }
}

