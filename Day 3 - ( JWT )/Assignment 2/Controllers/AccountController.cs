using Assignment_2.DTOs;
using Assignment_2.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Assignment_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class AccountController : ControllerBase
    {
        private UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            this.userManager = userManager;
            this.config = config;
        }

        // Create New Account ( Registration )
        [HttpPost("register")]
        public async Task <IActionResult> Registraion(RegisterUserDTO userDTO)
        {
            // Worst Case of ModelState
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            // Happy Case of ModelState
            else
            {
                ApplicationUser newUser = new ApplicationUser();
                newUser.UserName = userDTO.UserName;
                newUser.Email = userDTO.Email;
                IdentityResult result = await userManager.CreateAsync(newUser, userDTO.Password);
                // Worst Case 
                if (!result.Succeeded)
                {
                    foreach(var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                    
                    return BadRequest(ModelState);
                }
                // Happy Case
                return Ok("Account Added Succeeded");
            }
        }

        // Check Account Valid "Login"
        [HttpPost("login")]
        public async Task<IActionResult> login(LoginUserDTO userDTO)
        {
            // Worst Case of ModelState
            if (!ModelState.IsValid)
                return Unauthorized();

            // Happy case of ModelState
            else
            // Check & Create token
            {
                // Check if userName and Password are Valid
                // 1- check username
                ApplicationUser user = await userManager.FindByNameAsync(userDTO.UserName);
                if(user == null)
                    return Unauthorized();
                else
                {
                    // 2- check password
                   bool found =  await userManager.CheckPasswordAsync(user, userDTO.Password);
                    if (!found)
                        return Unauthorized();

                    else  // Create Token 
                    {
                        // Token Claims
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name , user.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())); // Token Id

                        // Get Roles and add it to claims
                        var roles = await userManager.GetRolesAsync(user);
                        foreach (var role in roles)
                            claims.Add(new Claim(ClaimTypes.Role, role));

                        // credential key to create signature
                        SecurityKey securetyKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecretKey"]));
                            
                        SigningCredentials signingCredentials = new SigningCredentials  // verfid and trust
                            (
                                securetyKey,
                                SecurityAlgorithms.HmacSha256
                            );

                        // Token Structure Design  (Header - Paylod - signature) as a JSON
                        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
                            (
                                issuer: config["JWT:ValidIssuer"],       // api web url (Provider)
                                audience: config["JWT:ValidAudience"],   // consumer url
                                claims: claims,                          // List of claims
                                expires: DateTime.Now.AddHours(1),
                                signingCredentials : signingCredentials
                            );

                        return Ok
                           (new
                            {                                       // Create token of Design (Serialize JSON) Self Compact
                               token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                               expriration = jwtSecurityToken.ValidTo
                            });

                    }
                }
            }
        }
    }
}
