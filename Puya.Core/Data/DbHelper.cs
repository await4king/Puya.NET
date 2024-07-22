namespace Puya.Data
{
    public class DbHelper
    {
        public static string GetJsTypeOfSqlType(string sqlType)
        {
            string result;

            sqlType = sqlType.ToLower();

            switch (sqlType)
            {
                case "bigint": result = "number"; break;
                case "binary": result = "string"; break;
                case "bit": result = "boolean"; break;
                case "char": result = "string"; break;
                case "date": result = "date"; break;
                case "datetime": result = "date"; break;
                case "datetime2": result = "date"; break;
                case "datetimeoffset": result = "date"; break;
                case "decimal": result = "number"; break;
                case "float": result = "number"; break;
                case "image": result = "string"; break;
                case "int": result = "number"; break;
                case "money": result = "number"; break;
                case "nchar": result = "string"; break;
                case "ntext": result = "string"; break;
                case "nvarchar": result = "string"; break;
                case "real": result = "number"; break;
                case "smalldatetime": result = "date"; break;
                case "smallint": result = "number"; break;
                case "smallmoney": result = "number"; break;
                case "tinyint": result = "number"; break;
                case "uniqueidentifier": result = "string"; break;
                case "varbinary": result = "string"; break;
                case "varchar": result = "string"; break;
                case "xml": result = "string"; break;
                default:
                    // geography
                    // geometry
                    // hierarchyid
                    // sql_variant
                    // text
                    // time
                    // timestamp
                    result = "string"; break;
            }

            return result;
        }
    }
}
