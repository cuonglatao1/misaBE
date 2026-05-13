using MisaBE.Common.Entities;
using MisaBE.DL.Base;

namespace MisaBE.DL.SalaryComposition
{
    public interface ISalaryCompositionDL : IBaseDL<Common.Entities.SalaryComposition>
    {
        Task<int> UpdateStatusAsync(string id, string status);
    }
}
