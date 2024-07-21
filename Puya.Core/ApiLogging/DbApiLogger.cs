using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Data;

namespace Puya.ApiLogging
{
    public class DbApiLogger : BaseApiLogger
    {
        public DbApiLogger(ApiClient client, ApiServer server, IDb db) : base(client, server)
        {
            Db = db;
        }
        public DbApiLogger(IDb db)
        {
            Db = db;
        }
        public IDb db;
        public IDb Db
        {
            get
            {
                if (db == null)
                {
                    db = new NullDb();
                }

                return db;
            }
            set
            {
                db = value;
            }
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
        public override void Log(ApiLog log)
        {
            var id = Db.ExecuteScalerSql(Query, new
            {
                log.Direction,
                Client = Serialize(log.Client),
                Server = Serialize(log.Server),
                Request = Serialize(log.Request),
                Response = Serialize(log.Response)
            });

            log.Id = System.Convert.ToInt32(id);
        }

        public override async Task LogAsync(ApiLog log, CancellationToken cancellation)
        {
            var id = await Db.ExecuteScalerSqlAsync(Query, new
            {
                log.Direction,
                Client = Serialize(log.Client),
                Server = Serialize(log.Server),
                Request = Serialize(log.Request),
                Response = Serialize(log.Response)
            }, cancellation);

            log.Id = System.Convert.ToInt32(id);
        }
    }
}
