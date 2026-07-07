using Puya.Service;

namespace Puya.Logging.Services.LogManager
{
	public abstract partial class PuyaLogManagerBase : BaseActionBasedService, IPuyaLogManager
    {
        public abstract PuyaLogManagerClearBaseAction Clear { get; protected set; }
        public abstract PuyaLogManagerGetByPKBaseAction GetByPK { get; protected set; }
        public abstract PuyaLogManagerGetPageBaseAction GetPage { get; protected set; }
        public abstract PuyaLogManagerDeleteByPKBaseAction DeleteByPK { get; protected set; }
		public PuyaLogManagerBase()
		{
			Init();
        }
		partial void Init();
    }
}

