using Microsoft.AspNetCore.Mvc;
using LogBookAPI.Interfaces;
using LogBookAPI.Models;
using LogBookAPI.Models.Authentication;
namespace LogBookAPI.Controllers
{
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public AuthenticationController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationModel model)
        {
            var response = await _userServices.Authenticate(model);

            if (response == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return Ok(response);

        }
    }
}