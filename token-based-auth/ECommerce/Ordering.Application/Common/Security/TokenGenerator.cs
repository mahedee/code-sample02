using Microsoft.IdentityModel.Tokens;
using Ordering.Application.Common.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ordering.Application.Common.Security
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly string _userName = "mahedee";
        private readonly string _password = "mahedee123";
        private readonly string key;

        public TokenGenerator(string key)
        {
            this.key = key;
        }

        public string GenerateToken(string userName, string password)
        {
            if (!(_userName.Equals(userName) || _password.Equals(password)))
            {
                return null;
            }


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, userName),
                new Claim(ClaimTypes.Name,userName),
                new Claim("UserId",userName)
            };


            //claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: "jwt",
                audience: "jwt",
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: signingCredentials
            );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedToken;
        }
    }
}
