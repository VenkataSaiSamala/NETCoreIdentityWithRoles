using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyWebApi.Models;

namespace MYWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            var user = new IdentityUser {UserName = model.Username};
            var result = await _userManager.CreateAsync(user, model.Password);

            if(result.Succeeded){
                return Ok(new {Message = "Registration sucessfull !!!"});
            }

            return BadRequest(result.Errors);
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if(user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"]!)),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                    SecurityAlgorithms.HmacSha256
                    )
                );
                return Ok(new {Token=new JwtSecurityTokenHandler().WriteToken(token)});
            }

            return Unauthorized();
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody]string role)
        {
            if(!await _roleManager.RoleExistsAsync(role)){
                var result = await _roleManager.CreateAsync(new IdentityRole(role));
                if(result.Succeeded){
                    return Ok(new {message = "Role Added Successfully!!!"});
                }
                return BadRequest(result.Errors);
            }
            return BadRequest("Role already Exists!!!");
        }


        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] UserRoles model){
            var user = await _userManager.FindByNameAsync(model.Username);

            if(user==null){
                return BadRequest("User not found");
            }

            var result = await _userManager.AddToRoleAsync(user, model.Role);
            if(result.Succeeded){
                return Ok(new {Message = "Role Assigned Successfully!!!"});
            }
            return BadRequest(result.Errors);
        }
       

        [HttpPost("unassign-role")]
        public async Task<IActionResult> UnassignRole([FromBody] UserRoles model){
            var user = await _userManager.FindByNameAsync(model.Username);

            if(user==null){
                return BadRequest("User not found");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, model.Role);
            if(result.Succeeded){
                return Ok(new {Message = "Role Unassigned Successfully!!!"});
            }
            return BadRequest(result.Errors);
        }

        [HttpGet("roles")]
        public async Task<IEnumerable<string>> UnassignRole([FromBody] string username){
            var user = await _userManager.FindByNameAsync(username);

          

            var roles = await _userManager.GetRolesAsync(user!);
            return roles;

            
        }
    }
}
