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
	public abstract partial class TapWebLogManagerBase : BaseActionBasedService<TapWebLogManagerBaseConfig>, ITapWebLogManager
    {
        public abstract TapWebLogManagerClearBaseAction Clear { get; protected set; }
        public abstract TapWebLogManagerGetByPKBaseAction GetByPK { get; protected set; }
        public abstract TapWebLogManagerGetPageBaseAction GetPage { get; protected set; }
        public abstract TapWebLogManagerDeleteByPKBaseAction DeleteByPK { get; protected set; }
		public TapWebLogManagerBase(TapWebLogManagerBaseConfig config) : base(config)
		{
			Init(config);
        }
		partial void Init(TapWebLogManagerBaseConfig config);
    }
}

