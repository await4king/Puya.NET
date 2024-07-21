using System;
using Puya.Service;
using System.Threading;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Puya.Sms
{
    public class SqlServerSmsService : SmsServiceBase<SqlServerSmsServiceConfig>
    {
        public SqlServerSmsService(SqlServerSmsServiceConfig config, ISmsLogger logger) : base(config, logger)
        { }
        public SqlServerSmsService(ISmsLogger logger) : base(logger)
        { }
        public SqlServerSmsService(SqlServerSmsServiceConfig config) : base(config)
        { }
        public SqlServerSmsService()
        { }
        SqlCommand GetCommand(SqlConnection con, string mobile, string message)
        {
            var result = new SqlCommand();

            result.Connection = con;
            result.CommandType = System.Data.CommandType.Text;
            result.CommandText = "insert into dbo.Sms(mobile, message) values (@mobile, @message)";

            result.Parameters.AddWithValue("@mobile", mobile == null ? DBNull.Value : (object)mobile);
            result.Parameters.AddWithValue("@message", message == null ? DBNull.Value : (object)message);

            return result;
        }
        protected override SendResponse SendInternal(string mobile, string message)
        {
            var result = new SendResponse();

            if (!string.IsNullOrEmpty(Config.ConnectionString))
            {
                using (var con = new SqlConnection(Config.ConnectionString))
                {
                    using (var cmd = GetCommand(con, mobile, message))
                    {
                        con.Open();

                        cmd.ExecuteNonQuery();
                    }
                }

                result.Succeeded();
            }
            else
            {
                result.SetStatus("NoConnectionString");

                Warn("Missing connectionString");
            }

            return result;
        }

        protected override async Task<SendResponse> SendAsyncInternal(string mobile, string message, CancellationToken cancellation)
        {
            var result = new SendResponse();

            if (!string.IsNullOrEmpty(Config.ConnectionString))
            {
                using (var con = new SqlConnection(Config.ConnectionString))
                {
                    using (var cmd = GetCommand(con, mobile, message))
                    {
                        await con.OpenAsync(cancellation);

                        await cmd.ExecuteNonQueryAsync(cancellation);
                    }
                }

                result.Succeeded();
            }
            else
            {
                result.SetStatus("NoConnectionString");

                Warn("Missing connectionString");
            }

            return result;
        }
    }
}
