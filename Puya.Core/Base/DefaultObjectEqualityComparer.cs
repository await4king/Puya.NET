using System;
using System.Collections;

namespace Puya.Base
{
    public class DefaultObjectEqualityComparer : IEqualityComparer
    {
        public new bool Equals(object x, object y)
        {
            return Object.Equals(x, y);
        }

        public int GetHashCode(object obj)
        {
            return obj?.GetHashCode() ?? 0;
        }
    }
}
