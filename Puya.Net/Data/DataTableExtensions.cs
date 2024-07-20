using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Puya.Extensions;
using Puya.Reflection;

namespace Puya.Data
{
    public static class DataTableExtensions
    {
        public static T ConvertTo<T>(this DataRow row) where T : new()
        {
            var result = new T();
            var type = typeof(T);
            var properties = ReflectionHelper.GetPublicInstanceWritableProperties(type);

            foreach (var property in properties)
            {
                property.SetValue(result, row[property.Name]);
            }

            return result;
        }
        public static List<T> ToList<T>(this DataTable table) where T : new()
        {
            var result = new List<T>();

            foreach (DataRow row in table.Rows)
            {
                result.Add(row.ConvertTo<T>());
            }

            return result;
        }
        public static string Merge(this DataTable table, string cellSeparator = ",", string rowSeparator = "\r\n")
        {
            var sb = new StringBuilder();

            foreach (DataRow row in table.Rows)
            {
                var temp = "";

                for (var i = 0; i < row.Table.Columns.Count; i++)
                {
                    if (temp == "")
                        temp = row[i].ToString();
                    else
                        temp = temp + cellSeparator + row[i].ToString();
                }

                if (sb.Length > 0)
                    sb.Append(rowSeparator + temp);
                else
                    sb.Append(temp);
            }

            var result = sb.ToString();

            return result;
        }
        public static string Merge(this DataTable table, string[] cols, string cellSeparator = ",", string rowSeparator = "\r\n")
        {
            var sb = new StringBuilder();
            var indexes = new List<int>();

            foreach (var col in cols)
            {
                for (var i = 0; i < table.Columns.Count; i++)
                {
                    if (String.Compare(table.Columns[i].ColumnName, col, true) == 0)
                    {
                        indexes.Add(i);
                    }
                }
            }

            if (indexes.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    var temp = "";

                    foreach (var index in indexes)
                    {
                        if (temp == "")
                            temp = row[index].ToString();
                        else
                            temp = temp + cellSeparator + row[index].ToString();
                    }

                    if (sb.Length > 0)
                        sb.Append(rowSeparator + temp);
                    else
                        sb.Append(temp);
                }
            }

            var result = sb.ToString();

            return result;
        }
        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {
            var result = new DataTable();
            var type = typeof(T);
            var props = ReflectionHelper.GetPublicInstanceReadableProperties(typeof(T));

            foreach (var prop in props)
            {
                result.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (var item in list)
            {
                var row = new object[props.Length];
                var i = 0;

                foreach (var prop in props)
                {
                    row[i++] = prop.GetValue(item);
                }

                result.Rows.Add(row);
            }

            return result;
        }
        public static DataTable ToDataTable(this IEnumerable list)
        {
            var result = new DataTable();

            PropertyInfo[] props = null;
            Type itemType = null;

            foreach (var item in list)
            {
                if (props == null && item != null)
                {
                    itemType = item.GetType();
                    props = ReflectionHelper.GetPublicInstanceReadableProperties(itemType);

                    foreach (var prop in props)
                    {
                        result.Columns.Add(prop.Name, prop.PropertyType);
                    }
                }

                if (props != null && item != null && item.GetType() == itemType)
                {
                    var row = new object[props.Length];
                    var i = 0;

                    foreach (var prop in props)
                    {
                        row[i++] = prop.GetValue(item);
                    }

                    result.Rows.Add(row);
                }
            }

            return result;
        }
    }
}
