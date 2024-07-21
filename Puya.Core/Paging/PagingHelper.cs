using System;
using System.Collections.Generic;
using System.Text;
using Puya.Conversion;

namespace Puya.Paging
{
    public class PagingOptions
    {
        public int MaxPageSize { get; set; }
        public int MaxVisiblePages { get; set; }
        public int DefaultPage { get; set; }
        public int DefaultPageSize { get; set; }
        public int DefaultVisiblePages { get; set; }
    }
    public static class PagingHelper
    {
        public static PageResult PagingCalc(object recordcount, object page, object pagesize, object visiblePages = null, PagingOptions options = null)
        {
            var _options = options ?? new PagingOptions();

            if (_options.MaxPageSize < 0)
            {
                _options.MaxPageSize = 500;
            }

            if (_options.MaxVisiblePages < 0)
            {
                _options.MaxVisiblePages = 50;
            }

            if (_options.DefaultPage < 0)
            {
                _options.DefaultPage = 1;
            }

            if (_options.DefaultPageSize < 0)
            {
                _options.DefaultPageSize = 10;
            }

            if (_options.DefaultVisiblePages < 0)
            {
                _options.DefaultVisiblePages = 10;
            }

            var result = new PageResult {
                Page = SafeClrConvert.ToInt(page, _options.DefaultPage),
                PageSize = SafeClrConvert.ToInt(pagesize, _options.DefaultPageSize),
                VisiblePages = SafeClrConvert.ToInt(visiblePages, _options.DefaultVisiblePages),
                RecordCount = SafeClrConvert.ToLong(recordcount)
            };

            if (result.Page < 1)
            {
                result.Page = _options.DefaultPage;
            }

            if (result.PageSize < 1 || result.PageSize > _options.MaxPageSize)
            {
                result.PageSize = _options.DefaultPageSize;
            }

            if (result.RecordCount < 0)
            {
                result.RecordCount = 0;
            }

            result.PageCount = (int)(result.RecordCount / result.PageSize);

            if (result.RecordCount > result.PageCount * result.PageSize)
            {
                result.PageCount++;
            }

            if (result.Page > result.PageCount)
            {
                result.Page = result.PageCount;
            }

            if (result.VisiblePages < 0 || result.VisiblePages > _options.MaxVisiblePages)
            {
                result.VisiblePages = _options.DefaultVisiblePages;
            }

            result.FromPage = (int)Math.Ceiling(result.Page / result.VisiblePages * 1.0);
            result.FromPage = (result.FromPage - 1) * result.VisiblePages + 1;
            result.ToPage = result.FromPage + result.VisiblePages - 1;

            if (result.ToPage > result.PageCount)
            {
                result.ToPage = result.PageCount;
            }

            result.FromRow = (result.Page - 1) * result.PageSize + 1;
            result.ToRow = result.FromRow + result.PageSize - 1;

            if (result.ToRow > result.RecordCount)
            {
                result.ToRow = result.RecordCount;
            }

            return result;
        }
    }
}
