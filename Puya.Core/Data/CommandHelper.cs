using Puya.Conversion;
using Puya.Extensions;
using System.Collections.Generic;
using System.Data;

namespace Puya.Data
{
    public static class CommandHelper
    {
        public static CommandOutputParameter Result(int? size = null)
        {
            return SqlDbType.VarChar.Output(size ?? 2000);
        }
        public static CommandOutputParameter Status(int? size = null)
        {
            return SqlDbType.VarChar.Output(size ?? 2000);
        }
        public static CommandOutputParameter Field(int? size = null)
        {
            return SqlDbType.VarChar.Output(size ?? 100);
        }
        public static CommandOutputParameter Message(int? size = null)
        {
            return SqlDbType.NVarChar.Output(size ?? 300);
        }
        public static CommandInputOutputParameter Page(object arg = null)
        {
            var result = CommandParameter.InputOutput(SqlDbType.Int, "SqlDbType");

            if (arg != null)
            {
                var argType = arg.GetType();

                if (argType.IsNumeric())
                {
                    result.Value = SafeClrConvert.ToInt(arg);
                }
                else
                if (argType.IsDictionary<string, object>())
                {
                    var dic = arg as IDictionary<string, object>;
                    var value = dic.ContainsKey("Page") ? dic["Page"] : dic.ContainsKey("page") ? dic["page"] : null;

                    if (value != null)
                    {
                        result.Value = SafeClrConvert.ToInt(value);
                    }
                    else
                    {
                        var model = dic.ContainsKey("Model") ? dic["Model"] : dic.ContainsKey("model") ? dic["model"] : null;

                        if (model != null && model.GetType().IsDictionary<string, object>())
                        {
                            var modelDic = model as IDictionary<string, object>;

                            value = modelDic.ContainsKey("Page") ? modelDic["Page"] : modelDic.ContainsKey("page") ? modelDic["page"] : null;

                            if (value != null)
                            {
                                result.Value = SafeClrConvert.ToInt(value);
                            }
                        }
                    }
                }
            }

            return result;
        }
        public static CommandInputOutputParameter PageSize(object arg = null)
        {
            var result = CommandParameter.InputOutput(SqlDbType.Int, "SqlDbType");

            if (arg != null)
            {
                var argType = arg.GetType();

                if (argType.IsNumeric())
                {
                    result.Value = SafeClrConvert.ToInt(arg);
                }
                else
                if (argType.IsDictionary<string, object>())
                {
                    var dic = arg as IDictionary<string, object>;
                    var value = dic.ContainsKey("PageSize") ? dic["PageSize"] : dic.ContainsKey("pagesize") ? dic["pagesize"] : null;

                    if (value != null)
                    {
                        result.Value = SafeClrConvert.ToInt(value);
                    }
                    else
                    {
                        var model = dic.ContainsKey("Model") ? dic["Model"] : dic.ContainsKey("model") ? dic["model"] : null;

                        if (model != null && model.GetType().IsDictionary<string, object>())
                        {
                            var modelDic = model as IDictionary<string, object>;

                            value = modelDic.ContainsKey("PageSize") ? modelDic["PageSize"] : modelDic.ContainsKey("pagesize") ? modelDic["pagesize"] : null;

                            if (value != null)
                            {
                                result.Value = SafeClrConvert.ToInt(value);
                            }
                        }
                    }
                }
            }

            return result;
        }
        public static CommandOutputParameter RecordCount()
        {
            return CommandParameter.Output(SqlDbType.Int, "SqlDbType");
        }
        public static CommandOutputParameter PageCount()
        {
            return CommandParameter.Output(SqlDbType.Int, "SqlDbType");
        }
    }
}
