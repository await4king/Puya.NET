using Puya.Service;

namespace Puya.Logging.Services.LogManager
{
	public partial interface IPuyaLogManager :IService
    {
        PuyaLogManagerClearBaseAction Clear { get; }
        PuyaLogManagerGetByPKBaseAction GetByPK { get; }
        PuyaLogManagerGetPageBaseAction GetPage { get; }
        PuyaLogManagerDeleteByPKBaseAction DeleteByPK { get; }
    }
}
