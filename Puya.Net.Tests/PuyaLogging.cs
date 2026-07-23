using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Puya.Conversion;
using Puya.Data;
using Puya.Logging;
using System.Security.Claims;
using System.Text;

namespace Puya.Net.Tests
{
    public class PuyaLogging
    {
        IDb GetDb()
        {
            var constrProvider = new DefaultConnectionStringProvider();

            constrProvider.SetConnectionString("Server=.\\I2k17;Database=MyDb;User Id=sa;Password=sql2k17pass123;TrustServerCertificate=true;MultipleActiveResultSets=true");

            var db = new SqlServerDb(constrProvider);

            db.GetConnection();

            return db;
        }
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
        SqlServerWebLoggerConfig GetDbLoggerConfig(Dictionary<string, string> headers,
                                                    string method,
                                                    string username,
                                                    Claim[] claims,
                                                    string roles = null,
                                                    string users = null,
                                                    IFeatureCollection features = null,
                                                    string body = null,
                                                    string contentType = null)
        {
            var httpcontextAccessor = GetHttpContextAccessor(headers, method, username, claims, features, body, contentType);
            var policy = new WebLoggingUserRolePolicy(httpcontextAccessor, roles, users);

            var result = new SqlServerWebLoggerConfig(policy);

            result.Level = LogLevel.All;

            return result;
        }
        DebugLoggerConfig GetDebugLoggerConfig(Dictionary<string, string> headers,
                                                    string method,
                                                    string username,
                                                    Claim[] claims,
                                                    string roles = null,
                                                    string users = null,
                                                    IFeatureCollection features = null,
                                                    string body = null,
                                                    string contentType = null)
        {
            var httpcontextAccessor = GetHttpContextAccessor(headers, method, username, claims, features, body, contentType);
            var policy = new WebLoggingUserRolePolicy(httpcontextAccessor, roles, users);

            var result = new DebugLoggerConfig(policy);

            result.Level = LogLevel.All;

            return result;
        }
        [Fact]
        public void Test_DbLogger_LogsTable_Exists()
        {
            var db = GetDb();
            var config = GetDbLoggerConfig(null, "GET", null, null);
            var tbl = config.LogTable;
            var schema = "dbo";
            var index = tbl.IndexOf(".");

            if (index > 0)
            {
                schema = tbl.Substring(0, index);
                tbl = tbl.Substring(index + 1);
            }

            var exists = db.ExecuteScalarSql(@"
select case when exists
(
    select 1
    from            sys.tables  t
        inner join  sys.schemas s on t.schema_id = s.schema_id
    where t.name = @tbl and s.name = @schema
) then 1 else 0 end", new { tbl, schema });

            Assert.True(SafeClrConvert.ToBoolean(exists));
        }
        [Fact]
        public void Test_DbLogger_No_Roles_And_Users_Policy_DoNotAllowNotAuthenticated()
        {
            var db = GetDb();
            var config = GetDbLoggerConfig(null, "GET", null, null);
            var logger = new SqlServerWebLogger(config, db);

            logger.Clear();

            logger.Info("test");

            var logs = logger.FetchLogs();

            Assert.True(logs.Count == 0);
        }
        [Fact]
        public void Test_DbLogger_No_Roles_And_Users_Policy_AllowNotAuthenticated()
        {
            var db = GetDb();
            var config = GetDbLoggerConfig(null, "GET", null, null);
            
            (config.Policy as WebLoggingUserRolePolicy).AllowNotAuthenticatedUsers = true;
            
            var logger = new SqlServerWebLogger(config, db);

            logger.Clear();

            logger.Info("test");

            var logs = logger.FetchLogs();

            Assert.True(logs.Count == 1);
        }
        [Fact]
        public void Test_DbLogger_User_Policy()
        {
            var db = GetDb();
            var config = GetDbLoggerConfig(null, "GET", "ali", null, null, "ali");
            var logger = new SqlServerWebLogger(config, db);

            logger.Clear();

            logger.Info("test");

            var logs = logger.FetchLogs();

            Assert.True(logs.Count == 1);
        }
        [Fact]
        public void Test_DbLogger_User_And_Role_Policy_By_User1()
        {
            var db = GetDb();
            var config = GetDbLoggerConfig(null, "GET", "ali", null, "debugger", "ali");
            var logger = new SqlServerWebLogger(config, db);

            logger.Clear();

            logger.Info("test");

            var logs = logger.FetchLogs();

            Assert.True(logs.Count == 1);
        }
        [Fact]
        public void Test_DbLogger_User_And_Role_Policy_By_User2()
        {
            var db = GetDb();
            var config = GetDbLoggerConfig(null, "GET", "ali", null, "debugger", "hasan");
            var logger = new SqlServerWebLogger(config, db);

            logger.Clear();

            logger.Info("test");

            var logs = logger.FetchLogs();

            Assert.True(logs.Count == 0);
        }
        [Fact]
        public void Test_DbLogger_User_And_Role_Policy_By_Role1()
        {
            var db = GetDb();
            var config = GetDbLoggerConfig(null, "GET", "ali", new Claim[] { new Claim(ClaimTypes.Role, "debugger") }, "debugger", "hasan");
            var logger = new SqlServerWebLogger(config, db);

            logger.Clear();

            logger.Info("test");

            var logs = logger.FetchLogs();

            Assert.True(logs.Count == 1);
        }
        [Fact]
        public void Test_DbLogger_User_And_Role_Policy_By_Role2()
        {
            var db = GetDb();
            var config = GetDbLoggerConfig(null, "GET", "ali", new Claim[] { new Claim(ClaimTypes.Role, "operator") }, "debugger", "hasan");
            var logger = new SqlServerWebLogger(config, db);

            logger.Clear();

            logger.Info("test");

            var logs = logger.FetchLogs();

            Assert.True(logs.Count == 0);
        }
        [Fact]
        public void Test_DbLogger_User_And_Role_Policy()
        {
            var db = GetDb();
            var config = GetDbLoggerConfig(null, "GET", "ali", new Claim[] { new Claim(ClaimTypes.Role, "debugger") }, "debugger", "ali");
            var logger = new SqlServerWebLogger(config, db);

            logger.Clear();

            logger.Info("test");

            var logs = logger.FetchLogs();

            Assert.True(logs.Count == 1);
        }
        [Fact]
        public void Test_DbLogger_Clear()
        {
            var db = GetDb();
            var config = GetDbLoggerConfig(null, "GET", null, null);
            var logger = new SqlServerWebLogger(config, db);

            logger.Info("test");
            
            logger.Clear();

            var count = db.ExecuteScalarSql("select count(*) from " + config.LogTable);

            Assert.True(SafeClrConvert.ToInt(count) == 0);
        }
        [Fact]
        public void Test_DbLogger_MaxDailyLog()
        {
            var db = GetDb();
            var config = GetDbLoggerConfig(null, "GET", null, null);

            (config.Policy as WebLoggingUserRolePolicy).AllowNotAuthenticatedUsers = true;
            config.MaxDailyLog = 3;

            var logger = new SqlServerWebLogger(config, db);

            logger.Clear();

            logger.Info("test");
            logger.Info("test");
            logger.Info("test");
            logger.Info("test");

            var logs = logger.FetchLogs();

            Assert.True(logs.Count == 1);
        }
        [Fact]
        public void Test_DbLogger_MaxLog()
        {
            var db = GetDb();
            var config = GetDbLoggerConfig(null, "GET", null, null);

            (config.Policy as WebLoggingUserRolePolicy).AllowNotAuthenticatedUsers = true;
            config.MaxLog = 3;

            var logger = new SqlServerWebLogger(config, db);

            logger.Clear();

            logger.Info("test");
            logger.Info("test");
            logger.Info("test");
            logger.Info("test");

            var logs = logger.FetchLogs();

            Assert.True(logs.Count == 1);
        }
        [Fact]
        public void Test_Mock_HttpContext()
        {
            // 1. Create the form data
            var formData = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
    {
        { "username", "testuser" },
        { "password", "secure123" }
    };
            var formCollection = new FormCollection(formData);

            // 2. Create a service provider with basic services
            var services = new ServiceCollection();
            services.AddHttpContextAccessor();
            services.AddLogging();
            var serviceProvider = services.BuildServiceProvider();

            // 3. Create the HttpContext
            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider
            };

            // 4. Set up the request
            httpContext.Request.Method = "POST";
            httpContext.Request.ContentType = "application/x-www-form-urlencoded";

            // 5. CRITICAL: Set the form feature on the request's feature collection
            httpContext.Request.Form = formCollection;  // ← This is the key!

            // 6. Set user if needed
            var claimsPrincipal = GetPrincipal("testuser", new Claim[] { });
            if (claimsPrincipal != null)
            {
                httpContext.User = claimsPrincipal;
            }

            // 7. Now this will work
            var username = httpContext.Request.Form["username"];

            Assert.True(username == "testuser");
        }
        [Fact]
        public void Test_DbLogger_log1()
        {
            // Create form data
            var formData = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
    {
        { "username", "testuser" },
        { "password", "secure123" }
    };
            var formCollection = new FormCollection(formData);

            // Setup service provider
            var services = new ServiceCollection();
            services.AddHttpContextAccessor();
            var serviceProvider = services.BuildServiceProvider();

            // Create HttpContext
            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider
            };

