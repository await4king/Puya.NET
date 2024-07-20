using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Logging.Web.Abstractions.Models;

namespace Puya.Logging.Web.Abstractions.Services.WebLogManager
{
	public abstract partial class TapWebLogManagerGetByPKBaseAction:
        ServiceAction<TapWebLogManagerBase, TapWebLogManagerBaseConfig, TapWebLogManagerGetByPKRequest, TapWebLogManagerGetByPKResponse>
    {
        public TapWebLogManagerGetByPKBaseAction(TapWebLogManagerBase owner) : base(owner)
        {
        }
    }
}
