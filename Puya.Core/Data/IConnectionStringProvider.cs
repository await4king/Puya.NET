using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Data
{
    public interface IConnectionStringProvider
    {
        string GetConnectionString();
        void SetConnectionString(string constr);
    }
}
