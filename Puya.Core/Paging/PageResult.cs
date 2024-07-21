using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.Paging
{
    public class PageResult
    {
        public int Page { get; set; }
        public int PageCount { get; set; }
        public long RecordCount { get; set; }
        public int PageSize { get; set; }
        public int VisiblePages { get; set; }
        public long FromRow { get; set; }
        public long ToRow { get; set; }
        public int FromPage { get; set; }
        public int ToPage { get; set; }
    }
    public class PageResult<T> : PageResult
    {
        private List<T> data;
        public List<T> Data
        {
            get
            {
                if (data == null)
                    data = new List<T>();
                return data;
            }
            set { data = value; }
        }
        public void Copy(PageResult paging)
        {
            this.Page = paging.Page;
            this.RecordCount = paging.RecordCount;
            this.PageSize = paging.PageSize;
            this.PageCount = paging.PageCount;
            this.FromPage = paging.FromPage;
            this.ToPage = paging.ToPage;
            this.FromRow = paging.FromRow;
            this.ToRow = paging.ToRow;
            this.VisiblePages = paging.VisiblePages;
        }
    }
}
