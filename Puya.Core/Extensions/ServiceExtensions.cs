using Puya.Service;
using Puya.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Puya.Base;
using System.Data;
using System.Globalization;

namespace Puya.Extensions
{
    public static class ServiceExtensions
    {
        public static async Task<ReportData<List<object>>> GetSchemaBasedList(this IDb db, string query, bool sproc, object args, bool async, CancellationToken cancellation)
        {
            var count = 0;
            var schema = new List<ReportDataSchemaItem>();
            var response = new ReportData<List<object>>();

            if (async)
            {
                if (sproc)
                {
                    response.Items = await db.ExecuteReaderCommandAsync(query, reader => MapReader(reader, schema, ref count), args, cancellation);
                }
                else
                {
                    response.Items = await db.ExecuteReaderSqlAsync(query, reader => MapReader(reader, schema, ref count), args, cancellation);
                }
            }
            else
            {
                if (sproc)
                {
                    response.Items = db.ExecuteReaderCommand(query, reader => MapReader(reader, schema, ref count), args);
                }
                else
                {
                    response.Items = db.ExecuteReaderSql(query, reader => MapReader(reader, schema, ref count), args);
                }
            }

            response.Schema = schema;

            return response;
        }
        public static List<object> MapReader(this IDataReader reader, List<ReportDataSchemaItem> schema, ref int count)
        {
            var items = new List<object>();

            if (count++ == 0)
            {
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    schema.Add(new ReportDataSchemaItem(reader, i));

                    if (reader.GetFieldType(i) == TypeHelper.TypeOfDateTime)
                    {
                        schema.Add(new ReportDataSchemaItem(reader.GetName(i) + "Fa", "varchar", "string", "String", 20));
                        schema.Add(new ReportDataSchemaItem(reader.GetName(i) + "Time", "varchar", "string", "String", 8));
                    }
                }
            }

            for (var i = 0; i < reader.FieldCount; i++)
            {
                var value = reader.GetValue(i);

                if (reader.IsDBNull(i))
                {
                    items.Add(null);
                }
                else
                {
                    items.Add(value);
                }

                if (reader.GetFieldType(i) == TypeHelper.TypeOfDateTime)
                {
                    if (reader.IsDBNull(i))
                    {
                        items.Add(null);
                        items.Add(null);
                    }
                    else
                    {
                        var d = (DateTime)value;
                        var pc = new PersianCalendar();

                        items.Add(d.ToPersian());
                        items.Add($"{pc.GetHour(d)}:{pc.GetMinute(d)}:{pc.GetSecond(d)}");
                    }
                }
            }

            return items;
        }
    }
}
