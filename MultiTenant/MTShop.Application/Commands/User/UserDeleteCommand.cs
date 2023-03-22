using MediatR;
using MTShop.Application.Interfaces;

namespace MTShop.Application.Commands.User
{
    public record UserDeleteCommand(string id) : IRequest<int>;

    public class UserDeleteCommandHandler : IRequestHandler<UserDeleteCommand, int>
    {
        private readonly IIdentityService _identityService;

        public UserDeleteCommandHandler(IIdentityService identityService)
        {
            this._identityService = identityService;
        }

        public async Task<int> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
        {
            var reuslt = await _identityService.DeleteUserAsync(request.id);
            return reuslt ? 1 : 0;
        }
    }
}
