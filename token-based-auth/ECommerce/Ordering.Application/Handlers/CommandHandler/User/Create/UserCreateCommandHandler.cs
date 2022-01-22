using MediatR;
using Ordering.Application.Commands.User.Create;
using Ordering.Application.Common.Interfaces;

namespace Ordering.Application.Handlers.CommandHandler.User.Create
{
    public class UserCreateCommandHandler : IRequestHandler<UserCreateCommand, int>
    {
        private readonly IIdentityService _identityService;
        public UserCreateCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<int> Handle(UserCreateCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.CreateUserAsync(request.UserName, request.Password, request.Email, request.Roles);
            return result.isSucceed ? 1 : 0;
        }
    }
}
