using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ChatZorro.Services;
using System.Threading.Tasks;
using ChatZorro.Models;
using ChatZorro.Entities;

namespace ChatZorro.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateModel model)
        {
            try
            {
                var profile = await _userService.Authenticate(model.Username, model.Password);

                if (profile == null)
                    return BadRequest(new { message = "Username or password is incorrect" });

                return Ok(profile);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] Models.UserModel model)
        {
            try
            {
                var result = await _userService.Registration(model);

                if(result == null)
                    return BadRequest(new { message = "An error has occurred" });

                return Ok("Success.");
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("contacts")]
        public IActionResult Contacts(string key)
        {

            return Ok(new User().GetContactList());
        }
    }
}