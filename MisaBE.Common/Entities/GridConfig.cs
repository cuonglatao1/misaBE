namespace MisaBE.Common.Entities
{
    public class GridConfig
    {
        public string ConfigId { get; set; } = Guid.NewGuid().ToString();
        public string GridId { get; set; } = string.Empty;
        public string ColumnField { get; set; } = string.Empty;
        public string ColumnCaption { get; set; } = string.Empty;
        public int ColumnWidth { get; set; } = 150;
        public bool IsVisible { get; set; } = true;
        public bool IsFixed { get; set; }
        /// <summary>left | right</summary>
        public string? FixedPosition { get; set; }
        public int SortOrder { get; set; }
        public string? UserId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
