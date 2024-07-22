using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.Service
{
    public class ReportData
    {
        public IList<List<object>> Items { get; set; }
        public List<ReportDataSchemaItem> Schema { get; set; }
    }
    public class ReportData<T>
    {
        public IList<T> Items { get; set; }
        public List<ReportDataSchemaItem> Schema { get; set; }
    }
}
