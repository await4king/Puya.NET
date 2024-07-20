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
	public abstract partial class TapLogManagerBase : BaseActionBasedService<TapLogManagerBaseConfig>, ITapLogManager
    {
        public abstract TapLogManagerClearBaseAction Clear { get; protected set; }
        public abstract TapLogManagerGetByPKBaseAction GetByPK { get; protected set; }
        public abstract TapLogManagerGetPageBaseAction GetPage { get; protected set; }
        public abstract TapLogManagerDeleteByPKBaseAction DeleteByPK { get; protected set; }
		public TapLogManagerBase(TapLogManagerBaseConfig config) : base(config)
		{
			Init(config);
        }
		partial void Init(TapLogManagerBaseConfig config);
    }
}

