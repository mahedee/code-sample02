using AutoMapper;
using MediatR;
using MTShop.Application.DTOs;
using MTShop.Application.Interfaces.Admin;

namespace MTShop.Application.Queries.Tenant
{
    public record TenantByIdQuery(Guid id) : IRequest<TenantDTO>;

    public class TenantByIdQueryHandler : IRequestHandler<TenantByIdQuery, TenantDTO>
    {
        private readonly IMapper _mapper;
        private readonly IManageTenantRepository _manageTenantRepository;

        public TenantByIdQueryHandler(IMapper mapper, IManageTenantRepository manageTenantRepository)
        {
            this._mapper = mapper;
            this._manageTenantRepository = manageTenantRepository;
        }

        public async Task<TenantDTO> Handle(TenantByIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<TenantDTO>(await _manageTenantRepository.GetById(request.id));
        }
    }
}
