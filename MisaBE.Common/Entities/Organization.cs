namespace MisaBE.Common.Entities
{
    public class Organization
    {
        public string OrganizationId { get; set; } = Guid.NewGuid().ToString();
        public string OrganizationCode { get; set; } = string.Empty;
        public string OrganizationName { get; set; } = string.Empty;
        public string? ParentId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
