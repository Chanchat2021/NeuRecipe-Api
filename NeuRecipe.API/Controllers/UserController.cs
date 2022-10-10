using Microsoft.AspNetCore.Mvc;
using NeuRecipe.Application.DTO;
using NeuRecipe.Application.Services.Interfaces;

namespace NeuRecipe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> CreateUser(UserDTO user)
        {
            var result = await userService.CreateUser(user);
            return StatusCode(201, result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> UserLogIn(Login loginInfo)
        {
            var result = await userService.UserLogIn(loginInfo.Email, loginInfo.Password);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await userService.GetUsers();
            return Ok(users);
        }
    }
}
