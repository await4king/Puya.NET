using System;
using System.Collections.Generic;

namespace Puya.Service
{
    public class LogList : List<Log>, ICloneable
    {
        public object Clone()
        {
            var result = new LogList();

            foreach (var item in this)
            {
                result.Add(item.Clone());
            }

            return result;
        }
    }
}
