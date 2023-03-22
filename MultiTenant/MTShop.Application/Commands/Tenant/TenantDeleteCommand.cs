using MediatR;
using MTShop.Application.Interfaces;
using MTShop.Application.Interfaces.Admin;

namespace MTShop.Application.Commands.Tenant
{
    public record TenantDeleteCommand(Guid id) : IRequest<bool>;
    public class TenantDeleteCommandHandler : IRequestHandler<TenantDeleteCommand, bool>
    {
        private readonly IManageTenantRepository _manageTenantRepository;
        private readonly IAdminUnitOfWork _adminUnitOfWork;

        public TenantDeleteCommandHandler( IManageTenantRepository manageTenantRepository,
            IAdminUnitOfWork adminUnitOfWork)
        {
            this._manageTenantRepository = manageTenantRepository;
            this._adminUnitOfWork = adminUnitOfWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(TenantDeleteCommand request, CancellationToken cancellationToken)
        {
            var tenant = await _manageTenantRepository.GetById(request.id);
            await _manageTenantRepository.DeleteAsync(tenant);
            await _adminUnitOfWork.CommitAsync();
            return true;
        }
    }
}
