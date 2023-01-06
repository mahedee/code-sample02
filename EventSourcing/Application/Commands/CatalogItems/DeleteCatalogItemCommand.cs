using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Commands.CatalogItems
{
    public class DeleteCatalogItemCommand : INotification
    {
        public DeleteCatalogItemCommand(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; private set; }
    }

    public class DeleteCatlogItemCommandHandler : INotificationHandler<DeleteCatalogItemCommand>
    {
        private readonly IAggregateRepository<CatalogItem, Guid> _aggregateRepository;
        private readonly ICatalogItemRepository _catalogItemRepository;

        public DeleteCatlogItemCommandHandler(IAggregateRepository<CatalogItem, Guid> agrregateRepository
            , ICatalogItemRepository catalogItemRepository)
        {
            _aggregateRepository = agrregateRepository;
            _catalogItemRepository = catalogItemRepository;
        }
        public async Task Handle(DeleteCatalogItemCommand notification, CancellationToken cancellationToken)
        {
            var catalogItem = await _aggregateRepository.RehydreateAsync(notification.Id, cancellationToken);
            if (catalogItem == null)
            {
                throw new Exception("Invalid Catalog Item information");
            }

            catalogItem.Delete(catalogItem.Id);
            await _aggregateRepository.AppendAsync(catalogItem, cancellationToken);

            // Save data into database
            await _catalogItemRepository.UpdateAsync(catalogItem);
        }
    }
}
