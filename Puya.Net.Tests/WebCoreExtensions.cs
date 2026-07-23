using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.HttpSys;
using Moq;
using Puya.Extensions;
using System.Security.Claims;
using System.Text;

namespace Puya.Net.Tests
{
    public class MyHttpContext : HttpContext
    {
        public override IFeatureCollection Features => throw new NotImplementedException();

        public override HttpRequest? Request { get; }

        public override HttpResponse Response { get; }

        public override ConnectionInfo Connection { get; }

        public override WebSocketManager WebSockets { get; }

        public override ClaimsPrincipal User { get; set; }
        public override IDictionary<object, object> Items { get; set; }
        public override IServiceProvider RequestServices { get; set; }
        public override CancellationToken RequestAborted { get; set; }
        public override string TraceIdentifier { get; set; }
        public override ISession Session { get; set; }
        public MyHttpContext()
        { }
        public MyHttpContext(HttpRequest request)
        {
            Request = request;
        }
        public MyHttpContext(HttpRequest request, HttpResponse response)
        {
            Request = request;
            Response = response;
        }
        public override void Abort()
        {
            throw new NotImplementedException();
        }
    }
    public class MyHttpRequest : HttpRequest
    {
        public override HttpContext HttpContext { get; }

        public override string Method { get; set; }
        public override string Scheme { get; set; }
        public override bool IsHttps { get; set; }
        public override HostString Host { get; set; }
        public override PathString PathBase { get; set; }
        public override PathString Path { get; set; }
        public override QueryString QueryString { get; set; }
        public override IQueryCollection Query { get; set; }
        public override string Protocol { get; set; }

        public override IHeaderDictionary Headers => throw new NotImplementedException();

        public override IRequestCookieCollection Cookies { get; set; }
        public override long? ContentLength { get; set; }
        public override string ContentType { get; set; }
        public override Stream Body { get; set; }

        public override bool HasFormContentType { get; }

        public override IFormCollection Form { get; set; }

        public override Task<IFormCollection> ReadFormAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
    public class WebCoreExtensions
    {
        ClaimsPrincipal GetPrincipal(string username, Claim[] claims)
        {
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }

            if (claims == null)
            {
                claims = new Claim[0];
            }

            var _claims = claims.ToList();

            _claims.Add(new Claim(ClaimTypes.Name, username));

            var identity = new ClaimsIdentity(_claims, "Cookies");

