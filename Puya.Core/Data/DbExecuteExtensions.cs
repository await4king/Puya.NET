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
    public static partial class DbExecuteExtensions
    {
		public static IList<T> ExecuteReaderCommand<T>(this IDb db, string sproc, CommandBehavior behavior, Func<IDataReader, T> fn, object parameters = null)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = null as IList<T>;
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters, db.AutoNullEmptyStrings);
                
                result = DbCommandExtensions.ExecuteReader<T>(cmd, behavior, fn);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
        public static T ExecuteSingleCommand<T>(this IDb db, string sproc, CommandBehavior behavior, Func<IDataReader, T> fn, object parameters = null)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = default(T);
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters, db.AutoNullEmptyStrings);
                
                result = DbCommandExtensions.ExecuteSingle<T>(cmd, behavior, fn);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
		public static IList<T> ExecuteReaderCommand<T>(this IDb db, string sproc, Func<IDataReader, T> fn, object parameters = null)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = null as IList<T>;
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters, db.AutoNullEmptyStrings);
                
                result = DbCommandExtensions.ExecuteReader<T>(cmd, fn);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
        public static T ExecuteSingleCommand<T>(this IDb db, string sproc, Func<IDataReader, T> fn, object parameters = null)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = default(T);
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters, db.AutoNullEmptyStrings);
                
                result = DbCommandExtensions.ExecuteSingle<T>(cmd, fn);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
		public static IList<T> ExecuteReaderCommand<T>(this IDb db, string sproc, CommandBehavior behavior, object parameters = null)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = null as IList<T>;
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters, db.AutoNullEmptyStrings);
                
                result = DbCommandExtensions.ExecuteReader<T>(cmd, behavior);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
        public static T ExecuteSingleCommand<T>(this IDb db, string sproc, CommandBehavior behavior, object parameters = null)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = default(T);
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters, db.AutoNullEmptyStrings);
                
                result = DbCommandExtensions.ExecuteSingle<T>(cmd, behavior);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
		public static IList<T> ExecuteReaderCommand<T>(this IDb db, string sproc, object parameters = null)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = null as IList<T>;
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters, db.AutoNullEmptyStrings);
                
                result = DbCommandExtensions.ExecuteReader<T>(cmd);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
        public static T ExecuteSingleCommand<T>(this IDb db, string sproc, object parameters = null)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = default(T);
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters, db.AutoNullEmptyStrings);
                
                result = DbCommandExtensions.ExecuteSingle<T>(cmd);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
		public static IList<T> ExecuteReaderSql<T>(this IDb db, string query, CommandBehavior behavior, Func<IDataReader, T> fn, object parameters = null)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = null as IList<T>;
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(query, CommandType.Text, parameters, db.AutoNullEmptyStrings);
                
                result = DbCommandExtensions.ExecuteReader<T>(cmd, behavior, fn);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
        public static T ExecuteSingleSql<T>(this IDb db, string query, CommandBehavior behavior, Func<IDataReader, T> fn, object parameters = null)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = default(T);
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(query, CommandType.Text, parameters, db.AutoNullEmptyStrings);
                
                result = DbCommandExtensions.ExecuteSingle<T>(cmd, behavior, fn);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
		public static IList<T> ExecuteReaderSql<T>(this IDb db, string query, Func<IDataReader, T> fn, object parameters = null)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = null as IList<T>;
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(query, CommandType.Text, parameters, db.AutoNullEmptyStrings);
                
                result = DbCommandExtensions.ExecuteReader<T>(cmd, fn);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
        public static T ExecuteSingleSql<T>(this IDb db, string query, Func<IDataReader, T> fn, object parameters = null)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = default(T);
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(query, CommandType.Text, parameters, db.AutoNullEmptyStrings);
                
                result = DbCommandExtensions.ExecuteSingle<T>(cmd, fn);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
		public static IList<T> ExecuteReaderSql<T>(this IDb db, string query, CommandBehavior behavior, object parameters = null)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = null as IList<T>;
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(query, CommandType.Text, parameters, db.AutoNullEmptyStrings);
                
                result = DbCommandExtensions.ExecuteReader<T>(cmd, behavior);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
        public static T ExecuteSingleSql<T>(this IDb db, string query, CommandBehavior behavior, object parameters = null)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = default(T);
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(query, CommandType.Text, parameters, db.AutoNullEmptyStrings);
                
                result = DbCommandExtensions.ExecuteSingle<T>(cmd, behavior);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
		public static IList<T> ExecuteReaderSql<T>(this IDb db, string query, object parameters = null)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = null as IList<T>;
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(query, CommandType.Text, parameters, db.AutoNullEmptyStrings);
                
                result = DbCommandExtensions.ExecuteReader<T>(cmd);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
        public static T ExecuteSingleSql<T>(this IDb db, string query, object parameters = null)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = default(T);
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(query, CommandType.Text, parameters, db.AutoNullEmptyStrings);
                
                result = DbCommandExtensions.ExecuteSingle<T>(cmd);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
		public static async Task<IList<T>> ExecuteReaderCommandAsync<T>(this IDb db, string sproc, CommandBehavior behavior, Func<IDataReader, T> fn, object parameters, CancellationToken cancellation)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = null as IList<T>;
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters, db.AutoNullEmptyStrings);
                
                result = await DbCommandExtensions.ExecuteReaderAsync<T>(cmd, behavior, fn, cancellation);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
        public static async Task<T> ExecuteSingleCommandAsync<T>(this IDb db, string sproc, CommandBehavior behavior, Func<IDataReader, T> fn, object parameters, CancellationToken cancellation)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = default(T);
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters, db.AutoNullEmptyStrings);
                
                result = await DbCommandExtensions.ExecuteSingleAsync<T>(cmd, behavior, fn, cancellation);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
		public static Task<IList<T>> ExecuteReaderCommandAsync<T>(this IDb db, string sproc, CommandBehavior behavior, Func<IDataReader, T> fn, object parameters = null)
        {
            return ExecuteReaderCommandAsync<T>(db, sproc, behavior, fn, parameters, CancellationToken.None);
        }
        public static Task<T> ExecuteSingleCommandAsync<T>(this IDb db, string sproc, CommandBehavior behavior, Func<IDataReader, T> fn, object parameters = null)
        {
            return ExecuteSingleCommandAsync<T>(db, sproc, behavior, fn, parameters, CancellationToken.None);
        }
		public static async Task<IList<T>> ExecuteReaderCommandAsync<T>(this IDb db, string sproc, Func<IDataReader, T> fn, object parameters, CancellationToken cancellation)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = null as IList<T>;
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters, db.AutoNullEmptyStrings);
                
                result = await DbCommandExtensions.ExecuteReaderAsync<T>(cmd, fn, cancellation);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
        public static async Task<T> ExecuteSingleCommandAsync<T>(this IDb db, string sproc, Func<IDataReader, T> fn, object parameters, CancellationToken cancellation)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = default(T);
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters, db.AutoNullEmptyStrings);
                
                result = await DbCommandExtensions.ExecuteSingleAsync<T>(cmd, fn, cancellation);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
		public static Task<IList<T>> ExecuteReaderCommandAsync<T>(this IDb db, string sproc, Func<IDataReader, T> fn, object parameters = null)
        {
            return ExecuteReaderCommandAsync<T>(db, sproc, fn, parameters, CancellationToken.None);
        }
        public static Task<T> ExecuteSingleCommandAsync<T>(this IDb db, string sproc, Func<IDataReader, T> fn, object parameters = null)
        {
            return ExecuteSingleCommandAsync<T>(db, sproc, fn, parameters, CancellationToken.None);
        }
		public static async Task<IList<T>> ExecuteReaderCommandAsync<T>(this IDb db, string sproc, CommandBehavior behavior, object parameters, CancellationToken cancellation)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = null as IList<T>;
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters, db.AutoNullEmptyStrings);
                
                result = await DbCommandExtensions.ExecuteReaderAsync<T>(cmd, behavior, cancellation);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
        public static async Task<T> ExecuteSingleCommandAsync<T>(this IDb db, string sproc, CommandBehavior behavior, object parameters, CancellationToken cancellation)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = default(T);
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters, db.AutoNullEmptyStrings);
                
                result = await DbCommandExtensions.ExecuteSingleAsync<T>(cmd, behavior, cancellation);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
		public static Task<IList<T>> ExecuteReaderCommandAsync<T>(this IDb db, string sproc, CommandBehavior behavior, object parameters = null)
        {
            return ExecuteReaderCommandAsync<T>(db, sproc, behavior, parameters, CancellationToken.None);
        }
        public static Task<T> ExecuteSingleCommandAsync<T>(this IDb db, string sproc, CommandBehavior behavior, object parameters = null)
        {
            return ExecuteSingleCommandAsync<T>(db, sproc, behavior, parameters, CancellationToken.None);
        }
		public static async Task<IList<T>> ExecuteReaderCommandAsync<T>(this IDb db, string sproc, object parameters, CancellationToken cancellation)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = null as IList<T>;
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters, db.AutoNullEmptyStrings);
                
                result = await DbCommandExtensions.ExecuteReaderAsync<T>(cmd, cancellation);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
        public static async Task<T> ExecuteSingleCommandAsync<T>(this IDb db, string sproc, object parameters, CancellationToken cancellation)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = default(T);
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters, db.AutoNullEmptyStrings);
                
                result = await DbCommandExtensions.ExecuteSingleAsync<T>(cmd, cancellation);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
		public static Task<IList<T>> ExecuteReaderCommandAsync<T>(this IDb db, string sproc, object parameters = null)
        {
            return ExecuteReaderCommandAsync<T>(db, sproc, parameters, CancellationToken.None);
        }
        public static Task<T> ExecuteSingleCommandAsync<T>(this IDb db, string sproc, object parameters = null)
        {
            return ExecuteSingleCommandAsync<T>(db, sproc, parameters, CancellationToken.None);
        }
		public static async Task<IList<T>> ExecuteReaderSqlAsync<T>(this IDb db, string query, CommandBehavior behavior, Func<IDataReader, T> fn, object parameters, CancellationToken cancellation)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = null as IList<T>;
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(query, CommandType.Text, parameters, db.AutoNullEmptyStrings);
                
                result = await DbCommandExtensions.ExecuteReaderAsync<T>(cmd, behavior, fn, cancellation);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
        public static async Task<T> ExecuteSingleSqlAsync<T>(this IDb db, string query, CommandBehavior behavior, Func<IDataReader, T> fn, object parameters, CancellationToken cancellation)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = default(T);
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(query, CommandType.Text, parameters, db.AutoNullEmptyStrings);
                
                result = await DbCommandExtensions.ExecuteSingleAsync<T>(cmd, behavior, fn, cancellation);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
		public static Task<IList<T>> ExecuteReaderSqlAsync<T>(this IDb db, string query, CommandBehavior behavior, Func<IDataReader, T> fn, object parameters = null)
        {
            return ExecuteReaderSqlAsync<T>(db, query, behavior, fn, parameters, CancellationToken.None);
        }
        public static Task<T> ExecuteSingleSqlAsync<T>(this IDb db, string query, CommandBehavior behavior, Func<IDataReader, T> fn, object parameters = null)
        {
            return ExecuteSingleSqlAsync<T>(db, query, behavior, fn, parameters, CancellationToken.None);
        }
		public static async Task<IList<T>> ExecuteReaderSqlAsync<T>(this IDb db, string query, Func<IDataReader, T> fn, object parameters, CancellationToken cancellation)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = null as IList<T>;
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(query, CommandType.Text, parameters, db.AutoNullEmptyStrings);
                
                result = await DbCommandExtensions.ExecuteReaderAsync<T>(cmd, fn, cancellation);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
        public static async Task<T> ExecuteSingleSqlAsync<T>(this IDb db, string query, Func<IDataReader, T> fn, object parameters, CancellationToken cancellation)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = default(T);
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(query, CommandType.Text, parameters, db.AutoNullEmptyStrings);
                
                result = await DbCommandExtensions.ExecuteSingleAsync<T>(cmd, fn, cancellation);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
		public static Task<IList<T>> ExecuteReaderSqlAsync<T>(this IDb db, string query, Func<IDataReader, T> fn, object parameters = null)
        {
            return ExecuteReaderSqlAsync<T>(db, query, fn, parameters, CancellationToken.None);
        }
        public static Task<T> ExecuteSingleSqlAsync<T>(this IDb db, string query, Func<IDataReader, T> fn, object parameters = null)
        {
            return ExecuteSingleSqlAsync<T>(db, query, fn, parameters, CancellationToken.None);
        }
		public static async Task<IList<T>> ExecuteReaderSqlAsync<T>(this IDb db, string query, CommandBehavior behavior, object parameters, CancellationToken cancellation)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = null as IList<T>;
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(query, CommandType.Text, parameters, db.AutoNullEmptyStrings);
                
                result = await DbCommandExtensions.ExecuteReaderAsync<T>(cmd, behavior, cancellation);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
        public static async Task<T> ExecuteSingleSqlAsync<T>(this IDb db, string query, CommandBehavior behavior, object parameters, CancellationToken cancellation)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = default(T);
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(query, CommandType.Text, parameters, db.AutoNullEmptyStrings);
                
                result = await DbCommandExtensions.ExecuteSingleAsync<T>(cmd, behavior, cancellation);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
		public static Task<IList<T>> ExecuteReaderSqlAsync<T>(this IDb db, string query, CommandBehavior behavior, object parameters = null)
        {
            return ExecuteReaderSqlAsync<T>(db, query, behavior, parameters, CancellationToken.None);
        }
        public static Task<T> ExecuteSingleSqlAsync<T>(this IDb db, string query, CommandBehavior behavior, object parameters = null)
        {
            return ExecuteSingleSqlAsync<T>(db, query, behavior, parameters, CancellationToken.None);
        }
		public static async Task<IList<T>> ExecuteReaderSqlAsync<T>(this IDb db, string query, object parameters, CancellationToken cancellation)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = null as IList<T>;
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(query, CommandType.Text, parameters, db.AutoNullEmptyStrings);
                
                result = await DbCommandExtensions.ExecuteReaderAsync<T>(cmd, cancellation);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
        public static async Task<T> ExecuteSingleSqlAsync<T>(this IDb db, string query, object parameters, CancellationToken cancellation)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = default(T);
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(query, CommandType.Text, parameters, db.AutoNullEmptyStrings);
                
                result = await DbCommandExtensions.ExecuteSingleAsync<T>(cmd, cancellation);

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
		public static Task<IList<T>> ExecuteReaderSqlAsync<T>(this IDb db, string query, object parameters = null)
        {
            return ExecuteReaderSqlAsync<T>(db, query, parameters, CancellationToken.None);
        }
        public static Task<T> ExecuteSingleSqlAsync<T>(this IDb db, string query, object parameters = null)
        {
            return ExecuteSingleSqlAsync<T>(db, query, parameters, CancellationToken.None);
        }
        public static int ExecuteNonQueryCommand(this IDb db, string sproc, object parameters = null)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = -1;
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters, db.AutoNullEmptyStrings);
                var obj = DbCommandExtensions.Execute(cmd, false);
                
                result = (int)System.Convert.ChangeType(obj, typeof(System.Int32));

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

			return result;
        }
		public static object ExecuteScalerCommand(this IDb db, string sproc, object parameters = null)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            object result = null;
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters, db.AutoNullEmptyStrings);
                
                result = cmd.Execute(true);

                cmd.ApplyOutputs(parameters);

                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
        public static int ExecuteNonQuerySql(this IDb db, string query, object parameters = null)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = -1;
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(query, CommandType.Text, parameters, db.AutoNullEmptyStrings);
                var obj = DbCommandExtensions.Execute(cmd, false);
                
                result = (int)System.Convert.ChangeType(obj, typeof(System.Int32));

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

			return result;
        }
		public static object ExecuteScalerSql(this IDb db, string query, object parameters = null)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            object result = null;
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(query, CommandType.Text, parameters, db.AutoNullEmptyStrings);
                
                result = cmd.Execute(true);

                cmd.ApplyOutputs(parameters);

                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
        public static async Task<int> ExecuteNonQueryCommandAsync(this IDb db, string sproc, object parameters, CancellationToken cancellation)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = -1;
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters, db.AutoNullEmptyStrings);
                var obj = await DbCommandExtensions.ExecuteAsync(cmd, false, cancellation);
                
                result = (int)System.Convert.ChangeType(obj, typeof(System.Int32));

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

			return result;
        }
		public static async Task<object> ExecuteScalerCommandAsync(this IDb db, string sproc, object parameters, CancellationToken cancellation)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            object result = null;
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(sproc, CommandType.StoredProcedure, parameters, db.AutoNullEmptyStrings);
                
                result = await cmd.ExecuteAsync(true, cancellation);

                cmd.ApplyOutputs(parameters);

                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
		public static Task<int> ExecuteNonQueryCommandAsync(this IDb db, string sproc, object parameters = null)
        {
            return ExecuteNonQueryCommandAsync(db, sproc, parameters, CancellationToken.None);
        }
		public static Task<object> ExecuteScalerCommandAsync(this IDb db, string sproc, object parameters = null)
        {
            return ExecuteScalerCommandAsync(db, sproc, parameters, CancellationToken.None);
        }
        public static async Task<int> ExecuteNonQuerySqlAsync(this IDb db, string query, object parameters, CancellationToken cancellation)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            var result = -1;
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(query, CommandType.Text, parameters, db.AutoNullEmptyStrings);
                var obj = await DbCommandExtensions.ExecuteAsync(cmd, false, cancellation);
                
                result = (int)System.Convert.ChangeType(obj, typeof(System.Int32));

                cmd.ApplyOutputs(parameters);
            
                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

			return result;
        }
		public static async Task<object> ExecuteScalerSqlAsync(this IDb db, string query, object parameters, CancellationToken cancellation)
        {
            if (db == null)
	        {
		        throw new DbNullException();
	        }

            object result = null;
            var con = db.GetConnection();

            if (con != null)
            {
                var cmd = con.CreateCommand(query, CommandType.Text, parameters, db.AutoNullEmptyStrings);
                
                result = await cmd.ExecuteAsync(true, cancellation);

                cmd.ApplyOutputs(parameters);

                if (!db.PersistConnection)
			    {
                    cmd.Dispose();
				    con.Dispose();
			    }
            }

            return result;
        }
		public static Task<int> ExecuteNonQuerySqlAsync(this IDb db, string query, object parameters = null)
        {
            return ExecuteNonQuerySqlAsync(db, query, parameters, CancellationToken.None);
        }
		public static Task<object> ExecuteScalerSqlAsync(this IDb db, string query, object parameters = null)
        {
            return ExecuteScalerSqlAsync(db, query, parameters, CancellationToken.None);
        }
	}
}