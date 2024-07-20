using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Logging.Services.LogManager
{
	public abstract partial class TapLogManagerClearBaseAction:
        ServiceAction<TapLogManagerBase, TapLogManagerBaseConfig, TapLogManagerClearRequest, TapLogManagerClearResponse>
    {
        public TapLogManagerClearBaseAction(TapLogManagerBase owner) : base(owner)
        {
        }
    }
}
