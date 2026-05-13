namespace MisaBE.Common.Entities
{
    public class SalaryCompositionSystem
    {
        public string SystemId { get; set; } = Guid.NewGuid().ToString();
        public string SystemCode { get; set; } = string.Empty;
        public string SystemName { get; set; } = string.Empty;
        public string ComponentType { get; set; } = string.Empty;
        public string Nature { get; set; } = string.Empty;
        public string TaxOption { get; set; } = "taxable";
        public string? Quota { get; set; }
        public string ValueType { get; set; } = "money";
        public string? ValueFormula { get; set; }
        public string? Description { get; set; }
        public string ShowOnPayslip { get; set; } = "yes";
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
