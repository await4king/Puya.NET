using Puya.Base;
using Puya.Conversion;
using Puya.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Reflection;

namespace Puya.Data
{
    public enum CommandExecutionType
    {
        Reader,
        Single,
        NonQuery,
        Scaler
    }
    public static class DbCommandExtensions
    {
        private static void SetOutput(ref object property, object value)
        {

        }
        public static void ApplyOutputs(this DbCommand cmd, object parameters)
        {
            if (cmd != null && parameters != null && cmd.Parameters.Count > 0)
            {
                var dictionary = parameters as IDictionary<string, object>;

                if (dictionary != null)
                {
                    foreach (DbParameter param in cmd.Parameters)
                    {
                        if (param.Direction == ParameterDirection.Output || param.Direction == ParameterDirection.InputOutput)
                        {
                            var paramName = param.ParameterName.StartsWith("@") ? param.ParameterName.Substring(1) : param.ParameterName;

                            if (dictionary.Keys.Contains(paramName))
                            {
                                var outParam = dictionary[paramName] as CommandParameter;

                                if (outParam != null)
                                {
                                    outParam.Value = DBNull.Value.Equals(param.Value) || param.Value == null ? null : param.Value;
                                }
                                else
                                {
                                    dictionary[paramName] = DBNull.Value.Equals(param.Value) || param.Value == null ? null : param.Value;
                                }
                            }
                        }
                    }
                }
                else
                {
                    var props = ReflectionHelper.GetPublicInstanceProperties(parameters.GetType());

                    if (props.Length > 0)
                    {
                        foreach (DbParameter param in cmd.Parameters)
                        {
                            if (param.Direction == ParameterDirection.Output || param.Direction == ParameterDirection.InputOutput)
                            {
                                var paramName = param.ParameterName.StartsWith("@") ? param.ParameterName.Substring(1) : param.ParameterName;
                                var prop = props.FirstOrDefault(p => string.Compare(p.Name, paramName, false) == 0 && p.CustomAttributes.Count(a => a.AttributeType == typeof(IgnoreAttribute)) == 0);

                                if (prop != null)
                                {
                                    if (prop.PropertyType.DescendsFrom<CommandParameter>())
                                    {
                                        var propValue = param.Value == null || DBNull.Value.Equals(param.Value) ? null : param.Value;

                                        if (prop.CanRead)
                                        {
                                            var cp = prop.GetValue(parameters) as CommandParameter;

                                            if (cp != null && (cp.Direction == ParameterDirection.Output || cp.Direction == ParameterDirection.InputOutput))
                                            {
                                                cp.Value = propValue;
                                            }
                                            else
                                            {
                                                if (prop.CanWrite)
                                                {
                                                    cp = new CommandParameter { Name = param.ParameterName, Type = param.DbType, Size = param.Size, Value = propValue };

                                                    prop.SetValue(parameters, cp);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (prop.CanWrite)
                                            {
                                                var cp = new CommandParameter { Name = param.ParameterName, Type = param.DbType, Size = param.Size, Value = propValue };

                                                prop.SetValue(parameters, cp);
                                            }
                                        }
                                    }
                                    else if (prop.PropertyType.IsNullableOrSimpleType() && prop.CanWrite)
                                    {
                                        var propValue =
                                            param.Value == null || DBNull.Value.Equals(param.Value) ?
                                                !prop.PropertyType.IsValueType ? null :
                                                prop.PropertyType.GetDefault() :
                                            ObjectExtensions.ConvertTo(param.Value, prop.PropertyType);

                                        prop.SetValue(parameters, propValue);
                                    }
                                    else if (prop.PropertyType.IsClass && !prop.PropertyType.IsAbstract)
                                    {
                                        if (prop.CanRead)
                                        {
                                            var obj = prop.GetValue(parameters);

                                            if (obj != null)
                                            {
                                                var valueProp = prop.PropertyType.GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);

                                                if (valueProp != null && valueProp.CanWrite)
                                                {
                                                    var propValue =
                                                        param.Value == null || DBNull.Value.Equals(param.Value) ?
                                                            !valueProp.PropertyType.IsValueType ? null :
                                                            valueProp.PropertyType.GetDefault() :
                                                        ObjectExtensions.ConvertTo(param.Value, valueProp.PropertyType);

                                                    valueProp.SetValue(obj, param.Value);
                                                }
                                            }
                                            else
                                            {
                                                if (prop.PropertyType == TypeHelper.TypeOfObject && prop.CanWrite)
                                                {
                                                    var propValue = param.Value == null || DBNull.Value.Equals(param.Value) ? null : param.Value;

                                                    prop.SetValue(parameters, new CommandParameter { Name = param.ParameterName, Size = param.Size, Value = propValue });
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (prop.PropertyType == TypeHelper.TypeOfObject && prop.CanWrite)
                                            {
                                                var propValue = param.Value == null || DBNull.Value.Equals(param.Value) ? null : param.Value;

                                                prop.SetValue(parameters, new CommandParameter { Name = param.ParameterName, Size = param.Size, Value = propValue });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        internal static void InitCommand(DbCommand cmd)
        {
            if (cmd == null)
                throw new CommandNullException();

            if (cmd.Connection == null)
                new CommandConnectionNullException();

            if (cmd.Connection.State == ConnectionState.Closed || cmd.Connection.State == ConnectionState.Broken)
                cmd.Connection.Open();
        }
        internal static void InitCommand<T>(DbCommand cmd, IList<T> list)
        {
            if (cmd == null)
                throw new CommandNullException();

            if (cmd.Connection == null)
                new CommandConnectionNullException();

            if (list == null)
                new ListNullException();

            if (cmd.Connection.State == ConnectionState.Closed || cmd.Connection.State == ConnectionState.Broken)
                cmd.Connection.Open();
        }
        // ------------------ ExecuteReader() ------------------
        public static void ExecuteReader<T>(this DbCommand cmd, IList<T> result, Func<IDataReader, T> fn)
        {
            ExecuteReader<T>(cmd, CommandBehavior.Default, result, fn);
        }
        public static void ExecuteReader<T>(this DbCommand cmd, IList<T> result)
        {
            ExecuteReader(cmd, result, reader => reader.ConvertTo<T>());
        }
        public static IList<T> ExecuteReader<T>(this DbCommand cmd, Func<IDataReader, T> fn)
        {
            var result = new List<T>();

            ExecuteReader(cmd, result, fn);

            return result;
        }
        public static IList<T> ExecuteReader<T>(this DbCommand cmd)
        {
            var result = new List<T>();

            ExecuteReader(cmd, result);

            return result;
        }
        public static void ExecuteReader(this DbCommand cmd, IList<object> result, Type type)
        {
            ExecuteReader(cmd, result, reader => reader.ConvertTo(type));
        }
        public static IList<object> ExecuteReader(this DbCommand cmd, Type type)
        {
            var result = new List<object>();

            ExecuteReader(cmd, result, type);

            return result;
        }
        // ------------------ ExecuteReader(CommandBehavior) ------------------
        public static void ExecuteReader<T>(this DbCommand cmd, CommandBehavior behavior, IList<T> result, Func<IDataReader, T> fn)
        {
            InitCommand(cmd, result);

            using (var reader = cmd.ExecuteReader(behavior))
            {
                while (reader.Read())
                {
                    var item = fn(reader);

                    result.Add(item);
                }
            }
        }
        public static void ExecuteReader<T>(this DbCommand cmd, CommandBehavior behavior, IList<T> result)
        {
            ExecuteReader(cmd, behavior, result, reader => reader.ConvertTo<T>());
        }
        public static IList<T> ExecuteReader<T>(this DbCommand cmd, CommandBehavior behavior, Func<IDataReader, T> fn)
        {
            var result = new List<T>();

            ExecuteReader(cmd, behavior, result, fn);

            return result;
        }
        public static IList<T> ExecuteReader<T>(this DbCommand cmd, CommandBehavior behavior)
        {
            var result = new List<T>();

            ExecuteReader(cmd, behavior, result);

            return result;
        }
        public static void ExecuteReader(this DbCommand cmd, CommandBehavior behavior, IList<object> result, Type type)
        {
            ExecuteReader(cmd, behavior, result, reader => reader.ConvertTo(type));
        }
        public static IList<object> ExecuteReader(this DbCommand cmd, CommandBehavior behavior, Type type)
        {
            var result = new List<object>();

            ExecuteReader(cmd, behavior, result, type);

            return result;
        }
        // ------------------ ExecuteSingle ------------------
        public static T ExecuteSingle<T>(this DbCommand cmd, Func<IDataReader, T> fn)
        {
            return ExecuteSingle<T>(cmd, CommandBehavior.Default, fn);
        }
        public static T ExecuteSingle<T>(this DbCommand cmd)
        {
            var result = ExecuteSingle(cmd, reader => reader.ConvertTo<T>());

            return result;
        }
        public static object ExecuteSingle(this DbCommand cmd, Type type)
        {
            var result = ExecuteSingle(cmd, reader => reader.ConvertTo(type));

            return result;
        }
        // ------------------ ExecuteSingle (CommandBehavior) ------------------
        public static T ExecuteSingle<T>(this DbCommand cmd, CommandBehavior behavior, Func<IDataReader, T> fn)
        {
            var result = default(T);

            InitCommand(cmd);

            using (var reader = cmd.ExecuteReader(behavior))
            {
                while (reader.Read())
                {
                    result = fn(reader);

                    break;
                }
            }

            return result;
        }
        public static T ExecuteSingle<T>(this DbCommand cmd, CommandBehavior behavior)
        {
            var result = ExecuteSingle(cmd, behavior, reader => reader.ConvertTo<T>());

            return result;
        }
        public static object ExecuteSingle(this DbCommand cmd, CommandBehavior behavior, Type type)
        {
            var result = ExecuteSingle(cmd, behavior, reader => reader.ConvertTo(type));

            return result;
        }
        // ------------------ ExecuteSingleAsync ------------------
        public static Task<T> ExecuteSingleAsync<T>(this DbCommand cmd, Func<IDataReader, T> fn, CancellationToken cancellation)
        {
            return ExecuteSingleAsync<T>(cmd, CommandBehavior.Default, fn, cancellation);
        }
        public static Task<T> ExecuteSingleAsync<T>(this DbCommand cmd, Func<IDataReader, T> fn)
        {
            return ExecuteSingleAsync<T>(cmd, fn, CancellationToken.None);
        }
        public static Task<T> ExecuteSingleAsync<T>(this DbCommand cmd, CancellationToken cancellation)
        {
            return ExecuteSingleAsync(cmd, reader => reader.ConvertTo<T>(), cancellation);
        }
        public static Task<T> ExecuteSingleAsync<T>(this DbCommand cmd)
        {
            return ExecuteSingleAsync<T>(cmd, CancellationToken.None);
        }
        public static Task<object> ExecuteSingleAsync(this DbCommand cmd, Type type, CancellationToken cancellation)
        {
            return ExecuteSingleAsync(cmd, reader => reader.ConvertTo(type), cancellation);
        }
        public static Task<object> ExecuteSingleAsync(this DbCommand cmd, Type type)
        {
            return ExecuteSingleAsync(cmd, type, CancellationToken.None);
        }
        // ------------------ ExecuteSingleAsyncAsync (CommandBehavior) ------------------
        public static async Task<T> ExecuteSingleAsync<T>(this DbCommand cmd, CommandBehavior behavior, Func<IDataReader, T> fn, CancellationToken cancellation)
        {
            var result = default(T);

            InitCommand(cmd);

            using (var reader = await cmd.ExecuteReaderAsync(behavior))
            {
                while (reader.Read())
                {
                    result = fn(reader);

                    break;
                }
            }

            return result;
        }
        public static Task<T> ExecuteSingleAsync<T>(this DbCommand cmd, CommandBehavior behavior, Func<IDataReader, T> fn)
        {
            return ExecuteSingleAsync<T>(cmd, behavior, fn, CancellationToken.None);
        }
        public static Task<T> ExecuteSingleAsync<T>(this DbCommand cmd, CommandBehavior behavior, CancellationToken cancellation)
        {
            return ExecuteSingleAsync(cmd, behavior, reader => reader.ConvertTo<T>());
        }
        public static Task<T> ExecuteSingleAsync<T>(this DbCommand cmd, CommandBehavior behavior)
        {
            return ExecuteSingleAsync<T>(cmd, behavior, CancellationToken.None);
        }
        public static Task<object> ExecuteSingleAsync(this DbCommand cmd, CommandBehavior behavior, Type type, CancellationToken cancellation)
        {
            return ExecuteSingleAsync(cmd, behavior, reader => reader.ConvertTo(type));
        }
        public static Task<object> ExecuteSingleAsync(this DbCommand cmd, CommandBehavior behavior, Type type)
        {
            return ExecuteSingleAsync(cmd, behavior, type, CancellationToken.None);
        }
        // ------------------ ExecuteReaderAsync ------------------
        public static Task ExecuteReaderAsync<T>(this DbCommand cmd, IList<T> result, Func<IDataReader, T> fn, CancellationToken cancellation)
        {
            return ExecuteReaderAsync<T>(cmd, CommandBehavior.Default, result, fn, cancellation);
        }
        public static Task ExecuteReaderAsync<T>(this DbCommand cmd, IList<T> result, Func<IDataReader, T> fn)
        {
            return ExecuteReaderAsync<T>(cmd, result, fn, CancellationToken.None);
        }
        public static Task ExecuteReaderAsync<T>(this DbCommand cmd, IList<T> result, CancellationToken cancellation)
        {
            return ExecuteReaderAsync(cmd, result, reader => reader.ConvertTo<T>(), cancellation);
        }
        public static Task ExecuteReaderAsync<T>(this DbCommand cmd, IList<T> result)
        {
            return ExecuteReaderAsync(cmd, result, CancellationToken.None);
        }
        public static async Task<IList<T>> ExecuteReaderAsync<T>(this DbCommand cmd, Func<IDataReader, T> fn, CancellationToken cancellation)
        {
            var result = new List<T>();

            await ExecuteReaderAsync(cmd, result, fn, cancellation);

            return result;
        }
        public static Task<IList<T>> ExecuteReaderAsync<T>(this DbCommand cmd, Func<IDataReader, T> fn)
        {
            return ExecuteReaderAsync<T>(cmd, fn, CancellationToken.None);

        }
        public static async Task<IList<T>> ExecuteReaderAsync<T>(this DbCommand cmd, CancellationToken cancellation)
        {
            var result = new List<T>();

            await ExecuteReaderAsync(cmd, result, cancellation);

            return result;
        }
        public static Task<IList<T>> ExecuteReaderAsync<T>(this DbCommand cmd)
        {
            return ExecuteReaderAsync<T>(cmd, CancellationToken.None);
        }
        public static Task ExecuteReaderAsync(this DbCommand cmd, IList<object> result, Type type, CancellationToken cancellation)
        {
            return ExecuteReaderAsync(cmd, result, reader => reader.ConvertTo(type), cancellation);
        }
        public static Task ExecuteReaderAsync(this DbCommand cmd, IList<object> result, Type type)
        {
            return ExecuteReaderAsync(cmd, result, type, CancellationToken.None);
        }
        public static async Task<IList<object>> ExecuteReaderAsync(this DbCommand cmd, Type type, CancellationToken cancellation)
        {
            var result = new List<object>();

            await ExecuteReaderAsync(cmd, result, type, cancellation);

            return result;
        }
        public static Task<IList<object>> ExecuteReaderAsync(this DbCommand cmd, Type type)
        {
            return ExecuteReaderAsync(cmd, type, CancellationToken.None);
        }
        // ------------------ ExecuteReaderAsync (CoomandBehavior) ------------------
        public static async Task ExecuteReaderAsync<T>(this DbCommand cmd, CommandBehavior behavior, IList<T> result, Func<IDataReader, T> fn, CancellationToken cancellation)
        {
            InitCommand(cmd, result);

            using (var reader = await cmd.ExecuteReaderAsync(behavior, cancellation))
            {
                while (reader.Read())
                {
                    var item = fn(reader);

                    result.Add(item);
                }
            }
        }
        public static Task ExecuteReaderAsync<T>(this DbCommand cmd, CommandBehavior behavior, IList<T> result, Func<IDataReader, T> fn)
        {
            return ExecuteReaderAsync<T>(cmd, behavior, result, fn, CancellationToken.None);
        }
        public static Task ExecuteReaderAsync<T>(this DbCommand cmd, CommandBehavior behavior, IList<T> result, CancellationToken cancellation)
        {
            return ExecuteReaderAsync(cmd, behavior, result, reader => reader.ConvertTo<T>(), cancellation);
        }
        public static Task ExecuteReaderAsync<T>(this DbCommand cmd, CommandBehavior behavior, IList<T> result)
        {
            return ExecuteReaderAsync(cmd, behavior, result, CancellationToken.None);
        }
        public static async Task<IList<T>> ExecuteReaderAsync<T>(this DbCommand cmd, CommandBehavior behavior, Func<IDataReader, T> fn, CancellationToken cancellation)
        {
            var result = new List<T>();

            await ExecuteReaderAsync(cmd, behavior, result, fn, cancellation);

            return result;
        }
        public static Task<IList<T>> ExecuteReaderAsync<T>(this DbCommand cmd, CommandBehavior behavior, Func<IDataReader, T> fn)
        {
            return ExecuteReaderAsync<T>(cmd, behavior, fn, CancellationToken.None);

        }
        public static async Task<IList<T>> ExecuteReaderAsync<T>(this DbCommand cmd, CommandBehavior behavior, CancellationToken cancellation)
        {
            var result = new List<T>();

            await ExecuteReaderAsync(cmd, behavior, result, cancellation);

            return result;
        }
        public static Task<IList<T>> ExecuteReaderAsync<T>(this DbCommand cmd, CommandBehavior behavior)
        {
            return ExecuteReaderAsync<T>(cmd, behavior, CancellationToken.None);
        }
        public static Task ExecuteReaderAsync(this DbCommand cmd, CommandBehavior behavior, IList<object> result, Type type, CancellationToken cancellation)
        {
            return ExecuteReaderAsync(cmd, behavior, result, reader => reader.ConvertTo(type), cancellation);
        }
        public static Task ExecuteReaderAsync(this DbCommand cmd, CommandBehavior behavior, IList<object> result, Type type)
        {
            return ExecuteReaderAsync(cmd, behavior, result, type, CancellationToken.None);
        }
        public static async Task<IList<object>> ExecuteReaderAsync(this DbCommand cmd, CommandBehavior behavior, Type type, CancellationToken cancellation)
        {
            var result = new List<object>();

            await ExecuteReaderAsync(cmd, behavior, result, type, cancellation);

            return result;
        }
        public static Task<IList<object>> ExecuteReaderAsync(this DbCommand cmd, CommandBehavior behavior, Type type)
        {
            return ExecuteReaderAsync(cmd, behavior, type, CancellationToken.None);
        }
        // ------------------ Execute (Scaler/NonQuery) ------------------
        public static object Execute(this DbCommand cmd, bool scaler)
        {
            InitCommand(cmd);

            object result = null;

            if (scaler)
            {
                result = cmd.ExecuteScalar();
            }
            else
            {
                result = cmd.ExecuteNonQuery();
            }

            return result;
        }
        public static async Task<object> ExecuteAsync(this DbCommand cmd, bool scaler, CancellationToken cancellation)
        {
            InitCommand(cmd);

            if (cmd.Connection.State == ConnectionState.Closed || cmd.Connection.State == ConnectionState.Broken)
                cmd.Connection.Open();

            object result = null;

            if (scaler)
            {
                result = await cmd.ExecuteScalarAsync(cancellation);
            }
            else
            {
                result = await cmd.ExecuteNonQueryAsync(cancellation);
            }

            return result;
        }
        public static Task<object> ExecuteAsync(this DbCommand cmd, bool scaler)
        {
            return ExecuteAsync(cmd, scaler, CancellationToken.None);
        }
    }
}
