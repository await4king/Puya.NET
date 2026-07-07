using Puya.Service;

namespace Puya.Logging.Services.LogManager
{
	public abstract partial class PuyaLogManagerGetByPKBaseAction:
        ServiceAction<PuyaLogManagerBase, PuyaLogManagerGetByPKRequest, PuyaLogManagerGetByPKResponse>
    {
        public PuyaLogManagerGetByPKBaseAction(PuyaLogManagerBase owner) : base(owner)
        {
        }
    }
}
