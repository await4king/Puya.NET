using Puya.Service;
using Puya.Logging.Web.Abstractions.Services.WebLogManager;

namespace Puya.Logging.Services.WebLogManager.GetPage
{
	public abstract partial class PuyaWebLogManagerGetPageBaseAction:
        ServiceAction<PuyaWebLogManagerBase, PuyaWebLogManagerBaseConfig, PuyaWebLogManagerGetPageRequest, PuyaWebLogManagerGetPageResponse>
    {
        public PuyaWebLogManagerGetPageBaseAction(PuyaWebLogManagerBase owner) : base(owner)
        {
        }
    }
}
