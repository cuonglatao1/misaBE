using MisaBE.BL.Base;
using MisaBE.Common.DTOs;

namespace MisaBE.BL.SalaryComposition
{
    public interface ISalaryCompositionBL : IBaseBL<Common.Entities.SalaryComposition>
    {
        Task<ServiceResult> CloneAsync(string id);
        Task<ServiceResult> ChangeStatusAsync(string id, string status);
        Task<ServiceResult> AddFromSystemAsync(string systemId);
    }
}
