using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Puya.Conversion;

namespace Puya.Web
{
    public static class Extensions
    {
        //public static void Set(this HttpContextCurrentProvider provider)
        //{
        //    provider?.Set(new HttpContextWrapper(HttpContext.Current));
        //}
        static string hashNumber;
        static Extensions()
        {
            hashNumber = DateTime.Now.ToString("yyyyMMddhhmmss");
        }
        public static HtmlString ActionCss(ViewContext context)
        {
            var useActionCss = SafeClrConvert.ToBoolean(context.ViewData["UseActionCss"], true);

            if (useActionCss)
            {
                var area = context.RouteData.DataTokens["area"]?.ToString();
                var controller = context.RouteData.Values["controller"]?.ToString();
                var action = context.RouteData.Values["action"]?.ToString();

                if (string.IsNullOrEmpty(area))
                {
                    area = "app";
                }

                return new HtmlString($"<link href=\"/css/{area}/{controller}/{action}.css?{hashNumber}\" rel=\"stylesheet\" type=\"text/css\" />");
            }

            return new HtmlString("");
        }
        public static HtmlString ActionJs(ViewContext context)
        {
            var useActionJs = SafeClrConvert.ToBoolean(context.ViewData["UseActionJs"], true);

            if (useActionJs)
            {
                var area = context.RouteData.DataTokens["area"]?.ToString();
                var controller = context.RouteData.Values["controller"]?.ToString();
                var action = context.RouteData.Values["action"]?.ToString();

                if (string.IsNullOrEmpty(area))
                {
                    area = "app";
                }

                return new HtmlString($"<script src=\"/js/{area}/{controller}/{action}.js?{hashNumber}\"></script>");
            }

            return new HtmlString("");
        }
        public static HtmlString ActionCss(this RazorPageBase page)
        {
            return ActionCss(page.ViewContext);
        }
        public static HtmlString ActionJs(this RazorPageBase page)
        {
            return ActionJs(page.ViewContext);
        }
        /// <summary>
        /// Retrieve the raw body as a string from the Request.Body stream
        /// source: https://weblog.west-wind.com/posts/2017/sep/14/accepting-raw-request-body-content-in-aspnet-core-api-controllers
        /// </summary>
        /// <param name="request">Request instance to apply to</param>
        /// <param name="encoding">Optional - Encoding, defaults to UTF8</param>
        /// <returns></returns>
        public static async Task<string> GetRawBodyStringAsync(this HttpRequest request, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            using (StreamReader reader = new StreamReader(request.Body, encoding))
                return await reader.ReadToEndAsync();
        }

        /// <summary>
        /// Retrieves the raw body as a byte array from the Request.Body stream
        /// source: https://weblog.west-wind.com/posts/2017/sep/14/accepting-raw-request-body-content-in-aspnet-core-api-controllers
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<byte[]> GetRawBodyBytesAsync(this HttpRequest request)
        {
            using (var ms = new MemoryStream(2048))
            {
                await request.Body.CopyToAsync(ms);
                return ms.ToArray();
            }
        }
    }
}