            return new ClaimsPrincipal(identity);
        }
        IHttpContextAccessor GetHttpContextAccessor(Dictionary<string, string> headers,
                                                    string method,
                                                    string username,
                                                    Claim[] claims,
                                                    IFeatureCollection features = null,
                                                    string body = null,
                                                    string contentType = null)
        {
            var mockAccessor = new Mock<IHttpContextAccessor>();
            var context = features == null ? new DefaultHttpContext() : new DefaultHttpContext(features);

            context.Request.Method = method;
            context.Request.ContentType = contentType;

            var claimsPrincipal = GetPrincipal(username, claims);

            if (claimsPrincipal != null)
            {
                context.User = claimsPrincipal;
            }

            context.Request.Headers["Content-Type"] = contentType;

            if (headers?.Count > 0)
            {
                foreach (var item in headers)
                {
                    context.Request.Headers[item.Key] = item.Value;
                }
            }

            if (!string.IsNullOrEmpty(body) && !string.IsNullOrEmpty(contentType))
            {
                var bodyStream = new MemoryStream(Encoding.UTF8.GetBytes(body));

                bodyStream.Seek(0, SeekOrigin.Begin); // Reset position

                context.Request.Body = bodyStream;
                context.Request.ContentType = contentType;
            }

            mockAccessor.Setup(_ => _.HttpContext).Returns(context);

            return mockAccessor.Object;
        }
        [Fact]
        public void Test_TryGetHttpContext_Null_Input()
        {
            var httpcontextAccessor = null as IHttpContextAccessor;
            var result = httpcontextAccessor.TryGetHttpContext(out HttpContext context);

            Assert.False(result);
        }
        [Fact]
        public void Test_TryGetHttpContext_Empty_Context_With_Default_Options()
        {
            var mockAccessor = new Mock<IHttpContextAccessor>();
            var context = new MyHttpContext();

            mockAccessor.Setup(_ => _.HttpContext).Returns(context);

            var httpcontextAccessor = mockAccessor.Object;
            var result = httpcontextAccessor.TryGetHttpContext(out HttpContext httpContext);

            Assert.False(result);
        }
        [Fact]
        public void Test_TryGetHttpContext_Empty_Context_With_Options()
        {
            var mockAccessor = new Mock<IHttpContextAccessor>();
            var context = new MyHttpContext();

            mockAccessor.Setup(_ => _.HttpContext).Returns(context);

            var httpcontextAccessor = mockAccessor.Object;
            var result = httpcontextAccessor.TryGetHttpContext(out HttpContext httpContext, "request");

            Assert.False(result);
        }
        [Fact]
        public void Test_TryGetHttpContext_Request_Only_Context_With_Default_Options()
        {
            var mockAccessor = new Mock<IHttpContextAccessor>();
            var context = new MyHttpContext(new MyHttpRequest());

            mockAccessor.Setup(_ => _.HttpContext).Returns(context);

            var httpcontextAccessor = mockAccessor.Object;
            var result = httpcontextAccessor.TryGetHttpContext(out HttpContext httpContext);

            Assert.False(result);
        }
        [Fact]
        public void Test_TryGetHttpContext_Request_Only_Context_With_Options()
        {
            var mockAccessor = new Mock<IHttpContextAccessor>();
            var context = new MyHttpContext(new MyHttpRequest());

            mockAccessor.Setup(_ => _.HttpContext).Returns(context);

            var httpcontextAccessor = mockAccessor.Object;
            var result = httpcontextAccessor.TryGetHttpContext(out HttpContext httpContext, "request");

            Assert.True(result);
        }
        [Fact]
        public void Test_TryGetHttpContext_Check_Request_Default_Options()
        {
            var httpcontextAccessor = GetHttpContextAccessor(headers: null, method: "GET", username: null, claims: null, features: null, body: null, contentType: null);
            var result = httpcontextAccessor.TryGetHttpContext(out HttpContext context);

            Assert.True(result);
        }
        [Fact]
        public void Test_TryGetHttpContext_Check_Request_By_Options1()
        {
            var httpcontextAccessor = GetHttpContextAccessor(headers: null, method: "GET", username: null, claims: null, features: null, body: null, contentType: null);
            var result = httpcontextAccessor.TryGetHttpContext(out HttpContext context, "request");

            Assert.True(result);
        }
        [Fact]
        public void Test_TryGetHttpContext_Check_Request_By_Options2()
        {
            var httpcontextAccessor = GetHttpContextAccessor(headers: null, method: "GET", username: null, claims: null, features: null, body: null, contentType: null);
            var result = httpcontextAccessor.TryGetHttpContext(out HttpContext context, "request,request.headers");

            Assert.True(result);
        }
        [Fact]
        public void Test_TryGetHttpContext_Request_By_Options3()
        {
            var formData = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
{
    { "username", "testuser" },
    { "password", "secure123" }
};

            // Create a FormCollection with your mock data
            var formCollection = new FormCollection(formData);

            // Create a FormFeature and set it in a FeatureCollection
            var formFeature = new FormFeature(formCollection);
            var features = new FeatureCollection();
            var url = new Uri("https://ali.com/api/test?foo=bar");
            features.Set<IFormFeature>(formFeature);
            features.Set<IHttpRequestFeature>(new HttpRequestFeature
            {
                Scheme = url.Scheme,
                Path = url.LocalPath,
                QueryString = url.Query,
                Headers = new HeaderDictionary { ["Host"] = url.Host }
            });

            var httpcontextAccessor = GetHttpContextAccessor(headers: null, method: "POST", username: null, claims: null, features, body: null, contentType: null);
            var result = httpcontextAccessor.TryGetHttpContext(out HttpContext context, "request,request.headers");

            Assert.True(result);
        }
    }
}
