using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MYWEBAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("You have accessed the User controller");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getadmin")]
        public IActionResult Get(int id)
        {
            return Ok("This is from User -> Admin id");
        }
    }
}