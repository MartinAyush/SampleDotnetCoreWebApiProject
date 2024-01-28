using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Api.Models;
using NZWalks.Api.Models.DTO;
using NZWalks.Api.Repositories;

namespace NZWalks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this._userManager = userManager;
            this._tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser()
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };

            var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                // Assign roles to this user
                if(registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                    identityResult = await _userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                if (identityResult.Succeeded)
                {
                    return Ok("User registered, please login!");
                }
            }

            return BadRequest("Something went wrong.");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var identityUser = await _userManager.FindByEmailAsync(loginRequestDto.Username);

            if(identityUser != null)
            {
                bool checkPasswordResult = await _userManager.CheckPasswordAsync(identityUser, loginRequestDto.Password);
                if (checkPasswordResult)
                {
                    // create token
                    var roles = await _userManager.GetRolesAsync(identityUser);
                    if(roles != null)
                    {
                        var token = _tokenRepository.CreateToken(identityUser, roles.ToList());

                        var response = new LoginResponseDto
                        {
                            JwtToken = token
                        };

                        return Ok(response);
                    }
                }
            }

            return BadRequest("username or password is incorrect");
        }
    }
}
