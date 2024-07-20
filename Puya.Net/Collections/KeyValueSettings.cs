using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.Collections
{
    public class KeyValueSettings : CaseInsensitiveDictionary<string>
    {
        public KeyValueSettings(): base(true)
        {
        }
    }
}
