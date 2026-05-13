using MisaBE.BL.Base;
using MisaBE.Common.Entities;
using MisaBE.DL.SalaryCompositionSystem;

namespace MisaBE.BL.SalaryCompositionSystem
{
    public class SalaryCompositionSystemBL : BaseBL<SalaryCompositionSystem>, ISalaryCompositionSystemBL
    {
        public SalaryCompositionSystemBL(ISalaryCompositionSystemDL dl) : base(dl) { }
    }
}
