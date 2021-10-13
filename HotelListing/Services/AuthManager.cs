using HotelListing.Data;
using HotelListing.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private ApiUser _user;

        public AuthManager(
            UserManager<ApiUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> CreateToken()
        {
            SigningCredentials signingCredentials = GetSigningCredentials();
            List<Claim> claims = await GetClaims();
            JwtSecurityToken tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public async Task<bool> ValidateUser(LoginUserDTO userDTO)
        {
            _user = await _userManager.FindByNameAsync(userDTO.Email);
            return _user != null && await _userManager.CheckPasswordAsync(_user, userDTO.Password);
        }

        private SigningCredentials GetSigningCredentials()
        {
            string key = Environment.GetEnvironmentVariable("KEY");
            SymmetricSecurityKey secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.UserName)
            };

            IList<string> roles = await _userManager.GetRolesAsync(_user);

            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(
            SigningCredentials signingCredentials,
            List<Claim> claims)
        {
            IConfiguration jwtSettings = _configuration.GetSection("Jwt");
            DateTime expiration = DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("validIssuer").Value));

            return new JwtSecurityToken(
                issuer: jwtSettings.GetSection("validIssuer").Value,
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials);
        }
    }
}
