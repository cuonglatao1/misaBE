namespace MisaBE.Common.DTOs
{
    public class PagingRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SearchText { get; set; }
        public string? OrganizationId { get; set; }
        public string? Status { get; set; }
        public string? SortField { get; set; }
        public string SortOrder { get; set; } = "asc";
    }
}
