using System.Threading.Tasks;
using System.Threading;

namespace Puya.Data
{
    public interface ISequenceManager
    {
        Task<bool> AlterAsync(string name, SequenceCreateOptions options, CancellationToken cancellation);
        Task<bool> CreateAsync(string name, SequenceCreateOptions options, CancellationToken cancellation);
        Task DropAsync(string name, CancellationToken cancellation);
        Task<object> NextAsync(string name, SequenceCreateOptions options, CancellationToken cancellation);
    }
}
