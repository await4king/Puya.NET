using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Logging.Web.Abstractions.Models;
using Puya.Logging.Models;

namespace Puya.Logging.Web.Abstractions.Services.WebLogManager
{
	public abstract partial class TapWebLogManagerGetPageBaseAction:
        ServiceAction<TapWebLogManagerBase, TapWebLogManagerBaseConfig, TapWebLogManagerGetPageRequest, TapWebLogManagerGetPageResponse>
    {
        public TapWebLogManagerGetPageBaseAction(TapWebLogManagerBase owner) : base(owner)
        {
        }
    }
}
