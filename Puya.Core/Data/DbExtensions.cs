using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Puya.Extensions;
using Puya.Reflection;
using Puya.Collections;
using Puya.Service;
using System.Threading.Tasks;
using System.Threading;

namespace Puya.Data
{
	public static partial class DbExecuteExtensions
	{
		public static void SetContextInfo(this IDbContextInfoProvider contextInfo, IDictionary<string, object> data)
        {
			if (data != null && data.Count > 0)
			{
				var sb = new StringBuilder();

				foreach (var item in data)
				{
					sb.Append($"{(sb.Length == 0 ? "": ",")}{item.Key}={item.Value}");
				}

				contextInfo.SetContextInfo(sb.ToString());
			}
        }
		public static void SetContextInfoAsCsv(this IDbContextInfoProvider contextInfo, object data)
		{
			if (data != null)
			{
				var props = ReflectionHelper.GetPublicInstanceReadableProperties(data.GetType());

				if (props != null && props.Length > 0)
				{
					var sb = new StringBuilder();

					foreach (var prop in props)
					{
						sb.Append($"{(sb.Length == 0 ? "" : ",")}{prop.Name}={prop.GetValue(data)}");
					}

					contextInfo.SetContextInfo(sb.ToString());
				}
			}
		}
		public static void SetContextInfoAsJson(this IDbContextInfoProvider contextInfo, object data)
		{
			if (data != null)
			{
				contextInfo.SetContextInfo(JsonConvert.SerializeObject(data));
			}
		}
        public static async Task<IList<DynamicModel>> ExecuteReaderCommandDynamicAsync(this IDb db, string sproc, object args, bool async, CancellationToken cancellation)
        {
            IList<DynamicModel> result;

            if (async)
            {
                result = await db.ExecuteReaderCommandAsync(sproc, (reader) =>
                {
                    var item = new DynamicModel();

                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        item.Add(reader.GetName(i), reader.GetValue(i));
                    }

                    return item;
                }, args, cancellation);
            }
            else
            {
                result = db.ExecuteReaderCommand(sproc, (reader) =>
                {
                    var item = new DynamicModel();

                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        item.Add(reader.GetName(i), reader.GetValue(i));
                    }

                    return item;
                }, args);
            }

            return result;
        }
        public static async Task<IList<DynamicModel>> ExecuteReaderSqlDynamicAsync(this IDb db, string query, object args, bool async, CancellationToken cancellation)
        {
            IList<DynamicModel> result;

            if (async)
            {
                result = await db.ExecuteReaderSqlAsync(query, (reader) =>
                {
                    var item = new DynamicModel();

                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        item.Add(reader.GetName(i), reader.GetValue(i));
                    }

                    return item;
                }, args, cancellation);
            }
            else
            {
                result = db.ExecuteReaderSql(query, (reader) =>
                {
                    var item = new DynamicModel();

                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        item.Add(reader.GetName(i), reader.GetValue(i));
                    }

                    return item;
                }, args);
            }

            return result;
        }
        public static async Task<ReportResponse> ExecuteReportCommand(this IDb db, string sproc, object args, bool async, CancellationToken cancellation)
        {
            var count = 0;
            var schema = new List<ReportDataSchemaItem>();
            var response = new ReportResponse();

            response.Data = new ReportData();

            if (async)
            {
                response.Data.Items = await db.ExecuteReaderCommandAsync(sproc, reader =>
                {
                    var items = new List<object>();

                    if (count++ == 0)
                    {
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            schema.Add(new ReportDataSchemaItem(reader, i));
                        }
                    }

                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        items.Add(reader[i]);
                    }

                    return items;
                }, args, cancellation);
            }
            else
            {
                response.Data.Items = db.ExecuteReaderCommand(sproc, reader =>
                {
                    var items = new List<object>();

                    if (count++ == 0)
                    {
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            schema.Add(new ReportDataSchemaItem(reader, i));
                        }
                    }

                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        items.Add(reader[i]);
                    }

                    return items;
                }, args);
            }

            response.Data.Schema = schema;

            response.Success = true;

            return response;
        }
        public static async Task<ReportResponse> ExecuteReportSql(this IDb db, string sproc, object args, bool async, CancellationToken cancellation)
        {
            var count = 0;
            var schema = new List<ReportDataSchemaItem>();
            var response = new ReportResponse();

            response.Data = new ReportData();

            if (async)
            {
                response.Data.Items = await db.ExecuteReaderSqlAsync(sproc, reader =>
                {
                    var items = new List<object>();

                    if (count++ == 0)
                    {
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            schema.Add(new ReportDataSchemaItem(reader, i));
                        }
                    }

                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        items.Add(reader[i]);
                    }

                    return items;
                }, args, cancellation);
            }
            else
            {
                response.Data.Items = db.ExecuteReaderSql(sproc, reader =>
                {
                    var items = new List<object>();

                    if (count++ == 0)
                    {
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            schema.Add(new ReportDataSchemaItem(reader, i));
                        }
                    }

                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        items.Add(reader[i]);
                    }

                    return items;
                }, args);
            }

            response.Data.Schema = schema;

            response.Success = true;

            return response;
        }

        public static async Task<bool> RecordExistsSql(this IDb db, string query, object args, bool async)
        {
            object id;

            if (async)
            {
                id = await db.ExecuteScalerSqlAsync(query, args);
            }
            else
            {
                id = db.ExecuteScalerSql(query, args);
            }

            var result = !(id == null || DBNull.Value.Equals(id));

            return result;
        }
        public static async Task<bool> RecordExistsCommand(this IDb db, string query, object args, bool async)
        {
            object id;

            if (async)
            {
                id = await db.ExecuteScalerCommandAsync(query, args);
            }
            else
            {
                id = db.ExecuteScalerCommand(query, args);
            }

            var result = !(id == null || DBNull.Value.Equals(id));

            return result;
        }
        public static async Task<bool> RecordExistsSql(this IDb db, string query, object args, object value, bool async)
        {
            object id;

            if (async)
            {
                id = await db.ExecuteScalerSqlAsync(query, args);
            }
            else
            {
                id = db.ExecuteScalerSql(query, args);
            }

            var result = !(id == null || DBNull.Value.Equals(id) || id != value);

            return result;
        }
        public static async Task<bool> RecordExistsCommand(this IDb db, string query, object args, object value, bool async)
        {
            object id;

            if (async)
            {
                id = await db.ExecuteScalerCommandAsync(query, args);
            }
            else
            {
                id = db.ExecuteScalerCommand(query, args);
            }

            var result = !(id == null || DBNull.Value.Equals(id) || id != value);

            return result;
        }
        public static bool RecordExists(this IDb db, string query, object args)
        {
            var id = db.ExecuteScalerSql(query, args);
            var result = !(id == null || DBNull.Value.Equals(id));

            return result;
        }
        public static bool RecordExists(this IDb db, string query, object args, object value)
        {
            var id = db.ExecuteScalerSql(query, args);
            var result = !(id == null || DBNull.Value.Equals(id) || id != value);

            return result;
        }
    }
}
