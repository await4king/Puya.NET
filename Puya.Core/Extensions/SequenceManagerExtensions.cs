using Puya.Data;
using System.Threading.Tasks;
using System.Threading;

namespace Puya.Extensions
{
    public static class SequenceManagerExtensions
    {
        public static Task<bool> CreateAsync(this ISequenceManager sequenceManager, string name, int start = 1, int increment = 1)
        {
            return sequenceManager.CreateAsync(name, new SequenceCreateOptions { Start = start, Increment = increment }, CancellationToken.None);
        }
        public static Task<bool> ResetAsync(this ISequenceManager sequenceManager, string name, int start = 1, int increment = 1)
        {
            return sequenceManager.AlterAsync(name, new SequenceCreateOptions { Start = start, Increment = increment }, CancellationToken.None);
        }
        public static Task<object> NextAsync(this ISequenceManager sequenceManager, string name)
        {
            return sequenceManager.NextAsync(name, new SequenceCreateOptions { }, CancellationToken.None);
        }
        public static Task DropAsync(this ISequenceManager sequenceManager, string name)
        {
            return sequenceManager.DropAsync(name, CancellationToken.None);
        }
    }
}
