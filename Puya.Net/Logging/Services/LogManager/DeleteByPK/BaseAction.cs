using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Logging.Services.LogManager
{
	public abstract partial class TapLogManagerDeleteByPKBaseAction:
        ServiceAction<TapLogManagerBase, TapLogManagerBaseConfig, TapLogManagerDeleteByPKRequest, TapLogManagerDeleteByPKResponse>
    {
        public TapLogManagerDeleteByPKBaseAction(TapLogManagerBase owner) : base(owner)
        {
        }
    }
}
