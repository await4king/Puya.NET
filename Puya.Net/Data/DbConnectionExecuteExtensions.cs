using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Data
{
    public static partial class DbConnectionExtensions
    {
		public static IList<T> ExecuteReaderCommand<T>(this DbConnection con, string sproc, CommandBehavior behavior, Func<IDataReader, T> fn, object parameters = null)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters);
            var result = DbCommandExtensions.ExecuteReader<T>(cmd, behavior, fn);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
        public static T ExecuteSingleCommand<T>(this DbConnection con, string sproc, CommandBehavior behavior, Func<IDataReader, T> fn, object parameters = null)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters);
            var result = DbCommandExtensions.ExecuteSingle<T>(cmd, behavior, fn);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
		public static IList<T> ExecuteReaderCommand<T>(this DbConnection con, string sproc, Func<IDataReader, T> fn, object parameters = null)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters);
            var result = DbCommandExtensions.ExecuteReader<T>(cmd, fn);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
        public static T ExecuteSingleCommand<T>(this DbConnection con, string sproc, Func<IDataReader, T> fn, object parameters = null)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters);
            var result = DbCommandExtensions.ExecuteSingle<T>(cmd, fn);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
		public static IList<T> ExecuteReaderCommand<T>(this DbConnection con, string sproc, CommandBehavior behavior, object parameters = null)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters);
            var result = DbCommandExtensions.ExecuteReader<T>(cmd, behavior);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
        public static T ExecuteSingleCommand<T>(this DbConnection con, string sproc, CommandBehavior behavior, object parameters = null)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters);
            var result = DbCommandExtensions.ExecuteSingle<T>(cmd, behavior);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
		public static IList<T> ExecuteReaderCommand<T>(this DbConnection con, string sproc, object parameters = null)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters);
            var result = DbCommandExtensions.ExecuteReader<T>(cmd);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
        public static T ExecuteSingleCommand<T>(this DbConnection con, string sproc, object parameters = null)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters);
            var result = DbCommandExtensions.ExecuteSingle<T>(cmd);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
		public static IList<T> ExecuteReaderSql<T>(this DbConnection con, string query, CommandBehavior behavior, Func<IDataReader, T> fn, object parameters = null)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(query, CommandType.Text, parameters);
            var result = DbCommandExtensions.ExecuteReader<T>(cmd, behavior, fn);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
        public static T ExecuteSingleSql<T>(this DbConnection con, string query, CommandBehavior behavior, Func<IDataReader, T> fn, object parameters = null)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(query, CommandType.Text, parameters);
            var result = DbCommandExtensions.ExecuteSingle<T>(cmd, behavior, fn);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
		public static IList<T> ExecuteReaderSql<T>(this DbConnection con, string query, Func<IDataReader, T> fn, object parameters = null)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(query, CommandType.Text, parameters);
            var result = DbCommandExtensions.ExecuteReader<T>(cmd, fn);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
        public static T ExecuteSingleSql<T>(this DbConnection con, string query, Func<IDataReader, T> fn, object parameters = null)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(query, CommandType.Text, parameters);
            var result = DbCommandExtensions.ExecuteSingle<T>(cmd, fn);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
		public static IList<T> ExecuteReaderSql<T>(this DbConnection con, string query, CommandBehavior behavior, object parameters = null)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(query, CommandType.Text, parameters);
            var result = DbCommandExtensions.ExecuteReader<T>(cmd, behavior);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
        public static T ExecuteSingleSql<T>(this DbConnection con, string query, CommandBehavior behavior, object parameters = null)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(query, CommandType.Text, parameters);
            var result = DbCommandExtensions.ExecuteSingle<T>(cmd, behavior);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
		public static IList<T> ExecuteReaderSql<T>(this DbConnection con, string query, object parameters = null)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(query, CommandType.Text, parameters);
            var result = DbCommandExtensions.ExecuteReader<T>(cmd);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
        public static T ExecuteSingleSql<T>(this DbConnection con, string query, object parameters = null)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(query, CommandType.Text, parameters);
            var result = DbCommandExtensions.ExecuteSingle<T>(cmd);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
		public static async Task<IList<T>> ExecuteReaderCommandAsync<T>(this DbConnection con, string sproc, CommandBehavior behavior, Func<IDataReader, T> fn, object parameters, CancellationToken cancellation)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters);
            var result = await DbCommandExtensions.ExecuteReaderAsync<T>(cmd, behavior, fn, cancellation);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
        public static async Task<T> ExecuteSingleCommandAsync<T>(this DbConnection con, string sproc, CommandBehavior behavior, Func<IDataReader, T> fn, object parameters, CancellationToken cancellation)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters);
            var result = await DbCommandExtensions.ExecuteSingleAsync<T>(cmd, behavior, fn, cancellation);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
		public static Task<IList<T>> ExecuteReaderCommandAsync<T>(this DbConnection con, string sproc, CommandBehavior behavior, Func<IDataReader, T> fn, object parameters = null)
        {
            return ExecuteReaderCommandAsync<T>(con, sproc, behavior, fn, parameters, CancellationToken.None);
        }
        public static Task<T> ExecuteSingleCommandAsync<T>(this DbConnection con, string sproc, CommandBehavior behavior, Func<IDataReader, T> fn, object parameters = null)
        {
            return ExecuteSingleCommandAsync<T>(con, sproc, behavior, fn, parameters, CancellationToken.None);
        }
		public static async Task<IList<T>> ExecuteReaderCommandAsync<T>(this DbConnection con, string sproc, Func<IDataReader, T> fn, object parameters, CancellationToken cancellation)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters);
            var result = await DbCommandExtensions.ExecuteReaderAsync<T>(cmd, fn, cancellation);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
        public static async Task<T> ExecuteSingleCommandAsync<T>(this DbConnection con, string sproc, Func<IDataReader, T> fn, object parameters, CancellationToken cancellation)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters);
            var result = await DbCommandExtensions.ExecuteSingleAsync<T>(cmd, fn, cancellation);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
		public static Task<IList<T>> ExecuteReaderCommandAsync<T>(this DbConnection con, string sproc, Func<IDataReader, T> fn, object parameters = null)
        {
            return ExecuteReaderCommandAsync<T>(con, sproc, fn, parameters, CancellationToken.None);
        }
        public static Task<T> ExecuteSingleCommandAsync<T>(this DbConnection con, string sproc, Func<IDataReader, T> fn, object parameters = null)
        {
            return ExecuteSingleCommandAsync<T>(con, sproc, fn, parameters, CancellationToken.None);
        }
		public static async Task<IList<T>> ExecuteReaderCommandAsync<T>(this DbConnection con, string sproc, CommandBehavior behavior, object parameters, CancellationToken cancellation)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters);
            var result = await DbCommandExtensions.ExecuteReaderAsync<T>(cmd, behavior, cancellation);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
        public static async Task<T> ExecuteSingleCommandAsync<T>(this DbConnection con, string sproc, CommandBehavior behavior, object parameters, CancellationToken cancellation)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters);
            var result = await DbCommandExtensions.ExecuteSingleAsync<T>(cmd, behavior, cancellation);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
		public static Task<IList<T>> ExecuteReaderCommandAsync<T>(this DbConnection con, string sproc, CommandBehavior behavior, object parameters = null)
        {
            return ExecuteReaderCommandAsync<T>(con, sproc, behavior, parameters, CancellationToken.None);
        }
        public static Task<T> ExecuteSingleCommandAsync<T>(this DbConnection con, string sproc, CommandBehavior behavior, object parameters = null)
        {
            return ExecuteSingleCommandAsync<T>(con, sproc, behavior, parameters, CancellationToken.None);
        }
		public static async Task<IList<T>> ExecuteReaderCommandAsync<T>(this DbConnection con, string sproc, object parameters, CancellationToken cancellation)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters);
            var result = await DbCommandExtensions.ExecuteReaderAsync<T>(cmd, cancellation);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
        public static async Task<T> ExecuteSingleCommandAsync<T>(this DbConnection con, string sproc, object parameters, CancellationToken cancellation)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters);
            var result = await DbCommandExtensions.ExecuteSingleAsync<T>(cmd, cancellation);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
		public static Task<IList<T>> ExecuteReaderCommandAsync<T>(this DbConnection con, string sproc, object parameters = null)
        {
            return ExecuteReaderCommandAsync<T>(con, sproc, parameters, CancellationToken.None);
        }
        public static Task<T> ExecuteSingleCommandAsync<T>(this DbConnection con, string sproc, object parameters = null)
        {
            return ExecuteSingleCommandAsync<T>(con, sproc, parameters, CancellationToken.None);
        }
		public static async Task<IList<T>> ExecuteReaderSqlAsync<T>(this DbConnection con, string query, CommandBehavior behavior, Func<IDataReader, T> fn, object parameters, CancellationToken cancellation)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(query, CommandType.Text, parameters);
            var result = await DbCommandExtensions.ExecuteReaderAsync<T>(cmd, behavior, fn, cancellation);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
        public static async Task<T> ExecuteSingleSqlAsync<T>(this DbConnection con, string query, CommandBehavior behavior, Func<IDataReader, T> fn, object parameters, CancellationToken cancellation)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(query, CommandType.Text, parameters);
            var result = await DbCommandExtensions.ExecuteSingleAsync<T>(cmd, behavior, fn, cancellation);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
		public static Task<IList<T>> ExecuteReaderSqlAsync<T>(this DbConnection con, string query, CommandBehavior behavior, Func<IDataReader, T> fn, object parameters = null)
        {
            return ExecuteReaderSqlAsync<T>(con, query, behavior, fn, parameters, CancellationToken.None);
        }
        public static Task<T> ExecuteSingleSqlAsync<T>(this DbConnection con, string query, CommandBehavior behavior, Func<IDataReader, T> fn, object parameters = null)
        {
            return ExecuteSingleSqlAsync<T>(con, query, behavior, fn, parameters, CancellationToken.None);
        }
		public static async Task<IList<T>> ExecuteReaderSqlAsync<T>(this DbConnection con, string query, Func<IDataReader, T> fn, object parameters, CancellationToken cancellation)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(query, CommandType.Text, parameters);
            var result = await DbCommandExtensions.ExecuteReaderAsync<T>(cmd, fn, cancellation);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
        public static async Task<T> ExecuteSingleSqlAsync<T>(this DbConnection con, string query, Func<IDataReader, T> fn, object parameters, CancellationToken cancellation)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(query, CommandType.Text, parameters);
            var result = await DbCommandExtensions.ExecuteSingleAsync<T>(cmd, fn, cancellation);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
		public static Task<IList<T>> ExecuteReaderSqlAsync<T>(this DbConnection con, string query, Func<IDataReader, T> fn, object parameters = null)
        {
            return ExecuteReaderSqlAsync<T>(con, query, fn, parameters, CancellationToken.None);
        }
        public static Task<T> ExecuteSingleSqlAsync<T>(this DbConnection con, string query, Func<IDataReader, T> fn, object parameters = null)
        {
            return ExecuteSingleSqlAsync<T>(con, query, fn, parameters, CancellationToken.None);
        }
		public static async Task<IList<T>> ExecuteReaderSqlAsync<T>(this DbConnection con, string query, CommandBehavior behavior, object parameters, CancellationToken cancellation)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(query, CommandType.Text, parameters);
            var result = await DbCommandExtensions.ExecuteReaderAsync<T>(cmd, behavior, cancellation);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
        public static async Task<T> ExecuteSingleSqlAsync<T>(this DbConnection con, string query, CommandBehavior behavior, object parameters, CancellationToken cancellation)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(query, CommandType.Text, parameters);
            var result = await DbCommandExtensions.ExecuteSingleAsync<T>(cmd, behavior, cancellation);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
		public static Task<IList<T>> ExecuteReaderSqlAsync<T>(this DbConnection con, string query, CommandBehavior behavior, object parameters = null)
        {
            return ExecuteReaderSqlAsync<T>(con, query, behavior, parameters, CancellationToken.None);
        }
        public static Task<T> ExecuteSingleSqlAsync<T>(this DbConnection con, string query, CommandBehavior behavior, object parameters = null)
        {
            return ExecuteSingleSqlAsync<T>(con, query, behavior, parameters, CancellationToken.None);
        }
		public static async Task<IList<T>> ExecuteReaderSqlAsync<T>(this DbConnection con, string query, object parameters, CancellationToken cancellation)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(query, CommandType.Text, parameters);
            var result = await DbCommandExtensions.ExecuteReaderAsync<T>(cmd, cancellation);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
        public static async Task<T> ExecuteSingleSqlAsync<T>(this DbConnection con, string query, object parameters, CancellationToken cancellation)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(query, CommandType.Text, parameters);
            var result = await DbCommandExtensions.ExecuteSingleAsync<T>(cmd, cancellation);

            cmd.ApplyOutputs(parameters);
                
            return result;
        }
		public static Task<IList<T>> ExecuteReaderSqlAsync<T>(this DbConnection con, string query, object parameters = null)
        {
            return ExecuteReaderSqlAsync<T>(con, query, parameters, CancellationToken.None);
        }
        public static Task<T> ExecuteSingleSqlAsync<T>(this DbConnection con, string query, object parameters = null)
        {
            return ExecuteSingleSqlAsync<T>(con, query, parameters, CancellationToken.None);
        }
        public static int ExecuteNonQueryCommand(this DbConnection con, string sproc, object parameters = null)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters);
            var obj = DbCommandExtensions.Execute(cmd, false);
            var result = (int)System.Convert.ChangeType(obj, typeof(System.Int32));

            cmd.ApplyOutputs(parameters);
                
			return result;
        }
		public static object ExecuteScalerCommand(this DbConnection con, string sproc, object parameters = null)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters);
            var result = cmd.Execute(true);

            cmd.ApplyOutputs(parameters);

            return result;
        }
        public static int ExecuteNonQuerySql(this DbConnection con, string query, object parameters = null)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(query, CommandType.Text, parameters);
            var obj = DbCommandExtensions.Execute(cmd, false);
            var result = (int)System.Convert.ChangeType(obj, typeof(System.Int32));

            cmd.ApplyOutputs(parameters);
                
			return result;
        }
		public static object ExecuteScalerSql(this DbConnection con, string query, object parameters = null)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(query, CommandType.Text, parameters);
            var result = cmd.Execute(true);

            cmd.ApplyOutputs(parameters);

            return result;
        }
        public static async Task<int> ExecuteNonQueryCommandAsync(this DbConnection con, string sproc, object parameters, CancellationToken cancellation)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters);
            var obj = await DbCommandExtensions.ExecuteAsync(cmd, false, cancellation);
            var result = (int)System.Convert.ChangeType(obj, typeof(System.Int32));

            cmd.ApplyOutputs(parameters);
                
			return result;
        }
		public static async Task<object> ExecuteScalerCommandAsync(this DbConnection con, string sproc, object parameters, CancellationToken cancellation)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters);
            var result = await cmd.ExecuteAsync(true, cancellation);

            cmd.ApplyOutputs(parameters);

            return result;
        }
		public static Task<int> ExecuteNonQueryCommandAsync(this DbConnection con, string sproc, object parameters = null)
        {
            return ExecuteNonQueryCommandAsync(con, sproc, parameters, CancellationToken.None);
        }
		public static Task<object> ExecuteScalerCommandAsync(this DbConnection con, string sproc, object parameters = null)
        {
            return ExecuteScalerCommandAsync(con, sproc, parameters, CancellationToken.None);
        }
        public static async Task<int> ExecuteNonQuerySqlAsync(this DbConnection con, string query, object parameters, CancellationToken cancellation)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(query, CommandType.Text, parameters);
            var obj = await DbCommandExtensions.ExecuteAsync(cmd, false, cancellation);
            var result = (int)System.Convert.ChangeType(obj, typeof(System.Int32));

            cmd.ApplyOutputs(parameters);
                
			return result;
        }
		public static async Task<object> ExecuteScalerSqlAsync(this DbConnection con, string query, object parameters, CancellationToken cancellation)
        {
            if (con == null)
	        {
		        throw new ConnectionNullException();
	        }

            var cmd = con.CreateCommand(query, CommandType.Text, parameters);
            var result = await cmd.ExecuteAsync(true, cancellation);

            cmd.ApplyOutputs(parameters);

            return result;
        }
		public static Task<int> ExecuteNonQuerySqlAsync(this DbConnection con, string query, object parameters = null)
        {
            return ExecuteNonQuerySqlAsync(con, query, parameters, CancellationToken.None);
        }
		public static Task<object> ExecuteScalerSqlAsync(this DbConnection con, string query, object parameters = null)
        {
            return ExecuteScalerSqlAsync(con, query, parameters, CancellationToken.None);
        }
	}
}