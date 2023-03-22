using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using MTShop.Application.DTOs;
using MTShop.Application.Interfaces;
using MTShop.Application.Interfaces.Admin;
using MTShop.Core.Entities.Admin;

namespace MTShop.Application.Commands.Tenant
{
    public class TenantCreateCommand: IRequest<TenantDTO>
    {
        public string TenantName { get; set; }
        public string DBProvider { get; set; }
    }


    // Create a new tenant
    public class TenantCreateCommandHandler : IRequestHandler<TenantCreateCommand, TenantDTO>
    {
        private readonly IMapper _mapper;
        private readonly IManageTenantRepository _manageTenantRepository;
        private readonly IConfiguration _configuration;
        private readonly IAdminUnitOfWork _adminUnitOfWork;
        private readonly IUnitOfWork _unitOfWork;

        // Constructor
        public TenantCreateCommandHandler(IManageTenantRepository manageTenantRepository, IMapper mapper, 
            IConfiguration configuration, IAdminUnitOfWork adminUnitOfWork, IUnitOfWork unitOfWork)
        {
            _manageTenantRepository = manageTenantRepository;
            _mapper = mapper;
            _configuration = configuration;
            _adminUnitOfWork = adminUnitOfWork;
            _unitOfWork = unitOfWork;
        }
        public async Task<TenantDTO> Handle(TenantCreateCommand request, CancellationToken cancellationToken)
        {
            if (await _manageTenantRepository.IsTenantExist(request.TenantName))
                throw new Exception("This tenant is already created, please try with different tenant name");
            var tenant = _mapper.Map<TenantEntity>(request);

            // Set new tenant key
            tenant.TenantKey = Guid.NewGuid().ToString();
            tenant.DatabaseServer = _configuration["AdminSettings:DatabaseConfig:Server"];
            tenant.UserName = _configuration["AdminSettings:DatabaseConfig:UserName"];
            tenant.Password = _configuration["AdminSettings:DatabaseConfig:Password"];
            tenant.ConnectionString = PrepareConnectionString(tenant.DatabaseServer, tenant.TenantName, tenant.UserName, tenant.Password);
            
            // Create tenant
            var result = await _manageTenantRepository.CreateAsync(tenant);

            // Save changes
            await _adminUnitOfWork.CommitAsync();

            // Execute database migration after a tenant is created.
            await _unitOfWork.MigrateTenantDatabase(result);

            return _mapper.Map<TenantDTO>(result);
        }

        private string PrepareConnectionString(string databaseServer, string tenantName, string userName, string password)
        {
            return $"Server={databaseServer};Initial Catalog={tenantName}TenantDb;User ID={userName};Password={password};";
        }
    }
}
