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
	public partial class ProductServiceSaveRequest : ServiceRequest
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Code  { get; set; }
		public decimal? Price { get; set; }
	}
}
