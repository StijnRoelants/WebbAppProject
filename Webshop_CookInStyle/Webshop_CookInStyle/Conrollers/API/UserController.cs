using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Webshop_CookInStyle.Entities;
using Webshop_CookInStyle.Helpers;
using Webshop_CookInStyle.Models;

namespace Webshop_CookInStyle.Conrollers.API
{
    [Route("api/[controller]")]
    //[ApiController]
    public class UserController : ControllerBase
    {
        private readonly SignInManager<Klant> _signInManager;
        private readonly UserManager<Klant> _userManager;
        private readonly AppSettings _appSettings;

        public UserController(UserManager<Klant> userManager, SignInManager<Klant> signInManager, IOptions<AppSettings> appsettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appsettings.Value;
        }

        [HttpPost("authenticate")]
        public async Task<object> Authenticate([FromBody] ApiUser apiUser)
        {
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(apiUser.Username, apiUser.Password, false, false);
            if (signInResult.Succeeded)
            {
                Klant klant = _userManager.Users.SingleOrDefault(x => x.Email == apiUser.Username);
                apiUser.Token = GenerateJwtToken(apiUser.Username, klant).ToString();

                return apiUser;
            }
            return BadRequest(new { message = "Username or password is incorrect" });
        }

        private object GenerateJwtToken(string username, Klant klant)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, klant.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
