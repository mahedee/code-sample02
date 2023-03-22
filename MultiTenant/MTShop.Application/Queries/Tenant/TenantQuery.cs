using AutoMapper;
using MediatR;
using MTShop.Application.DTOs;
using MTShop.Application.Interfaces.Admin;

namespace MTShop.Application.Queries.Tenant
{
    public record TenantQuery : IRequest<IEnumerable<TenantDTO>>;

    public class TenantQueryHandler : IRequestHandler<TenantQuery, IEnumerable<TenantDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IManageTenantRepository _tenantRepository;

        public TenantQueryHandler(IMapper mapper, IManageTenantRepository manageTenantRepository)
        {
             _mapper = mapper;
            _tenantRepository = manageTenantRepository;
        }
        public async Task<IEnumerable<TenantDTO>> Handle(TenantQuery request, CancellationToken cancellationToken)
        {
            // Mapping profiles is in MTShop->Common->Mappings
            return _mapper.Map<IEnumerable<TenantDTO>>(await _tenantRepository.GetAllAsync());
        }
    }
}
