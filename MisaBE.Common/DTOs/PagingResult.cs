namespace MisaBE.Common.DTOs
{
    public class PagingResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalRecords { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => PageSize > 0
            ? (int)Math.Ceiling((double)TotalRecords / PageSize)
            : 0;
    }
}
