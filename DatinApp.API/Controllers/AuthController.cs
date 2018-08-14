using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DatinApp.API.Data;
using DatinApp.API.DTO;
using DatinApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatinApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO user)
        {
            try
            {
                user.Username = user.Username.ToLower();
                if(await _repo.UserExists(user.Username))
                    return BadRequest("Username is already taken");

                var userToCreate = new User()
                {
                    Username = user.Username
                };

                var createdUser = await _repo.Register(userToCreate, user.Password);

                return StatusCode(201);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO user)
        {
            try
            {
                var userFromRepo = await _repo.Login(user.Username.ToLower(), user.Password);

                if(userFromRepo == null){

                    return Unauthorized();
                }
                var claims = new[] {
                    new Claim(ClaimTypes.Name, userFromRepo.Username),
                    new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString())
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

                var token = new JwtSecurityToken(

                    issuer:"https://localhost:5000/",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(2),
                    signingCredentials: credentials
                );

                return Ok(new {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });

                
            }
            catch (Exception ex)
            {
                
                throw ex;
            }

        }
    }
}