using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MTShop.Application.Common.Constants;
using MTShop.Application.Common.Exceptions;
using MTShop.Application.DTOs;
using MTShop.Application.Interfaces.Admin;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MTShop.Application.Commands.Auth
{
    public class AdminAuthCommand : IRequest<AuthResponseDTO>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class AdminAuthCommandHandler : IRequestHandler<AdminAuthCommand, AuthResponseDTO>
    {
        private readonly IAdminIdentityService _identityService;
        private readonly IConfiguration _configuration;

        public AdminAuthCommandHandler(IAdminIdentityService identityService, IConfiguration configuration)
        {
            this._identityService = identityService;
            this._configuration = configuration;
        }
        public async Task<AuthResponseDTO> Handle(AdminAuthCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.SignInUserAsync(request.UserName, request.Password);
            if (!result)
                throw new BadRequestException("Invalid UserName and Password! ");

            var userIdByName = await _identityService.GetUserIdAsync(request.UserName);
            var (userId, userName, roles) = await _identityService.GetUserDetailsAsync(userIdByName);
            var (token, expirationTime) = GenerateJwtToken((userId, userName, roles));

            return new AuthResponseDTO 
            { 
                UserId = userId,
                UserName = userName,
                Token = token,
                ExpirationTime = expirationTime
            };

        }

        private (string, DateTime) GenerateJwtToken((string userId, string userName, IList<string> roles) userDetails)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var (userId, userName, roles) = userDetails;
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, userId),
                new Claim(ClaimTypes.Name, userName),
                new Claim("UserId",userId),
                new Claim("TenantId", UserRolesConstants.SuperAdmin),
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:ValidIssuer"],
                audience: _configuration["jwt:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["Jwt:ExpiryMinutes"])),
                signingCredentials: credentials
                );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return (encodedToken, token.ValidTo);
        }
    }
}
