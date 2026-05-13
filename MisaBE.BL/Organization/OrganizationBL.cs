using MisaBE.BL.Base;
using MisaBE.Common.DTOs;
using MisaBE.Common.Entities;
using MisaBE.DL.Organization;

namespace MisaBE.BL.Organization
{
    public class OrganizationBL : BaseBL<Organization>, IOrganizationBL
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
