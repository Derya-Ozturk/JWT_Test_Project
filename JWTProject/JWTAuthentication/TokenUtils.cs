using Azure.Core;
using JWTProject.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JWTProject.JWTAuthentication
{
    public class TokenUtils
    {
        private readonly IConfiguration _configuration;
        public TokenUtils(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Token GenerateJWTToken(User user)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken securityToken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims:
                        new List<Claim> {
                            new(ClaimTypes.Email, user.Mail),
                            new(ClaimTypes.Role, user.Role) },
                expires: DateTime.Now.AddMinutes(5),
                notBefore: DateTime.Now,
                signingCredentials: signingCredentials
                );

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            return new Token
            {
                AccessToken = tokenHandler.WriteToken(securityToken),
                Expiration = DateTime.Now.AddMinutes(5),
                RefreshToken = CreateRefreshToken()
            };           
        }

        public string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
            {
                random.GetBytes(number);
                return Convert.ToBase64String(number);
            }
        }
    }
}
