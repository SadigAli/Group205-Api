using Library.Data.DTOs.Auth;
using Library.Data.Entities;
using Library.Repository.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Library.Repository.Implementations
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;
        public AuthManager(UserManager<AppUser> userManager,IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }
        public async Task<(int,string)> Login(LoginDTO login)
        {
            AppUser user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null) return (0, "Incorrect credentials");
            if(!(await _userManager.CheckPasswordAsync(user,login.Password))) return (0, "Incorrect credentials");
            List<Claim> claims = new List<Claim>
            {
                new Claim("Id",Guid.NewGuid().ToString()),
                new Claim("Email",JwtRegisteredClaimNames.Email),
                new Claim("Name",JwtRegisteredClaimNames.Jti),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            string token = GenerateToken(claims);
            return (1, token);
        }

        public Task Logout()
        {
            throw new NotImplementedException();
        }

        public async Task<(int,string)> Register(RegisterDTO register)
        {
            AppUser user = await _userManager.FindByEmailAsync(register.Email);
            if (user != null) return (0,"User is already exists");
            user = await _userManager.FindByNameAsync(register.Username);
            if (user != null) return (0,"User is already");
            user = new AppUser
            {
                FirstName = register.Firstname,
                LastName = register.Lastname,
                UserName = register.Username,
                Email = register.Email,
            };

            var identityResult = await _userManager.CreateAsync(user, register.Password);
            if (!identityResult.Succeeded) return (0,"Somenthing went wrong");

            return (1, "Successfully registered");
        }

        private string GenerateToken(List<Claim> claims)
        {
            var descriptor = new SecurityTokenDescriptor
            {
                Audience = _config["JWT:Audiance"],
                Issuer = _config["JWT:Issuer"],
                Expires = DateTime.Now.AddHours(1),
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(_config["JWT:SecretKey"])),SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
