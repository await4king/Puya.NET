using Puya.Service;

namespace Puya.Logging.Services.LogManager
{
	public abstract partial class PuyaLogManagerClearBaseAction:
        ServiceAction<PuyaLogManagerBase, PuyaLogManagerClearRequest, PuyaLogManagerClearResponse>
    {
        public PuyaLogManagerClearBaseAction(PuyaLogManagerBase owner) : base(owner)
        {
        }
    }
}
