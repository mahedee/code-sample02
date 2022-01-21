using MediatR;
using Ordering.Application.Commands;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers.CommandHandler
{
    public class AuthCommandHandler : IRequestHandler<AuthCommand, AuthResponseDTO>
    {
        private readonly ITokenGenerator _tokenGenerator;

        public AuthCommandHandler(ITokenGenerator tokenGenerator)
        {
            _tokenGenerator = tokenGenerator;
        }

        public async Task<AuthResponseDTO> Handle(AuthCommand request, CancellationToken cancellationToken)
        {
            string token = _tokenGenerator.GenerateToken(request.UserName, request.Password);

            return new AuthResponseDTO()
            {
                UserId = request.UserName,
                Name = request.UserName,
                Token = token
            };
        }
    }
}