            // Setup request
            var url = new Uri("https://ali.com/api/test?foo=bar");
            httpContext.Request.Method = "POST";
            httpContext.Request.ContentType = "application/x-www-form-urlencoded";
            httpContext.Request.Form = formCollection;
            httpContext.Request.Scheme = url.Scheme;
            httpContext.Request.Path = url.LocalPath;
            httpContext.Request.QueryString = new QueryString(url.Query);
            httpContext.Request.Headers["Host"] = url.Host;

            var db = GetDb();
            var config = GetDbLoggerConfig(null, "POST", "ali", null, null, "ali", httpContext.Features);
            var logger = new SqlServerWebLogger(config, db);

            logger.Clear();
            logger.Info("test");

            var logs = logger.FetchLogs();

            Assert.True(logs.Count == 1);

            var log0 = logs[0] as Log;
            Assert.NotNull(log0);
            Assert.True(string.Equals(log0.Method, "POST", StringComparison.OrdinalIgnoreCase));
            Assert.True(string.IsNullOrEmpty(log0.Form));
        }

        [Fact]
        public void Test_DbLogger_log11()
        {
            // Create form data
            var formData = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
    {
        { "username", "testuser" },
        { "password", "secure123" }
    };
            var formCollection = new FormCollection(formData);

            // Setup service provider
            var services = new ServiceCollection();
            services.AddHttpContextAccessor();
            var serviceProvider = services.BuildServiceProvider();

            // Create HttpContext
            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider
            };

