using Puya.Collections;
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
using Puya.Security;

namespace Puya.Samples.Products.Products
{
	public abstract partial class ProductServiceBase : TapBaseActionBasedService<ProductServiceBaseConfig>, IProductService
    {
        public abstract ProductServiceSaveBaseAction Save { get; protected set; }
        public abstract ProductServiceGetAllBaseAction GetAll { get; protected set; }
		public ProductServiceBase(ProductServiceBaseConfig config, ILogger logger, IDb db, ICacheManager cache, ISettingService settings, ITranslator translator, IServiceInterceptor interceptor, ILogProvider logProvider, IDebugger debugger) : base(config, logger, db, cache, settings, translator, interceptor, logProvider, debugger)
		{
			Init(config, logger, db, cache, settings, translator, interceptor, logProvider, debugger);
        }
		partial void Init(ProductServiceBaseConfig config, ILogger logger, IDb db, ICacheManager cache, ISettingService settings, ITranslator translator, IServiceInterceptor interceptor, ILogProvider logProvider, IDebugger debugger);
    }
}

