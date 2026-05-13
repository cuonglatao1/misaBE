using Microsoft.AspNetCore.Mvc;
using MisaBE.API.Controllers.Base;
using MisaBE.BL.Organization;

namespace MisaBE.API.Controllers
{
    [Route("api/organizations")]
    public class OrganizationController : BaseController
    {
        private readonly IOrganizationBL _bl;

        public OrganizationController(IOrganizationBL bl) => _bl = bl;

        /// <summary>Lấy tất cả đơn vị đang hoạt động (dùng cho dropdown lọc)</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _bl.GetAllActiveAsync();
            return HandleResult(result);
        }
    }
}
