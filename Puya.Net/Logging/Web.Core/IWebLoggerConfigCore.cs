using Puya.Logging.Web.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Logging.WebCore
{
    public interface IWebLoggerConfigCore: IBaseWebLoggerConfig
    {
        IHttpContextAccessor HttpContextAccessor { get; }
    }
}
