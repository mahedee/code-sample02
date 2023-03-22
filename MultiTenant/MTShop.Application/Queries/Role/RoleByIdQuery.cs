using AutoMapper;
using MediatR;
using MTShop.Application.DTOs;
using MTShop.Application.Interfaces;
using MTShop.Core.Entities.Identity;

namespace MTShop.Application.Queries.Role
{
    public record RoleByIdQuery(string Id) : IRequest<RoleDTO>;
    public class RoleByIdQueryHanlder : IRequestHandler<RoleByIdQuery, RoleDTO>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;

        public RoleByIdQueryHanlder(IMapper mapper, IIdentityService identityService)
        {
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<RoleDTO> Handle(RoleByIdQuery request, CancellationToken cancellationToken)
        {
            var roleInfo = await _identityService.GetRoleAsync(request.Id);
            return _mapper.Map<ApplicationRole, RoleDTO>(roleInfo);
        }
    }
}
