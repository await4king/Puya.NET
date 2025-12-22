using Puya.Base;
using Puya.Conversion;
using Puya.Data;
using Puya.Reflection;
using Puya.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        public static void Finalize(this ServiceResponse response, object args)
        {
            if (response != null && args != null)
            {
                var props = ReflectionHelper.GetPublicInstanceReadableProperties(args.GetType());
                var resultProp = props.FirstOrDefault(p => p.Name.Equalz("Result"));
                var fieldProp = props.FirstOrDefault(p => p.Name.Equalz("Field"));
                var messageProp = props.FirstOrDefault(p => p.Name.Equalz("Message"));

                if (resultProp != null)
                {
                    var resultParam = resultProp.GetValue(args) as CommandParameter;

                    if (resultParam != null)
                    {
                        response.Finalize(resultParam);
                    }
                }
                else
                {
                    var statusProp = props.FirstOrDefault(p => p.Name.Equalz("Status"));

                    if (statusProp != null)
                    {
                        var statusParam = statusProp.GetValue(args) as CommandParameter;

                        if (statusParam != null)
                        {
                            response.Finalize(statusParam);
                        }
                    }
                }

                if (fieldProp != null)
                {
                    var fieldParam = fieldProp.GetValue(args) as CommandParameter;

                    if (fieldParam != null)
                    {
                        response.Info = SafeClrConvert.ToString(fieldParam.Value);
                    }
                }

                if (messageProp != null)
                {
                    var messageParam = messageProp.GetValue(args) as CommandParameter;

                    if (messageParam != null)
                    {
                        response.Message = SafeClrConvert.ToString(messageParam.Value);
                    }
                }
            }
        }
        public static void Finalize(this ServiceResponse response, CommandParameter resultParam)
        {
            if (response != null && resultParam != null)
            {
                var result = SafeClrConvert.ToString(resultParam.Value);

                if (result.IsJsonObject())
                {
                    var res = result.SafeDeserialize<ServiceResponse>();

                    if (res != null)
                    {
                        response.Copy(res);
                    }
                    else
                    {
                        response.SetStatus("ProblematicResult");
                        response.Info = result;
                    }
                }
                else
                {
                    response.SetStatus(result);

                    if (string.IsNullOrEmpty(response.Status))
                    {
                        response.Succeeded();
                    }
                }
            }
            else if (response != null)
            {
                response.SetStatus("NoResultParam");
            }
        }
        public static void GetPagination<T>(this PagingResult<T> pagination, IDictionary<string, object> args)
        {
            pagination.Page = args.GetInt("Page");
            pagination.PageSize = args.GetInt("PageSize");
            pagination.PageCount = args.GetInt("PageCount");
            pagination.RecordCount = args.GetInt("RecordCount");
        }
    }
}
