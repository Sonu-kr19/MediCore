
namespace MediCore.Api.DTOs.Common
{
    //This wraps the list + pagination info
    public class PaginationResponseDto<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<T> Data { get; set; }
    }
}
