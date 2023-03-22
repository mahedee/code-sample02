using MediatR;
using MTShop.Application.Common.Exceptions;
using MTShop.Application.Interfaces;

namespace MTShop.Application.Commands.Role
{
    public record RoleDeleteCommand(string roleId) : IRequest<int>;


    public class RoleDeleteCommandHandler : IRequestHandler<RoleDeleteCommand, int>
    {
        private readonly IIdentityService _identityService;

        public RoleDeleteCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<int> Handle(RoleDeleteCommand request, CancellationToken cancellationToken)
        {
            var roleDetails = await _identityService.GetRoleAsync(request.roleId);
           
            if(roleDetails == null)
            {
                throw new NotFoundException("Role information not found");
            }

            var result = await _identityService.DeleteRoleAsync(request.roleId);
            return !result ? throw new Exception("Could not delete role!") : 1;
        }
    }
}
