using Puya.Logging.Web.Abstractions.Services.WebLogManager;
using Puya.Logging.Services.WebLogManager;
using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Logging.Services.WebLogManager.DeleteByPK
{
	public abstract partial class PuyaWebLogManagerDeleteByPKBaseAction:
        ServiceAction<PuyaWebLogManagerBase, PuyaWebLogManagerDeleteByPKRequest, PuyaWebLogManagerDeleteByPKResponse>
    {
        public PuyaWebLogManagerDeleteByPKBaseAction(PuyaWebLogManagerBase owner) : base(owner)
        {
        }
    }
}
