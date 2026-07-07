using Puya.Service;

namespace Puya.Logging.Services.LogManager
{
	public abstract partial class PuyaLogManagerGetPageBaseAction:
        ServiceAction<PuyaLogManagerBase, PuyaLogManagerGetPageRequest, PuyaLogManagerGetPageResponse>
    {
        public PuyaLogManagerGetPageBaseAction(PuyaLogManagerBase owner) : base(owner)
        {
        }
    }
}
