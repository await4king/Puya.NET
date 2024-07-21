using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Puya.Extensions;

namespace Puya.Sms
{
    public class SmsLoggerSqlServer : ISmsLogger
    {
        public string ConnectionString { get; set; }
        SqlCommand GetCommand(SqlConnection con, SmsLog log)
        {
            var cmd = new SqlCommand();

            cmd.Connection = con;
            cmd.CommandText = $@"
insert into SmsLog
(
    Topic,
    MobileNo,
    Message,
    Success,
    Response,
    Data,
    Error
)
values
(
    @Topic,
    @MobileNo,
    @Message,
    @Success,
    @Response,
    @Data,
    @Error
)";
            cmd.CommandType = System.Data.CommandType.Text;

            cmd.Parameters.AddWithValue("@Topic", log.Topic == null ? DBNull.Value : (object)log.Topic);
            cmd.Parameters.AddWithValue("@MobileNo", log.MobileNo == null ? DBNull.Value : (object)log.MobileNo);
            cmd.Parameters.AddWithValue("@Message", log.Message == null ? DBNull.Value : (object)log.Message);
            cmd.Parameters.AddWithValue("@Success", log.Success == null ? DBNull.Value : (object)log.Success);
            cmd.Parameters.AddWithValue("@Response", log.Response == null ? DBNull.Value : (object)JsonConvert.SerializeObject(log.Response));
            cmd.Parameters.AddWithValue("@Data", log.Data == null ? DBNull.Value : (object)JsonConvert.SerializeObject(log.Data));
            cmd.Parameters.AddWithValue("@Error", log.Error == null ? DBNull.Value : (object)JsonConvert.SerializeObject(new { Message = log.Error.ToString("\n"), StackTrace = log.Error.StackTrace }));

            return cmd;
        }
        public void Log(SmsLog log)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                using (var cmd = GetCommand(con, log))
                {
                    con.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public async Task LogAsync(SmsLog log, CancellationToken cancellation)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                using (var cmd = GetCommand(con, log))
                {
                    await con.OpenAsync(cancellation);

                    await cmd.ExecuteNonQueryAsync(cancellation);
                }
            }
        }
    }
}
