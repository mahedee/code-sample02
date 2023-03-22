using AutoMapper;
using MediatR;
using MTShop.Application.Common.Exceptions;
using MTShop.Application.DTOs;
using MTShop.Application.Interfaces;
using MTShop.Core.Entities.Identity;

namespace MTShop.Application.Commands.Role
{
    public class RoleUpdateCommand : IRequest<RoleDTO>
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
        public string? RoleDetails { get; set; }
    }

    public class RoleUpdateCommandHandler : IRequestHandler<RoleUpdateCommand, RoleDTO>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;

        public RoleUpdateCommandHandler(IMapper mapper, IIdentityService identityService)
        {
            _mapper = mapper;
            _identityService = identityService;
        }
        public async Task<RoleDTO> Handle(RoleUpdateCommand request, CancellationToken cancellationToken)
        {
            var existingRoleDetails = await _identityService.GetRoleAsync(request.Id);

            if (existingRoleDetails == null)
            {
                throw new NotFoundException("Role Information not found");
            }

            var role = _mapper.Map<RoleUpdateCommand, ApplicationRole>(request, existingRoleDetails);
            var (identityResult, roleId) = await _identityService.UpdateRoleAsync(role);

            if (!identityResult.Succeeded)
            {
                throw new IdentityResultException(identityResult, "Update Role");
            }


            var roleDetails = await _identityService.GetRoleAsync(roleId);
            return _mapper.Map<ApplicationRole, RoleDTO>(roleDetails);
        }
    }
}
