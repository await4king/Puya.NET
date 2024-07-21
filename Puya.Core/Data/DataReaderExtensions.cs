//Contributor: Rick Strahl - http://codepaste.net/qqcf4x

using Puya.Base;
using Puya.Conversion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Collections.Concurrent;
using System.Linq;
using Puya.Extensions;

namespace Puya.Data
{
    /// <summary>
    /// Data reader extensions
    /// </summary>
    public static class DataReaderExtensions
    {
        // https://stackoverflow.com/questions/373230/check-for-column-name-in-a-sqldatareader-object
        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Creates a list of a given type from all the rows in a DataReader.
        /// 
        /// Note this method uses Reflection so this isn't a high performance
        /// operation, but it can be useful for generic data reader to entity
        /// conversions on the fly and with anonymous types.
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="reader">An open DataReader that's in position to read</param>
        /// <param name="fieldsToSkip">Optional - comma delimited list of fields that you don't want to update</param>
        /// <param name="piList">
        /// Optional - Cached PropertyInfo dictionary that holds property info data for this object.
        /// Can be used for caching the PropertyInfo structure for multiple operations to speed up
        /// translation. If not passed automatically created.
        /// </param>
        /// <returns></returns>
        public static List<TType> ToList<TType>(this IDataReader reader, Type type = null, string fieldsToSkip = null)
            where TType : new()
        {
            if (reader == null)
                return null;

            var items = new List<TType>();

            while (reader.Read())
            {
                var inst = (object)new TType();

                ConvertTo(reader, ref inst, type, fieldsToSkip);

                items.Add((TType)inst);
            }

            return items;
        }

        /// <summary>
        /// Populates the properties of an object from a single DataReader row using
        /// Reflection by matching the DataReader fields to a public property on
        /// the object passed in. Unmatched properties are left unchanged.
        /// 
        /// You need to pass in a data reader located on the active row you want
        /// to serialize.
        /// 
        /// This routine works best for matching pure data entities and should
        /// be used only in low volume environments where retrieval speed is not
        /// critical due to its use of Reflection.
        /// </summary>
        /// <param name="reader">Instance of the DataReader to read data from. Should be located on the correct record (Read() should have been called on it before calling this method)</param>
        /// <param name="instance">Instance of the object to populate properties on</param>
        /// <param name="fieldsToSkip">Optional - A comma delimited list of object properties that should not be updated</param>
        public static bool ConvertTo(this IDataReader reader, ref object instance, Type type = null, string fieldsToSkip = null)
        {
            if (reader.IsClosed)
            {
                return false;
            }

            var _type = instance == null ? type : instance.GetType();

            if (_type == null)
            {
                instance = reader[0];

                return true;
            }

            if (_type.IsNullableOrBasicType())
            {
                var value = reader[0];

                if (value != null && !DBNull.Value.Equals(value))
                {
                    if (_type == TypeHelper.TypeOfString)
                    {
                        instance = ObjectActivator.Instance.Activate(TypeHelper.TypeOfString, SafeClrConvert.ToString(value).ToCharArray());
                    }
                    else
                    {
                        var _value = value.ConvertTo(_type);

                        if (_value != null)
                        {
                            if (_value.GetType().IsBasicType())
                            {
                                instance = _value;
                            }
                        }
                        else
                        {
                            // last chance.
                            // this is rare to occur though, since Object.ConvertTo() extension method
                            // is smart and able to successfully convert DataReader's current column value

                            instance = SafeClrConvert.ChangeType(value, _type);
                        }
                    }
                }

                return true;
            }

            if (instance == null)
            {
                if (!(_type.IsInterface || _type.IsAbstract))
                {
                    try
                    {
                        instance = ObjectActivator.Instance.Activate(_type);
                    }
                    catch
                    { }
                }
            }

            if (instance == null)
            {
                return false;
            }

            var arrFieldsToSkip = fieldsToSkip?.Split(',', MyStringSplitOptions.TrimToLowerAndRemoveEmptyEntries) ?? new string[0] { };
            
            for (var index = 0; index < reader.FieldCount; index++)
            {
                var name = reader.GetName(index);

                if (string.IsNullOrWhiteSpace(name))
                {
                    continue;
                }

                if (arrFieldsToSkip.Length > 0 && Array.IndexOf(arrFieldsToSkip, name) < 0)
                {
                    continue;
                }

                var value = reader.GetValue(index);

                ObjectHelper.SetProperty(instance, name, value, true);
            }

            return true;
        }
        public static object ConvertTo(this IDataReader reader, Type type, string fieldsToSkip = null)
        {
            object result = null;

