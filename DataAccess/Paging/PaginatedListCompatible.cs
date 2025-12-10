using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Paging
{
    public class PaginatedListCompatible<T> : List<T>, IReadOnlyCollection<T>
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public bool HasPreviousPage => (PageIndex > 1);
        public bool HasNextPage => (PageIndex < TotalPages);

        public PaginatedListCompatible(List<T> items, int count, int pageIndex, int pageSize)
        {
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageIndex = pageIndex;
            PageSize = pageSize;

            this.AddRange(items);
        }
    }
}
