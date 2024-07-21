using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Puya.Extensions;
using Puya.Base;
using Puya.Reflection;

namespace Puya.Data
{
    internal class CommandArgument
    {
        internal string Name { get; set; }
        internal object Value { get; set; }
        internal Type Type { get; set; }
        internal int? Size { get; set; }
        internal PropertyInfo Property { get; set; }
    }
    public static partial class DbConnectionExtensions
    {
        private static IEnumerator<CommandArgument> GetCommandParameterEnumeratorFromDictionary(IDictionary<string, object> data)
        {
            foreach (var item in data)
            {
                yield return new CommandArgument { Name = item.Key, Value = item.Value, Type = item.Value?.GetType() };
            }
        }
        private static IEnumerator<CommandArgument> GetCommandParameterEnumeratorFromEnumerable(System.Collections.IEnumerable data)
        {
            foreach (var item in data)
            {
                yield return new CommandArgument { Value = item, Type = item?.GetType() };
            }
        }
        private static IEnumerator<CommandArgument> GetCommandParameterEnumeratorFromObject(object data)
        {
            if (data != null)
            {
                var props = ReflectionHelper.GetPublicInstanceReadableProperties(data.GetType());

                if (props?.Length > 0)
                {
                    foreach (var prop in props)
                    {
                        var sizeAttr = prop.GetCustomAttribute<SizeAttribute>();
                        var size = sizeAttr?.Value;

                        if (prop.CustomAttributes.Count(a => a.AttributeType == typeof(IgnoreAttribute)) == 0)
                        {
                            var value = prop.GetValue(data);

                            yield return new CommandArgument { Name = prop.Name, Value = value, Type = value == null || DBNull.Value.Equals(value) ? prop.PropertyType : value.GetType(), Size = size, Property = prop };
                        }
                    }
                }
            }
        }
        public static DbCommand CreateCommand(this DbConnection con, string text, CommandType type, object parameters, bool autoNullEmptyStrings = false)
        {
            var dictionaryParams = parameters as IDictionary<string, object>;

            if (dictionaryParams != null)
            {
                return CreateCommand(con, text, type, GetCommandParameterEnumeratorFromDictionary(dictionaryParams), autoNullEmptyStrings);
            }

            var enumerable = parameters as System.Collections.IEnumerable;

            if (enumerable != null)
            {
                return CreateCommand(con, text, type, GetCommandParameterEnumeratorFromEnumerable(enumerable), autoNullEmptyStrings);
            }

            return CreateCommand(con, text, type, GetCommandParameterEnumeratorFromObject(parameters));
        }
        private static DbCommand CreateCommand(this DbConnection con, string text, CommandType type, IEnumerator<CommandArgument> parameters, bool autoNullEmptyStrings = false)
        {
            var result = con.CreateCommand();

            result.CommandText = text;
            result.CommandType = type;

            if (parameters != null)
            {
                while (parameters.MoveNext())
                {
                    var param = parameters.Current;
                    var cmdParam = result.CreateParameter();
                    var cmdParamType = cmdParam.GetType();

                    cmdParam.Direction = ParameterDirection.Input;

                    if (!string.IsNullOrEmpty(param.Name))
                    {
                        if (param.Name[0] == '@')
                        {
                            cmdParam.ParameterName = param.Name;
                        }
                        else
                        {
                            cmdParam.ParameterName = '@' + param.Name;
                        }
                    }

                    do
                    {
                        var value = param.Value;
                        var valueType = param.Type;

                        var dbParam = value as DbParameter;

                        if (dbParam != null)
                        {
                            if (string.IsNullOrEmpty(dbParam.ParameterName))
                            {
                                dbParam.ParameterName = cmdParam.ParameterName;
                            }

                            cmdParam = dbParam;

                            break;
                        }

                        if (valueType != null && !valueType.IsNullableOrSimpleType())
                        {
                            var isCommandParameter = false;
                            var outputAttr = param.Property?.GetCustomAttribute<OutputAttribute>();

                            if (outputAttr != null)
                            {
                                value = new CommandParameter { Type = outputAttr.Type, TypeProp = outputAttr.TypeProp, Size = outputAttr.Size };
                                isCommandParameter = true;
                            }
                            else
                            {
                                if (valueType == DataHelper.TypeOfCommandParameter || valueType.DescendsFrom(DataHelper.TypeOfCommandParameter))
                                {
                                    value = value as CommandParameter ?? new CommandInputOutputParameter();
                                    isCommandParameter = true;
                                }
                            }

                            if (isCommandParameter)
                            {
                                var _param = value as CommandParameter;

                                if (!string.IsNullOrEmpty(_param.Name))
                                {
                                    cmdParam.ParameterName = _param.Name;

                                    if (cmdParam.ParameterName[0] != '@')
                                    {
                                        cmdParam.ParameterName = '@' + cmdParam.ParameterName;
                                    }
                                }

                                if (_param.Value == null || (autoNullEmptyStrings && string.IsNullOrEmpty(_param.Value.ToString())))
                                    cmdParam.Value = DBNull.Value;
                                else
                                    cmdParam.Value = _param.Value;

                                cmdParam.Direction = _param.Direction;

                                if (_param.Scale.HasValue)
                                {
                                    cmdParam.Scale = _param.Scale.Value;
                                }
                                if (_param.Precision.HasValue)
                                {
                                    cmdParam.Precision = _param.Precision.Value;
                                }
                                if (_param.Size.HasValue)
                                {
                                    cmdParam.Size = _param.Size.Value;
                                }

                                if (!string.IsNullOrEmpty(_param.TypeProp))
                                {
                                    var typeProp = cmdParamType.GetProperty(_param.TypeProp);

                                    if (typeProp != null)
                                    {
                                        typeProp.SetValue(cmdParam, _param.Type);
                                    }
                                }
                                else
                                {
                                    cmdParam.DbType = _param.Type;
                                }

                                break;
                            }
                        }

                        if (param.Size.HasValue)
                        {
                            cmdParam.Size = param.Size.Value;
                        }
                        else
                        {
                            if (valueType == TypeHelper.TypeOfString)
                            {
                                cmdParam.Size = value == null ? -1 : value.ToString().Length;
                            }
                        }

                        if (value == null || DBNull.Value.Equals(value) || (string.IsNullOrEmpty(value.ToString()) && autoNullEmptyStrings))
                        {
                            cmdParam.Value = DBNull.Value;

                            break;
                        }

                        if (valueType?.IsNullableOrBasicType() ?? false)
                        {
                            cmdParam.Value = value;

                            break;
                        }

                        if (valueType?.IsEnumerable() ?? false)
                        {
                            var joinAttr = param.Property?.GetCustomAttribute<JoinAttribute>();

                            if (joinAttr == null)
                            {
                                cmdParam.Value = value;
                            }
                            else
                            {
                                var e = value as System.Collections.IEnumerable;

                                if (e != null)
                                {
                                    var separator = joinAttr.Separator;

                                    if (string.IsNullOrEmpty(separator))
                                    {
                                        separator = ",";
                                    }

                                    var sb = new StringBuilder();

                                    foreach (var x in e)
                                    {
                                        sb.Append((sb.Length == 0 ? "" : separator) + x?.ToString());
                                    }

                                    cmdParam.Value = sb.ToString();
                                }
                                else
                                {
                                    cmdParam.Value = value;
                                }
                            }

                            break;
                        }

                        cmdParam.Value = value.ToString();
                    }
                    while (false);

                    result.Parameters.Add(cmdParam);
                }
            }

            return result;
        }
    }
}
