using Puya.Service;
using Puya.Logging.Web.Abstractions.Services.WebLogManager;

namespace Puya.Logging.Services.WebLogManager.GetByPK
{
	public abstract partial class PuyaWebLogManagerGetByPKBaseAction:
        ServiceAction<PuyaWebLogManagerBase, PuyaWebLogManagerGetByPKRequest, PuyaWebLogManagerGetByPKResponse>
    {
        public PuyaWebLogManagerGetByPKBaseAction(PuyaWebLogManagerBase owner) : base(owner)
        {
        }
    }
}
