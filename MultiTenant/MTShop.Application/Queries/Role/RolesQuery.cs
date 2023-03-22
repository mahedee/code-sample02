using AutoMapper;
using MediatR;
using MTShop.Application.DTOs;
using MTShop.Application.Interfaces;

namespace MTShop.Application.Queries.Role
{
    public class RolesQuery : IRequest<IEnumerable<RoleDTO>>
    {
    }

    public class RoleQueryHandler : IRequestHandler<RolesQuery, IEnumerable<RoleDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        public RoleQueryHandler(IMapper mapper, IIdentityService identityService)
        {
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<IEnumerable<RoleDTO>> Handle(RolesQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<RoleDTO>>(await _identityService.GetRolesAsync());
        }
    }
}
