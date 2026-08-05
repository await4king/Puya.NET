using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Api;
using Puya.Debugging;

namespace Puya.Api
{
    public class ApiGatewayController : Controller
    {
        private readonly IApiGateway gateway;
        private readonly IDebugger debugger;

        public ApiGatewayController(IApiGateway gateway, IDebugger debugger)
        {
            this.gateway = gateway;
            this.debugger = debugger;
        }
        public virtual async Task<IActionResult> Root(CancellationToken cancellation)
        {
            var content = await gateway.ProcessAsync(ControllerContext.HttpContext, cancellation);

            System.Diagnostics.Debug.WriteLine("IsDebugging: " + debugger.IsDebugging);

            if (ControllerContext.HttpContext.Response.Headers.ContainsKey(ApiGatewayConstants.EncryptedResponseHeaderName))
            {
                return Content(content, "text/plain");
            }
            else
            {
                return Content(content, "application/json");
            }
        }
    }
}