            ConvertTo(reader, ref result, type, fieldsToSkip);

            return result;
        }
        public static T ConvertTo<T>(this IDataReader reader, string fieldsToSkip = null)
        {
            var result = ConvertTo(reader, typeof(T), fieldsToSkip);

            return (T)result;
        }
        public static Array[] ToArray(this IDataReader reader, out string[] columns)
        {
            var result = null as Array[];

            columns = new string[] { };

            if (reader == null)
                throw new InvalidOperationException("No data reader is specified");

            if (reader.IsClosed)
                throw new InvalidOperationException("Data reader cannot be used because it is closed.");

            var fieldCount = reader.FieldCount;

            if (fieldCount > 0)
            {
                columns = new string[fieldCount];

                for (int i = 0; i < fieldCount; i++)
                {
                    columns[i] = reader.GetName(i);
                }

                result = reader.ToArray();
            }

            return result;
        }
        public static Array[] ToArray(this IDataReader reader)
        {
            var result = null as List<Array>;

            if (reader == null)
                throw new InvalidOperationException("No data reader is specified");

            if (reader.IsClosed)
                throw new InvalidOperationException("Data reader cannot be used because it is closed.");

            var fieldCount = reader.FieldCount;

            if (fieldCount > 0)
            {
                result = new List<Array>();

                while (reader.Read())
                {
                    var values = new object[fieldCount];

                    reader.GetValues(values);

                    result.Add(values);
                }
            }

            return result.ToArray();
        }
        public static Array[][] ToArrays(this IDataReader reader, out List<string[]> columns)
        {
            var result = null as List<Array[]>;

            columns = new List<string[]>();

            if (reader == null)
                throw new InvalidOperationException("No data reader is specified");

            if (reader.IsClosed)
                throw new InvalidOperationException("Data reader cannot be used because it is closed.");

            Array[] GetSubResult (IDataReader r, out string[] cols)
            {
                cols = new string[] { };

                var count = r.FieldCount;

                if (count > 0)
                {
                    cols = new string[count];

                    for (int i = 0; i < count; i++)
                    {
                        cols[i] = r.GetName(i);
                    }
                }

                var subResult = new List<Array>();

                while (r.Read())
                {
                    var values = new object[count];

                    r.GetValues(values);

                    subResult.Add(values);
                }

                return subResult.ToArray();
            };

            result = new List<Array[]>();

            string[] subCols;

            result.Add(GetSubResult(reader, out subCols));
            columns.Add(subCols);

            while (reader.NextResult())
            {
                result.Add(GetSubResult(reader, out subCols));
                columns.Add(subCols);
            }

            return result.ToArray();
        }
        public static Array[][] ToArrays(this IDataReader reader)
        {
            var result = null as List<Array[]>;

            if (reader == null)
                throw new InvalidOperationException("No data reader is specified");

            if (reader.IsClosed)
                throw new InvalidOperationException("Data reader cannot be used because it is closed.");

            var fieldCount = reader.FieldCount;

            Array[] GetSubResult(IDataReader r)
            {
                var subResult = new List<Array>();

                while (r.Read())
                {
                    var values = new object[fieldCount];

                    r.GetValues(values);

                    subResult.Add(values);
                }

                return subResult.ToArray();
            };

            if (fieldCount > 0)
            {
                result = new List<Array[]>();

                result.Add(GetSubResult(reader));

                while (reader.NextResult())
                {
                    result.Add(GetSubResult(reader));
                }
            }

            return result.ToArray();
        }
    }
}
