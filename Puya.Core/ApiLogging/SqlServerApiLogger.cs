using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.ApiLogging
{
    public class SqlServerApiLogger : BaseApiLogger
    {
        public string ConnectionString { get; set; }
        public SqlServerApiLogger(ApiClient client, ApiServer server, string connectionString): base(client, server)
        {
            ConnectionString = connectionString;
        }
        public SqlServerApiLogger(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public string logTable;
        public string LogTable
        {
            get
            {
                if (string.IsNullOrEmpty(logTable))
                {
                    logTable = "ApiLog";
                }

                return logTable;
            }
            set
            {
                logTable = value;
            }
        }
        public string query;
        public string Query
        {
            get
            {
                if (string.IsNullOrEmpty(query))
                {
                    query = $@"insert into {LogTable}
                                (
                                    Direction,
                                    Client,
                                    Server,
                                    Request,
                                    Response
                                )
                            values
                                (
                                    @Direction,
                                    @Client,
                                    @Server,
                                    @Request,
                                    @Response
                                );
                            select scope_identity()
                            ";
                }

                return query;
            }
            set
            {
                query = value;
            }
        }
        protected virtual SqlCommand GetCommand(ApiLog log, SqlConnection connection)
        {
            var cmd = new SqlCommand(Query, connection);

            cmd.CommandType = System.Data.CommandType.Text;

            cmd.Parameters.AddWithValue("@Direction", Serialize(log.Direction));
            cmd.Parameters.AddWithValue("@Client", Serialize(log.Client));
            cmd.Parameters.AddWithValue("@Server", Serialize(log.Server));
            cmd.Parameters.AddWithValue("@Client", Serialize(log.Request));
            cmd.Parameters.AddWithValue("@Client", Serialize(log.Response));

            return cmd;
        }
        public override void Log(ApiLog log)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                using (var cmd = GetCommand(log, con))
                {
                    con.Open();

                    var id = cmd.ExecuteScalar();

                    con.Close();

                    log.Id = System.Convert.ToInt32(id);
                }
            }
        }

        public override async Task LogAsync(ApiLog log, CancellationToken cancellation)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                using (var cmd = GetCommand(log, con))
                {
                    await con.OpenAsync(cancellation);

                    var id = await cmd.ExecuteScalarAsync(cancellation);

                    con.Close();

                    log.Id = System.Convert.ToInt32(id);
                }
            }
        }
    }
}
