using Microsoft.AspNetCore.Mvc;
using SuperDoc.Customer.API.Authorization;
using SuperDoc.Customer.Repositories.Entities.Users;
using SuperDoc.Customer.Services.Users;
using SuperDoc.Shared.Models.Users;

namespace SuperDoc.Customer.API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ILoginService loginService;

        public UserController(IUserService userService, ILoginService loginService)
        {
            this.userService = userService;
            this.loginService = loginService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUser([FromBody] CrerateUserDto crerateUserDto)
        {
            if (crerateUserDto.Role < 0 || crerateUserDto.Role > 3)
            {
                return BadRequest("Invalid role");
            }


            if (ModelState.IsValid)
            {
                int result = await userService.AddUserAsync(crerateUserDto.FirstName, crerateUserDto.LastName, crerateUserDto.EmailAddress, crerateUserDto.PhoneCode, crerateUserDto.PhoneNumber, crerateUserDto.Password, (Roles)crerateUserDto.Role);

                if (result == 1)
                {
                    return BadRequest("Email is already used");
                }

                return Ok();
            }

            return BadRequest("Invalid input");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            if (ModelState.IsValid)
            {
                User? user = await userService.GetUserByEmailAndPasswordAsync(login.Email, login.Password);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(loginService.GenerateToken(user));
            }

            return BadRequest("Invalid input");

        }
    }
}
