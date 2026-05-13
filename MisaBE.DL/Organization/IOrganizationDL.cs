using MisaBE.Common.Entities;
using MisaBE.DL.Base;

namespace MisaBE.DL.Organization
{
    public interface IOrganizationDL : IBaseDL<Organization>
    {
        Task<IEnumerable<Organization>> GetAllActiveAsync();
    }
}
