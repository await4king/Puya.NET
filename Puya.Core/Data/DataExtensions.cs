using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Puya.Collections;
using Puya.Conversion;
using Puya.Data;
using Puya.Reflection;

namespace Puya.Extensions
{
    public static class DataExtensions
    {
        public static IDictionary<string, Object> ToDictionary(this IDataParameterCollection prameters)
        {
            var result = new Dictionary<string, Object>();

            foreach (IDbDataParameter p in prameters)
            {
                result.Add(p.ParameterName.Replace("@", ""), p.Value);
            }

            return result;
        }
        public static SqlParameter ToSqlParameter<T>(this T x) where T : class
        {
            var table = new DataTable();
            var t = typeof(T);
            var props = ReflectionHelper.GetPublicInstanceReadableProperties(t);

            foreach (var propInfo in props)
            {
                var name = propInfo.Name;
                var type = propInfo.PropertyType;

                table.Columns.Add(name, type);
            }

            var row = table.NewRow();

            foreach (var propInfo in props)
            {
                var value = propInfo.GetValue(x);
                var name = propInfo.Name;

                row[name] = value;
            }

            table.Rows.Add(row);

            var p = new SqlParameter("@" + typeof(T).ToString(), table);

            p.SqlDbType = SqlDbType.Structured;
            p.TypeName = typeof(T).ToString();

            return p;
        }
        public static SqlDbType ToSqlDbType(int sql_sys_type) // a is a SQL Server 'sys_type_id' from sys.types/sys.systypes table
        {
            SqlDbType result;

            switch (sql_sys_type)
            {
                case 34: result = SqlDbType.Image; break;
                case 35: result = SqlDbType.Text; break;
                case 36: result = SqlDbType.UniqueIdentifier; break;
                case 40: result = SqlDbType.Date; break;
                case 41: result = SqlDbType.Time; break;
                case 42: result = SqlDbType.DateTime2; break;
                case 43: result = SqlDbType.DateTimeOffset; break;
                case 48: result = SqlDbType.TinyInt; break;
                case 52: result = SqlDbType.SmallInt; break;
                case 56: result = SqlDbType.Int; break;
                case 58: result = SqlDbType.SmallDateTime; break;
                case 59: result = SqlDbType.Real; break;
                case 60: result = SqlDbType.Money; break;
                case 61: result = SqlDbType.DateTime; break;
                case 62: result = SqlDbType.Float; break;
                case 99: result = SqlDbType.NText; break;
                case 104: result = SqlDbType.Bit; break;
                case 106: result = SqlDbType.Decimal; break;
                case 122: result = SqlDbType.SmallMoney; break;
                case 127: result = SqlDbType.BigInt; break;             // Also: User-Defined Data Type
                case 165: result = SqlDbType.VarBinary; break;
                case 167: result = SqlDbType.VarChar; break;
                case 173: result = SqlDbType.Binary; break;
                case 175: result = SqlDbType.Char; break;
                case 189: result = SqlDbType.Timestamp; break;
                case 231: result = SqlDbType.NVarChar; break;           // Also: sysname: length will be 128 bytes
                case 239: result = SqlDbType.NChar; break;
                case 241: result = SqlDbType.Xml; break;

                case 98: result = SqlDbType.Variant; break;             // sql_variant
                case 108: result = SqlDbType.Variant; break;            // numeric
                case 240: result = SqlDbType.Udt; break;                // hierarchyid (128), geometry (129), geography (130)
                case 243: result = SqlDbType.Structured; break;        // User-Defined Table Type

                default: throw new ArgumentException("Invalid Sql Server data type");
            }

            return result;
        }
        public static bool GetSprocArgs(this IDb db, string sproc, object @params, DynamicModel customParams, out DynamicModel args)
        {
            var result = false;
            var _args = new DynamicModel();
            var props = @params?.GetType()
                                        .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                                        .Where(x => x.CanRead)
                                        .ToList() ?? new List<PropertyInfo>();
            var _params = @params as DynamicModel;

            try
            {
                db.ExecuteReaderSql(@"
select
	name
from sys.parameters
where object_id = OBJECT_ID(@sp)
order by parameter_id", reader =>
                {
                    var paramName = SafeClrConvert.ToString(reader[0]);

                    if (customParams.ContainsKey(paramName))
                    {
                        _args.Add(paramName, customParams[paramName]);
                    }
                    else
                    if (paramName[0] == '@' && customParams.ContainsKey(paramName.Substring(1)))
                    {
                        _args.Add(paramName, customParams[paramName.Substring(1)]);
                    }
                    else
                    if (_params != null && _params.ContainsKey(paramName))
                    {
                        _args.Add(paramName, _params[paramName]);
                    }
                    else
                    if (_params != null && paramName[0] == '@' && _params.ContainsKey(paramName.Substring(1)))
                    {
                        _args.Add(paramName, _params[paramName.Substring(1)]);
                    }
                    else
                    {
                        var prop = props.FirstOrDefault(x => string.Equals(paramName, "@" + x.Name, StringComparison.OrdinalIgnoreCase));

                        if (prop != null)
                        {
                            _args.Add(paramName, prop.GetValue(@params));
                        }
                        else
                        {
                            _args.Add(paramName, null);
                        }
                    }

                    return (object)null;
                }, new { sp = sproc });

                args = _args;

                result = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString("\n"));

                args = null;
            }

            return result;
        }
    }
}
