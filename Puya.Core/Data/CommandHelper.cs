using Newtonsoft.Json;
using Puya.Collections;
using Puya.Data;
using Puya.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Puya.Data
{
    public static class CommandHelper
    {
        public static CommandOutputParameter Result(int? size = null)
        {
            return SqlDbType.VarChar.Output(size ?? 100);
        }
        public static CommandOutputParameter Field(int? size = null)
        {
            return SqlDbType.VarChar.Output(size ?? 100);
        }
        public static CommandOutputParameter Message(int? size = null)
        {
            return SqlDbType.NVarChar.Output(size ?? 300);
        }
    }
}
