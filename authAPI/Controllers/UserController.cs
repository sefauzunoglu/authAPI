using authAPI.DTO;
using authAPI.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace authAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        private readonly IConfiguration _configuration;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDTO model)
        {
            var user = new User
            {             
                Name = model.Name,
                UserName = model.UserName,
                Email = model.Email,
                Created = DateTime.Now,
                LastActive = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return StatusCode(201);
            }

            return BadRequest(result.Errors);
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                return BadRequest(new { message = "username is incorrect" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false); //yanlış girilirse hesap kitlensin mi? nope.

            //var date = DateTime.ParseExact(user.Created, "")

            if (result.Succeeded)
            {
                return Ok(new
                {
                    token = GenerateJwtToken(user),
                    username = user.UserName,
                    email = user.Email,
                    recordDate = user.Created
                });
            }
            return Unauthorized();
        }

        private string GenerateJwtToken(User user)
        {
            // Tokenımızı burada oluşturalım.

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Secret").Value);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // Tokenı şifrelediğimiz kısım.
            };

            var token = tokenHandler.CreateToken(tokenDescriptor); // token oluşuyor. token handler aracılığıyla oluşturuyoruz.
            return tokenHandler.WriteToken(token); // oluşturulan bir byte var biz bunu stringe çeviricez
        }
    }
}
