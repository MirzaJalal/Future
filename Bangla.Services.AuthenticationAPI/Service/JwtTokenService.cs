﻿using Bangla.Services.AuthenticationAPI.Models;
using Bangla.Services.AuthenticationAPI.Service.IService;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bangla.Services.AuthenticationAPI.Service
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtOptions _jwtOptions;

        public JwtTokenService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles)
        {
            // 1. Define the handler, security key and credentials
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);


            // 2. Create the claims for the token
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email),
                new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id),  // Token ID
                new Claim(JwtRegisteredClaimNames.Name, applicationUser.UserName)
            };
            
            // adding roles to a user
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // 3. Define the token properties (expiry, issuer, audience)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(120),
                SigningCredentials = credentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            // 4. Generate the token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
