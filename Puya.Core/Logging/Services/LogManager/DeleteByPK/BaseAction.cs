using Puya.Service;

namespace Puya.Logging.Services.LogManager
{
	public abstract partial class PuyaLogManagerDeleteByPKBaseAction:
        ServiceAction<PuyaLogManagerBase, PuyaLogManagerDeleteByPKRequest, PuyaLogManagerDeleteByPKResponse>
    {
        public PuyaLogManagerDeleteByPKBaseAction(PuyaLogManagerBase owner) : base(owner)
        {
        }
    }
}