            // Setup request
            var url = new Uri("https://ali.com/api/test?foo=bar");
            httpContext.Request.Method = "POST";
            httpContext.Request.ContentType = "application/x-www-form-urlencoded";
            httpContext.Request.Form = formCollection;
            httpContext.Request.Scheme = url.Scheme;
            httpContext.Request.Path = url.LocalPath;
            httpContext.Request.QueryString = new QueryString(url.Query);
            httpContext.Request.Headers["Host"] = url.Host;

            var db = GetDb();
            var config = GetDbLoggerConfig(null, "POST", "ali", null, null, "ali", httpContext.Features);
            var logger = new SqlServerWebLogger(config, db);

            logger.UseForm();
            logger.Clear();
            logger.Info("test");

            var logs = logger.FetchLogs();

            Assert.True(logs.Count == 1);

            var log0 = logs[0] as Log;
            Assert.NotNull(log0);
            Assert.False(string.IsNullOrEmpty(log0.Form));
            Assert.True(log0.Method == "POST");
            Assert.True(log0.Form.Contains("username", StringComparison.OrdinalIgnoreCase));
            Assert.True(log0.Form.Contains("password", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void Test_DbLogger_log12()
        {
            // Create form data
            var formData = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
    {
        { "username", "testuser" },
        { "password", "secure123" }
    };
            var formCollection = new FormCollection(formData);

            // Setup service provider
            var services = new ServiceCollection();
            services.AddHttpContextAccessor();
            var serviceProvider = services.BuildServiceProvider();

            // Create HttpContext
            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider
            };

            // Setup request
            var url = new Uri("https://ali.com/api/test?foo=bar");
            httpContext.Request.Method = "POST";
            httpContext.Request.ContentType = "application/x-www-form-urlencoded";
            httpContext.Request.Form = formCollection;
            httpContext.Request.Scheme = url.Scheme;
            httpContext.Request.Path = url.LocalPath;
            httpContext.Request.QueryString = new QueryString(url.Query);
            httpContext.Request.Headers["Host"] = url.Host;

            var db = GetDb();
            var config = GetDbLoggerConfig(null, "POST", "ali", null, null, "ali", httpContext.Features);
            var logger = new SqlServerWebLogger(config, db);

            logger.UseForm("username");
            logger.Clear();
            logger.Info("test");

            var logs = logger.FetchLogs();

            Assert.True(logs.Count == 1);

            var log0 = logs[0] as Log;
            Assert.NotNull(log0);
            Assert.False(string.IsNullOrEmpty(log0.Form));
            Assert.True(log0.Method == "POST");
            Assert.True(log0.Form.Contains("username", StringComparison.OrdinalIgnoreCase));
            Assert.False(log0.Form.Contains("password", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void Test_DbLogger_log13()
        {
            // Create form data (empty for body test)
            var formData = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>();
            var formCollection = new FormCollection(formData);

            // Setup service provider
            var services = new ServiceCollection();
            services.AddHttpContextAccessor();
            var serviceProvider = services.BuildServiceProvider();

            // Create HttpContext
            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider
            };

            // Setup request
            var url = new Uri("https://ali.com/api/test?foo=bar");
            var body = "{\"username\":\"testuser\",\"password\":\"secure123\"}";
            var bodyBytes = System.Text.Encoding.UTF8.GetBytes(body);
            var stream = new MemoryStream(bodyBytes);

            httpContext.Request.Method = "POST";
            httpContext.Request.ContentType = "application/json";
            httpContext.Request.Form = formCollection;
            httpContext.Request.Scheme = url.Scheme;
            httpContext.Request.Path = url.LocalPath;
            httpContext.Request.QueryString = new QueryString(url.Query);
            httpContext.Request.Headers["Host"] = url.Host;
            httpContext.Request.Body = stream;

            var db = GetDb();
            var config = GetDbLoggerConfig(null, "POST", "ali", null, null, "ali", httpContext.Features, body, "application/json");
            var logger = new SqlServerWebLogger(config, db);

            logger.UseBody();
            logger.Clear();
            logger.Info("test");

            var logs = logger.FetchLogs();

            Assert.True(logs.Count == 1);

            var log0 = logs[0] as Log;
            Assert.NotNull(log0);
            Assert.False(string.IsNullOrEmpty(log0.Body));
            Assert.True(log0.Method == "POST");
            Assert.True(string.Equals(body, log0.Body));
        }

        [Fact]
        public void Test_DebugLogger_Log_1()
        {
            // Create form data (empty for body test)
            var formData = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>();
            var formCollection = new FormCollection(formData);

            // Setup service provider
            var services = new ServiceCollection();
            services.AddHttpContextAccessor();
            var serviceProvider = services.BuildServiceProvider();

            // Create HttpContext
            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider
            };

            // Setup request
            var url = new Uri("https://ali.com/api/test?foo=bar");
            var body = "{\"username\":\"testuser\",\"password\":\"secure123\"}";
            var bodyBytes = System.Text.Encoding.UTF8.GetBytes(body);
            var stream = new MemoryStream(bodyBytes);

            httpContext.Request.Method = "POST";
            httpContext.Request.ContentType = "application/json";
            httpContext.Request.Form = formCollection;
            httpContext.Request.Scheme = url.Scheme;
            httpContext.Request.Path = url.LocalPath;
            httpContext.Request.QueryString = new QueryString(url.Query);
            httpContext.Request.Headers["Host"] = url.Host;
            httpContext.Request.Body = stream;

            var config = GetDebugLoggerConfig(null, "POST", "ali", null, null, "ali", httpContext.Features, body, "application/json");
            var logger = new DebugLogger(config);

            logger.UseBody();
            logger.Clear();
            logger.Info("hello");
            logger.Debug("BeginJob", "this is a message", () => new { a = 10, b = true, c = "test" });

            Assert.True(true);
        }
    }
}
