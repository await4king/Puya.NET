using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Logging.Services.LogManager
{
	public partial interface ITapLogManager :IService<TapLogManagerBaseConfig>
    {
        TapLogManagerClearBaseAction Clear { get; }
        TapLogManagerGetByPKBaseAction GetByPK { get; }
        TapLogManagerGetPageBaseAction GetPage { get; }
        TapLogManagerDeleteByPKBaseAction DeleteByPK { get; }
    }
}
