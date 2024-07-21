using System;
using System.Collections.Generic;
using System.Text;
using Puya.Base;

namespace Puya.Caching
{
    public class Cache : InstanceProvider<ICache, NullCache>
    { }
}
