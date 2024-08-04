using Puya.Conversion;
using Puya.Data;
using System.Threading.Tasks;
using System.Threading;

namespace Puya.Data
{
    public class SqlServerSequenceManager : ISequenceManager
    {
        private readonly IDb db;

        public SqlServerSequenceManager(IDb db)
        {
            this.db = db;
        }
        public async Task<bool> CreateAsync(string name, SequenceCreateOptions options, CancellationToken cancellation)
        {
            var _options = options ?? new SequenceCreateOptions();
            var _name = $"{_options.Schema}.{name}";
            var query = $@"
if not exists(select 1 from sys.objects where object_id = OBJECT_ID(N'{_name}') and type = 'SO')
begin
    create sequence {_name} start with {_options.Start} increment by {_options.Increment}

    select 1
end
else
    select 0
";
            var result = await db.ExecuteScalerSqlAsync(query, null, cancellation);

            return SafeClrConvert.ToBoolean(result);
        }
        public async Task<bool> AlterAsync(string name, SequenceCreateOptions options, CancellationToken cancellation)
        {
            var _options = options ?? new SequenceCreateOptions();
            var _name = $"{_options.Schema}.{name}";
            var query = $@"
if exists(select 1 from sys.objects where object_id = OBJECT_ID(N'{_name}') and type = 'SO')
begin
    alter sequence {_name} restart with {_options.Start} increment by {_options.Increment}

    select 1
end
else
    select 0
";
            var result = await db.ExecuteScalerSqlAsync(query, null, cancellation);

            return SafeClrConvert.ToBoolean(result);
        }
        public async Task DropAsync(string name, CancellationToken cancellation)
        {
            var query = $"drop sequence if exists {name}";

            await db.ExecuteNonQuerySqlAsync(query, null, cancellation);
        }
        public async Task<object> NextAsync(string name, SequenceCreateOptions options, CancellationToken cancellation)
        {
            var _options = options ?? new SequenceCreateOptions();
            var _name = $"{_options.Schema}.{name}";
            var value = await db.ExecuteScalerSqlAsync($"select next value for {_name}", null, cancellation);

            return value;
        }
    }
}
