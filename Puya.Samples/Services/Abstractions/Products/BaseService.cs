using Puya.Logging;
using Puya.Data;
using Puya.Caching;
using Puya.Service;
using Puya.ServiceModel;
using Puya.Settings;
using Puya.Translation;
using Puya.Debugging;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Puya.Samples.Services.Products
{
	public abstract partial class TapProductsServiceBase : TapBaseActionBasedService, ITapProductsService
    {
        public abstract TapProductsServiceGetAllBaseAction GetAll { get; protected set; }
        public abstract TapProductsServiceSaveBaseAction Save { get; protected set; }
		public TapProductsServiceBase(ILogger logger, IDb db, ICacheManager cache, ISettingService settings, ITranslator translator, IServiceInterceptor interceptor, ILogProvider logProvider, IDebugger debugger) : base(logger, db, cache, settings, translator, interceptor, logProvider, debugger)
		{
			Init(logger, db, cache, settings, translator, interceptor, logProvider, debugger);
        }
		partial void Init(ILogger logger, IDb db, ICacheManager cache, ISettingService settings, ITranslator translator, IServiceInterceptor interceptor, ILogProvider logProvider, IDebugger debugger);
    }
}

