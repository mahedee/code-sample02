using AutoMapper;
using MediatR;
using MTShop.Application.Common.Exceptions;
using MTShop.Application.DTOs;
using MTShop.Application.Interfaces;
using MTShop.Application.Interfaces.Admin;

namespace MTShop.Application.Commands.Tenant
{
    public class TenantUpdateCommand : IRequest<TenantDTO>
    {
        public Guid Id { get; set; }
        public string TenantName { get; set; }
        //public string TenantKey { get; set; }
        //public string DatabaseServer { get; set; }
        public string? DbUserName { get; set; }
        public string? DbPassword { get; set; }
        //public string DBProvider { get; set; }
    }

    public class TenantUpdateCommandHandler : IRequestHandler<TenantUpdateCommand, TenantDTO>
    {
        private readonly IManageTenantRepository _manageTenantRepository;
        private readonly IMapper _mapper;
        private readonly IAdminUnitOfWork _adminUnitOfWork;
        public TenantUpdateCommandHandler(IManageTenantRepository manageTenantRepository, IMapper mapper, 
            IAdminUnitOfWork adminUnitOfWork)
        {
            _manageTenantRepository = manageTenantRepository;
            _mapper = mapper;
            _adminUnitOfWork = adminUnitOfWork;
        }
        public async Task<TenantDTO> Handle(TenantUpdateCommand request, CancellationToken cancellationToken)
        {
            var existingTenant = await _manageTenantRepository.GetById(request.Id);
            if (existingTenant == null)
                throw new NotFoundException("Invalid tenant Id!");

            existingTenant.TenantName = request.TenantName;
            existingTenant.UserName = request.DbUserName;
            existingTenant.Password = request.DbPassword;

            var updatedTenant = await _manageTenantRepository.UpdateAsync(existingTenant);
            await _adminUnitOfWork.CommitAsync();

            return _mapper.Map<TenantDTO>(updatedTenant);
        }
    }
}
