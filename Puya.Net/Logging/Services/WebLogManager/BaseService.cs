using Puya.Service;
using Puya.Logging.Services.WebLogManager.Clear;
using Puya.Logging.Services.WebLogManager.DeleteByPK;
using Puya.Logging.Services.WebLogManager.GetByPK;
using Puya.Logging.Services.WebLogManager.GetPage;

namespace Puya.Logging.Services.WebLogManager
{
	public abstract partial class PuyaWebLogManagerBase : BaseActionBasedService, IPuyaWebLogManager
    {
        public abstract PuyaWebLogManagerClearBaseAction Clear { get; protected set; }
        public abstract PuyaWebLogManagerGetByPKBaseAction GetByPK { get; protected set; }
        public abstract PuyaWebLogManagerGetPageBaseAction GetPage { get; protected set; }
        public abstract PuyaWebLogManagerDeleteByPKBaseAction DeleteByPK { get; protected set; }
		public PuyaWebLogManagerBase()
		{
			Init();
        }
		partial void Init();
    }
}

