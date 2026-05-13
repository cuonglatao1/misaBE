namespace MisaBE.Common.Entities
{
    public class SalaryComposition
    {
        public string CompositionId { get; set; } = Guid.NewGuid().ToString();
        public string CompositionCode { get; set; } = string.Empty;
        public string CompositionName { get; set; } = string.Empty;
        public string? OrganizationId { get; set; }
        /// <summary>income | deduction | attendance | sales | employee_info | other</summary>
        public string ComponentType { get; set; } = string.Empty;
        /// <summary>fixed | variable | formula</summary>
        public string Nature { get; set; } = string.Empty;
        /// <summary>taxable | exempt_full | exempt_partial</summary>
        public string TaxOption { get; set; } = "taxable";
        public bool AllowExceedQuota { get; set; }
        public string? Quota { get; set; }
        /// <summary>money | number | percent | text</summary>
        public string ValueType { get; set; } = "money";
        /// <summary>auto | manual</summary>
        public string ValueSource { get; set; } = "manual";
        public string? ValueFormula { get; set; }
        public string? Description { get; set; }
        /// <summary>yes | no | nonzero</summary>
        public string ShowOnPayslip { get; set; } = "yes";
        /// <summary>system | manual | default</summary>
        public string SourceType { get; set; } = "manual";
        public string? SystemId { get; set; }
        /// <summary>active | inactive</summary>
        public string Status { get; set; } = "active";
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        // JOIN field - not stored in this table
        public string? OrganizationName { get; set; }
    }
}
