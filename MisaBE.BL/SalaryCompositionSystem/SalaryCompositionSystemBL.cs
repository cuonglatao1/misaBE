using MisaBE.BL.Base;
using MisaBE.DL.SalaryCompositionSystem;
using SalaryCompositionSystemEntity = MisaBE.Common.Entities.SalaryCompositionSystem;

namespace MisaBE.BL.SalaryCompositionSystem
{
    public class SalaryCompositionSystemBL : BaseBL<SalaryCompositionSystemEntity>, ISalaryCompositionSystemBL
    {
        public SalaryCompositionSystemBL(ISalaryCompositionSystemDL dl) : base(dl) { }
    }
}
