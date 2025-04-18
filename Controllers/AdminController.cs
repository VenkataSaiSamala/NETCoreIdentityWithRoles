using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MYWEBAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("You have accessed the Admin controller");
        }

        [Authorize(Roles = "User")]
        [HttpGet("getuser")]
        public IActionResult GetUser()
        {
            return Ok("This is from Admin -> User id");
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("getmanager")]
        public IActionResult GetManager()
        {
            return Ok("This is from Admin -> Manager id");
        }
    }
}