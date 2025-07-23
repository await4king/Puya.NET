using Microsoft.AspNetCore.Http;
using Moq;
using Puya.Conversion;
using Puya.Data;
using Puya.Logging;
using System.Security.Claims;

namespace Puya.Net.Tests
{
    public class PuyaLogging
    {
        IDb GetDb()
        {
            var constrProvider = new DefaultConnectionStringProvider();

            constrProvider.SetConnectionString("Server=.\\I2k17;Database=MyDb;Trusted_Connection=True;");

            var db = new SqlServerDb(constrProvider);

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
        IHttpContextAccessor GetHttpContextAccessor(Dictionary<string, string> headers, string method, string username, Claim[] claims)
        {
            var mockAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();

            context.Request.Method = method;

            var claimsPrincipal = GetPrincipal(username, claims);

            if (claimsPrincipal != null)
            {
                context.User = claimsPrincipal;
            }

            if (headers?.Count > 0)
            {
                foreach (var item in headers)
                {
                    context.Request.Headers[item.Key] = item.Value;
                }
            }

            mockAccessor.Setup(_ => _.HttpContext).Returns(context);

            return mockAccessor.Object;
        }
        SqlServerWebLoggerConfig GetDbLoggerConfig(Dictionary<string, string> headers,
                                                    string method,
                                                    string username,
                                                    Claim[] claims,
                                                    string roles = null, string users = null)
        {
            var httpcontextAccessor = GetHttpContextAccessor(headers, method, username, claims);
            var webPolicy = new WebLoggingUserRolePolicy(httpcontextAccessor, roles, users);

            var result = new SqlServerWebLoggerConfig(webPolicy, new StringLogFormatter(new JsonLogDataConverter()));

            result.Level = LogLevel.All;

            return result;
        }
        IList<Log> GetLogs(SqlServerWebLogger logger)
        {
            return logger.Db.ExecuteReaderSql<Log>("select * from " + logger.Config.LogTable);
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

            var exists = db.ExecuteScalerSql(@"
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

            var logs = GetLogs(logger);

            Assert.True(logs.Count == 0);
        }
        [Fact]
        public void Test_DbLogger_No_Roles_And_Users_Policy_AllowNotAuthenticated()
        {
            var db = GetDb();
            var config = GetDbLoggerConfig(null, "GET", null, null);
            
            (config.WebPolicy as WebLoggingUserRolePolicy).AllowNotAuthenticatedUsers = true;
            
            var logger = new SqlServerWebLogger(config, db);

            logger.Clear();

            logger.Info("test");

            var logs = GetLogs(logger);

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

            var logs = GetLogs(logger);

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

            var logs = GetLogs(logger);

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

            var logs = GetLogs(logger);

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

            var logs = GetLogs(logger);

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

            var logs = GetLogs(logger);

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

            var logs = GetLogs(logger);

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

            var count = db.ExecuteScalerSql("select count(*) from " + config.LogTable);

            Assert.True(SafeClrConvert.ToInt(count) == 0);
        }
        [Fact]
        public void Test_DbLogger_MaxDailyLog()
        {
            var db = GetDb();
            var config = GetDbLoggerConfig(null, "GET", null, null);

            (config.WebPolicy as WebLoggingUserRolePolicy).AllowNotAuthenticatedUsers = true;
            config.MaxDailyLog = 3;

            var logger = new SqlServerWebLogger(config, db);

            logger.Clear();

            logger.Info("test");
            logger.Info("test");
            logger.Info("test");
            logger.Info("test");

            var logs = GetLogs(logger);

            Assert.True(logs.Count == 1);
        }
        [Fact]
        public void Test_DbLogger_MaxLog()
        {
            var db = GetDb();
            var config = GetDbLoggerConfig(null, "GET", null, null);

            (config.WebPolicy as WebLoggingUserRolePolicy).AllowNotAuthenticatedUsers = true;
            config.MaxLog = 3;

            var logger = new SqlServerWebLogger(config, db);

            logger.Clear();

            logger.Info("test");
            logger.Info("test");
            logger.Info("test");
            logger.Info("test");

            var logs = GetLogs(logger);

            Assert.True(logs.Count == 1);
        }
    }
}
