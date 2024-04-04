using WebAPI.Wrappers;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Models;

namespace WebAPI.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    public IdentityController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _configuration = configuration;
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register(RegisterModel register)
    {
        var userExists = await _userManager.FindByNameAsync(register.Username);
        if (userExists != null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Response<bool>
            {
                Succeeded = false,
                Message = "User already exists!"
            });
        }

        ApplicationUser user = new ApplicationUser()
        {
            Email = register.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = register.Username
        };

        var result = await _userManager.CreateAsync(user, register.Password);
        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Response<bool>
            {
                Succeeded = false,
                Message = "User creation failed! Please check user details and try again",
                Errors = result.Errors.Select(x => x.Description)
            });
        }

        if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

        await _userManager.AddToRoleAsync(user, UserRoles.User);

        //await _emailSenderService.Send(user.Email, "Regisstration confirmation");

        return Ok(new Response<bool> { Succeeded = true, Message = "User created successfully!" });
    }

    [HttpPost]
    [Route("RegisterAdmin")]
    public async Task<IActionResult> RegisterAdmin(RegisterModel register)
    {
        var userExists = await _userManager.FindByNameAsync(register.Username);
        if (userExists != null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Response<bool>
            {
                Succeeded = false,
                Message = "User slready exists!"
            });
        }

        ApplicationUser user = new ApplicationUser()
        {
            Email = register.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = register.Username
        };

        var result = await _userManager.CreateAsync(user, register.Password);
        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new Response<bool>
            {
                Succeeded = false,
                Message = "User creation failed! Please check user details and try again",
                Errors = result.Errors.Select(x => x.Description)
            });
        }

        if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

        await _userManager.AddToRoleAsync(user, UserRoles.Admin);

        return Ok(new Response<bool> { Succeeded = true, Message = "User created successfully!" });
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login(LoginModel login)
    {
        var user = await _userManager.FindByNameAsync(login.Username);
        if (user != null && await _userManager.CheckPasswordAsync(user, login.Password))
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddHours(2),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
        return Unauthorized();
    }
}

