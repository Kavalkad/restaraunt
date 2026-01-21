using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using restaraunt.Application.Interfaces.Auth;
using restaraunt.Core.Entities;

namespace restaraunt.Infrastructure
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _options;
        public JwtProvider(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }
        public string GenerateToken(UserEntity user)
        {
            var claims = new List<Claim>()
            {
                new("userId", user.Id.ToString()),
                new("Admin", "true")
            };
                
            var signingCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey!)),
                        SecurityAlgorithms.HmacSha256
                    );
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_options.ExpiredHours)
            );

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }

    }
}
