using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Logging.Services.LogManager
{
	public abstract partial class PuyaLogManagerClearBaseAction:
        ServiceAction<PuyaLogManagerBase, PuyaLogManagerBaseConfig, PuyaLogManagerClearRequest, PuyaLogManagerClearResponse>
    {
        public PuyaLogManagerClearBaseAction(PuyaLogManagerBase owner) : base(owner)
        {
        }
    }
}
