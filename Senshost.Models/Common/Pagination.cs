using Senshost.Models.Constants;

namespace Senshost.Models.Common
{
    public class Pagination
    {
        public int PageSize { get; set; } = 20;
        public int PageNumber { get; set; } = 1;
        public DataSorting Sort { get; set; } = DataSorting.Descending;
    }

    public class PaginationResult : Pagination
    {
        public PaginationResult() { }

        public PaginationResult(int pageSize)
        {
            PageSize = pageSize;
        }

        public long TotalRecord { get; set; }
        public long NumberOfPage { get; set; }
    }

    public class DataResponse<T>
    {
        public T Data { get; set; }
        public PaginationResult Pagination { get; set; }
    }
}
