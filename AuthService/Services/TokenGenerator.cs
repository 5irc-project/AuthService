﻿using AuthService.DTO;
using AuthService.Services.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Services
{
    public class TokenGenerator : ITokenGenerator
    {
        private IConfiguration configuration;

        private readonly int NB_MINUTE_EXPIRATION;

        public TokenGenerator(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.NB_MINUTE_EXPIRATION = GetExpirationTimeInMinutes();
        }

        public string GenerateToken(UserLoggedDto user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("UserId", user.UserId.ToString()),
                new Claim("displayName", user.Nom),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(NB_MINUTE_EXPIRATION),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private int GetExpirationTimeInMinutes()
        {
            string ExpirationString = configuration["Jwt:ExpirationTimeInMinutes"];
            int ExpirationMinute;
            if (! int.TryParse(ExpirationString, out ExpirationMinute)) ExpirationMinute = 30;

            return ExpirationMinute;
        }
    }
}
