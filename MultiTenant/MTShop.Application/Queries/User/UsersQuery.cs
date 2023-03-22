using MediatR;
using MTShop.Application.DTOs;
using MTShop.Application.Interfaces;

namespace MTShop.Application.Queries.User
{
    public record UsersQuery : IRequest<IEnumerable<UserResponseDTO>>;

    public class UsersQueryHandler : IRequestHandler<UsersQuery, IEnumerable<UserResponseDTO>>
    {
        private readonly IIdentityService _identityService;

        public UsersQueryHandler(IIdentityService identityService)
        {
            this._identityService = identityService;
        }

        public async Task<IEnumerable<UserResponseDTO>> Handle(UsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _identityService.GetAllUsersAsync();
            return users.Select(x => new UserResponseDTO
            {
                UserName = x.userName,
                Email = x.email,
                TenantId = x.tenantId
            });
        }
    }
}
