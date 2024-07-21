using System.Collections.Generic;

namespace Puya.Service
{
    public class PagingResult<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public long RecordCount { get; set; }
        public IList<T> Items { get; set; }
    }
}
