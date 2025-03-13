using Microsoft.AspNetCore.Mvc;

namespace SurfSwift.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class SSBaseController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok("init");
        }
    }
}
