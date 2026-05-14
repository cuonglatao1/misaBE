using MisaBE.DL.Base;
using OrganizationEntity = MisaBE.Common.Entities.Organization;

namespace MisaBE.DL.Organization
{
    public interface IOrganizationDL : IBaseDL<OrganizationEntity>
    {
        Task<IEnumerable<OrganizationEntity>> GetAllActiveAsync();
    }
}
