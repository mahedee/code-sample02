using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerce.Model
{
    public class Auth : IJwtAuth
    {
        private readonly string _userName = "mahedee";
        private readonly string _password = "mahedee123";
        private readonly string key;

        public Auth(string key)
        {
            this.key = key;
        }

        public string Authentication(string username, string password)
        {
            if (!(_userName.Equals(username) || _password.Equals(password)))
            {
                return null;
            }


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, username),
                new Claim(ClaimTypes.Name,username),
                new Claim("UserId",username)
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


            /* 
             
            // 1. Create security token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // 2. Create Private Key to Encrypted
            var tokenKey = Encoding.ASCII.GetBytes(key);


            // 3. Create JWT descriptor 

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, username)
                    }),
                Expires = DateTime.UtcNow.AddHours(1),

                // Private key + algorithm
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            //4. Create token

            var token = tokenHandler.CreateToken(tokenDescriptor);

            //5. Retun Token from method
            return tokenHandler.WriteToken(token);

            */
        }
    }
}
