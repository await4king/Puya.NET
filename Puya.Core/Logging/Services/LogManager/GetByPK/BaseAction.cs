using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Logging.Models;

namespace Puya.Logging.Services.LogManager
{
	public abstract partial class TapLogManagerGetByPKBaseAction:
        ServiceAction<TapLogManagerBase, TapLogManagerBaseConfig, TapLogManagerGetByPKRequest, TapLogManagerGetByPKResponse>
    {
        public TapLogManagerGetByPKBaseAction(TapLogManagerBase owner) : base(owner)
        {
        }
    }
}
