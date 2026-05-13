using MisaBE.BL.Base;
using MisaBE.Common.DTOs;
using MisaBE.DL.Organization;
using OrganizationEntity = MisaBE.Common.Entities.Organization;

namespace MisaBE.BL.Organization
{
    public class OrganizationBL : BaseBL<OrganizationEntity>, IOrganizationBL
    {
        private readonly IOrganizationDL _orgDL;

        public OrganizationBL(IOrganizationDL dl) : base(dl)
        {
            _orgDL = dl;
        }

        public async Task<ServiceResult> GetAllActiveAsync()
        {
            var data = await _orgDL.GetAllActiveAsync();
            return ServiceResult.Ok(data);
        }
    }
}
