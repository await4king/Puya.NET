using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Service
{
    public interface IService
    {
        string Name { get; }
        object GetAction(string name);
        object this[string action] { get; }
    }
    public interface IService<TConfig>: IService
        where TConfig : class, IServiceConfig, new()
    {
        TConfig Config { get; }
    }
}
