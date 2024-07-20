using Puya.Base;
using Puya.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Data
{
    public class CommandParameter
    {
        public string Name { get; set; }
        private object _value;
        public string ListSeparator { get; set; }
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value == null ||  DBNull.Value.Equals(value))
                {
                    _value = DBNull.Value;
                }
                else
                {
                    var valueType = value.GetType();

                    if (valueType.IsEnumerable() && valueType != TypeHelper.TypeOfString)
                    {
                        var e = value as System.Collections.IEnumerable;

                        if (e != null)
                        {
                            var sb = new StringBuilder();

                            foreach (var x in e)
                            {
                                sb.Append((sb.Length == 0 ? "" : ListSeparator) + x.ToString());
                            }

                            _value = sb.ToString();
                        }
                        else
                        {
                            _value = value.ToString();
                        }
                    }
                    else
                    {
                        _value = value;
                    }
                }
            }
        }
        public string TypeProp { get; set; }
        public dynamic Type { get; set; }
        public byte? Scale { get; set; }
        public byte? Precision { get; set; }
        public ParameterDirection Direction { get; set; }
        public int? Size { get; set; }
        public CommandParameter()
        {
            ListSeparator = ",";
        }
        public static CommandOutputParameter Output(dynamic type, int? size = null)
        {
            return new CommandOutputParameter { Type = type, Size = size };
        }
        public static CommandOutputParameter Output(string name, dynamic type, int? size = null)
        {
            return new CommandOutputParameter { Name = name, Type = type, Size = size };
        }
        public static CommandOutputParameter Output(dynamic type, string typeProp, int? size = null)
        {
            return new CommandOutputParameter { Type = type, TypeProp = typeProp, Size = size };
        }
        public static CommandOutputParameter Output(string name, string typeProp, dynamic type, int? size = null)
        {
            return new CommandOutputParameter { Name = name, TypeProp = typeProp, Type = type, Size = size };
        }
        public static CommandInputOutputParameter InputOutput(dynamic type, int? size = null)
        {
            return new CommandInputOutputParameter { Type = type, Size = size };
        }
        public static CommandInputOutputParameter InputOutput(string name, dynamic type, int? size = null)
        {
            return new CommandInputOutputParameter { Name = name, Type = type, Size = size };
        }
        public static CommandInputOutputParameter InputOutput(dynamic type, string typeProp, int? size = null)
        {
            return new CommandInputOutputParameter { Type = type, TypeProp = typeProp, Size = size };
        }
        public static CommandInputOutputParameter InputOutput(string name, string typeProp, dynamic type, int? size = null)
        {
            return new CommandInputOutputParameter { Name = name, TypeProp = typeProp, Type = type, Size = size };
        }
        public override string ToString()
        {
            return Value == null || DBNull.Value.Equals(Value) ? "" : Value.ToString();
        }
    }
    public class CommandOutputParameter: CommandParameter
    {
        public CommandOutputParameter()
        {
            Direction = ParameterDirection.Output;
            Type = DbType.Object;
        }
    }
    public class CommandInputOutputParameter : CommandParameter
    {
        public CommandInputOutputParameter()
        {
            Direction = ParameterDirection.InputOutput;
            Type = DbType.Object;
        }
    }
}
