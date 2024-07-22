using Puya.Data;
using System.Data;

namespace Puya.Service
{
    public class ReportDataSchemaItem
    {
        public string Name { get; set; }
        public string SqlType { get; set; }
        public string JsType { get; set; }
        public string Type { get; set; }
        public int Size { get; set; }
        public ReportDataSchemaItem()
        { }
        public ReportDataSchemaItem(string name, string sqlType, string jsType, string type, int size = 0)
        {
            Name = name;
            SqlType = sqlType;
            JsType = jsType;
            Type = type;
            Size = size;
        }
        public ReportDataSchemaItem(IDataReader reader, int index)
        {
            if (!reader.IsClosed)
            {
                Name = reader.GetName(index);
                SqlType = reader.GetDataTypeName(index);
                Type = reader.GetFieldType(index).Name;
                JsType = DbHelper.GetJsTypeOfSqlType(SqlType);
            }
        }
    }
}
