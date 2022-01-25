using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Commands.Auth
{
    public class AuthCommand : IRequest<AuthResponseDTO>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class AuthCommandHandler : IRequestHandler<AuthCommand, AuthResponseDTO>
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IIdentityService _identityService;

        public AuthCommandHandler(IIdentityService identityService, ITokenGenerator tokenGenerator)
        {
            _identityService = identityService;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<AuthResponseDTO> Handle(AuthCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.SigninUserAsync(request.UserName, request.Password);

            if (!result)
            {
                //throw new BadRequestException("Invalid username or password");
                throw new Exception("Invalid user name or password");
            }

            var (userId, userName, email, roles) = await _identityService.GetUserDetailsAsync(await _identityService.GetUserIdAsync(request.UserName));

            //string token = _tokenGenerator.GenerateToken(userId, userName, roles);

            //string token = _tokenGenerator.GenerateToken(request.UserName, request.Password);

            string token = _tokenGenerator.GenerateJWTToken((userId: userId, userName: userName, roles: roles));

            return new AuthResponseDTO()
            {
                UserId = userId,
                Name = userName,
                Token = token
            };
        }
    }
}
