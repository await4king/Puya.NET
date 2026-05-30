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

namespace Puya.Samples.Products.Products
{
	public partial class ProductServiceGetAllRequest : ServiceRequest
    {
		public int Page { get; set; }
		public int PageSize { get; set; }
		public string Name { get; set; }
		public decimal? PriceFrom { get; set; }
		public decimal? PriceTo { get; set; }
	}
}
