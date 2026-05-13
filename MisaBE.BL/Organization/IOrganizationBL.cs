using MisaBE.BL.Base;
using MisaBE.Common.DTOs;
using MisaBE.Common.Entities;

namespace MisaBE.BL.Organization
{
    public interface IOrganizationBL : IBaseBL<Organization>
    {
        Task<ServiceResult> GetAllActiveAsync();
    }
}
