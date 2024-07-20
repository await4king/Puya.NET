using System.Collections.Generic;
using Puya.Collections;

namespace Puya.Api
{
    public class JsonConfig<T> where T: ISettingable
    {
        public KeyValueSettings Settings { get; set; }
        public List<T> Data { get; set; }
    }
}
