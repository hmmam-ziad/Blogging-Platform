using Blogging_Platform_Backend.DTOModels;
using Blogging_Platform_Backend.Models;
using Blogging_Platform_Backend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blogging_Platform_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly IConfiguration _config;

        public AccountController(UserManager<UserApp> usermanager, IConfiguration config)
        {
            _userManager = usermanager;
            _config = config;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register(dtoRegister register)
        {
            if (ModelState.IsValid)
            {
                if(await isExist(register)) 
                    return BadRequest("Email or UserName or Phonenumber already exists.");
                UserApp user = new UserApp
                {
                    UserName = register.UserName,
                    Email = register.Email,
                    FullName = register.FullName,
                    PhoneNumber = register.PhoneNumber,
                };
                IdentityResult result = await _userManager.CreateAsync(user, register.Password);
                if (result.Succeeded)
                {
                    var jwtService = new JwtService(_config, _userManager);
                    var tokenResult = await jwtService.GenerateJwtTokenAsync(user);
                    return Ok(tokenResult);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return BadRequest(ModelState);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(dtoLogin login)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(login.usernameEmail)
                    ?? await _userManager.FindByEmailAsync(login.usernameEmail);
                if(user is not null)
                {
                    if (await _userManager.CheckPasswordAsync(user, login.password))
                    {
                        var jwtService = new JwtService(_config, _userManager);
                        var tokenResult = await jwtService.GenerateJwtTokenAsync(user);
                        return Ok(tokenResult);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return Unauthorized(ModelState);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return Unauthorized(ModelState);
                }
            }
            return BadRequest(ModelState);
        }

        private async Task<bool> isExist(dtoRegister register)
        {
            return (await _userManager.FindByNameAsync(register.UserName) is not null || await _userManager.FindByEmailAsync(register.Email) is not null
                    || await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == register.PhoneNumber) is not null);
        }
    }
}
