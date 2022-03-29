using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IEmailSender _sender;
        private readonly IConfiguration _configuration;

        public AccountController(ILogger<AccountController> logger, UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager, IMapper mapper, IEmailSender sender,IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _sender = sender;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterModel rm)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = _mapper.Map<IdentityUser>(rm);
                user.UserName = rm.Email;
                IdentityResult res = await _userManager.CreateAsync(user, rm.Password);

                if (res.Succeeded)
                {
                    IdentityRole adminRole = new() { Name = "Admin" };
                    IdentityRole userRole = new() { Name = "User" };
                    if (!await _roleManager.RoleExistsAsync("Admin"))
                    {
                        await _roleManager.CreateAsync(adminRole);
                        await _roleManager.CreateAsync(userRole);
                    }

                    await _userManager.AddToRoleAsync(user, "User");

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    string redirUrl = Url.Action("ConfirmEmail", "Account", new { email = rm.Email, token }, Request.Scheme);

                    await _sender.SendEmailAsync(rm.Email, "Library System Account Confirmation", redirUrl);

                    return Ok(new
                    {
                        Message = "Please visit your email for confirmation link"
                    });
                }
            }

            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginModel lm)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByEmailAsync(lm.Email);

                if(user is null)
                {
                    return NotFound();
                }

                var userRoles = await _userManager.GetRolesAsync(user);
                var newClaims = new List<Claim>() { new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti,user.SecurityStamp) };

                List<Claim> allClaims = new();
                allClaims.AddRange(newClaims);

                foreach (var role in userRoles)
                {
                    allClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:ValidIssuer"],
                    audience: _configuration["Jwt:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: allClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                string writtenToken = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new
                {
                    Token = writtenToken
                });

            }

            return BadRequest();
        }

        [HttpGet("confirmemail")]
        public async Task<IActionResult> ConfirmEmail(string email,string token)
        {
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }

            IdentityUser user = await _userManager.FindByEmailAsync(email);

            if(user is null)
            {
                return NotFound();
            }

            var res = await _userManager.ConfirmEmailAsync(user, token);

            if (res.Succeeded)
            {
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);

            if(user is null)
            {
                return NotFound();
            }

            await _userManager.DeleteAsync(user);

            return NoContent();
        }

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel fpm)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByEmailAsync(fpm.Email);

                if(user is null)
                {
                    return NotFound();
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                return Ok(new { token });
            }

            return BadRequest();
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword(string token, ResetPasswordModel rpm)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByEmailAsync(rpm.Email);

                if(user is null)
                {
                    return NotFound();
                }

                var res = await _userManager.ResetPasswordAsync(user, token, rpm.NewPassword);

                if (res.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return Unauthorized();
                }
            }

            return BadRequest();
        }
    }
}
