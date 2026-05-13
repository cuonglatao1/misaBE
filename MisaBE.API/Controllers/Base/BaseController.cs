using Microsoft.AspNetCore.Mvc;
using MisaBE.Common.DTOs;

namespace MisaBE.API.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult HandleResult(ServiceResult result)
        {
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        protected IActionResult NotFoundResult(string message = "Không tìm thấy dữ liệu")
            => NotFound(ServiceResult.Fail(message));
    }
}
