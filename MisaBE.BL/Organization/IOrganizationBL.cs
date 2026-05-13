using MisaBE.BL.Base;
using MisaBE.Common.DTOs;
using OrganizationEntity = MisaBE.Common.Entities.Organization;

namespace MisaBE.BL.Organization
{
    public interface IOrganizationBL : IBaseBL<OrganizationEntity>
    {
        Task<ServiceResult> GetAllActiveAsync();
    }
}
