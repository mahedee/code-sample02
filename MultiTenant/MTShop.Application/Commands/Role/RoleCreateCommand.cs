using AutoMapper;
using MediatR;
using MTShop.Application.Common.Exceptions;
using MTShop.Application.DTOs;
using MTShop.Application.Interfaces;
using MTShop.Core.Entities.Identity;

namespace MTShop.Application.Commands.Role
{
    public class RoleCreateCommand : IRequest<RoleDTO>
    {
        public string RoleName { get; set; }
    }

    public class RoleCreateCommandHandler : IRequestHandler<RoleCreateCommand, RoleDTO>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;

        public RoleCreateCommandHandler(IMapper mapper, IIdentityService identityService, ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _identityService = identityService;
            _currentUserService = currentUserService;
        }

        public async Task<RoleDTO> Handle(RoleCreateCommand request, CancellationToken cancellationToken)
        {
            var role = _mapper.Map<RoleCreateCommand, ApplicationRole>(request);
            role.TenantId = _currentUserService.TenantId;
            var (result, roleId) = await _identityService.CreateRoleAsync(role);

            if(!result.Succeeded)
                throw new IdentityResultException(result, "Role Creation failed.");

            var createdRole = await _identityService.GetRoleAsync(roleId);
            return _mapper.Map<ApplicationRole, RoleDTO>(createdRole);
        }
    }
}
