using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CollegeApp.Models;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public ActionResult Login(LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please provide valid details.");
            }
            LoginResponseDTO response = new() { Username = model.Username };

            if (model.Username == "Rashi" && model.Password == "Rashi@12")
            {
                byte[] key = null;
                if (model.PolicyName == "Local")
                {
                    key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretLocal"));
                }
                else if (model.PolicyName == "Microsoft")
                {
                    key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretMicrosoft"));
                }
                else if (model.PolicyName == "Google")
                {
                    key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretGoogle"));
                }
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.Name,model.Username),
                        new Claim(ClaimTypes.Role,"Admin")
                    }),
                    Expires = DateTime.Now.AddHours(4),
                    SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                response.Token = tokenHandler.WriteToken(token);
            }
            else
            {
                return Ok("Please provide valid details.");
            }
            return Ok(response);
        }
    }
}