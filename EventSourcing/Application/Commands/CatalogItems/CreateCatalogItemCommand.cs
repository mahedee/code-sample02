using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Commands.CatalogItems
{
    public class CreateCatalogItemCommand : INotification
    {
        public CreateCatalogItemCommand(string name, string description, double price, int availableStock, int restockThreshold, 
            int maxStockThreshold, bool onReorder)
        {
            Name = name;
            Description = description;
            Price = price;
            AvailableStock = availableStock;
            RestockThreshold = restockThreshold;
            MaxStockThreshold = maxStockThreshold;
            OnReorder = onReorder;
        }


        public string Name { get; private set; }

        public string Description { get; private set; }

        public double Price { get; private set; }

        // Quantity in stock
        public int AvailableStock { get; private set; }

        // Available stock at which we should reorder
        public int RestockThreshold { get; private set; }

        // Maximum number of units that can be in-stock at any time (due to physicial/logistical constraints in warehouses)
        public int MaxStockThreshold { get; private set; }

        /// <summary>
        /// True if item is on reorder
        /// </summary>
        public bool OnReorder { get; private set; }
    }

    public class CreateCatalogItemCommandHandler : INotificationHandler<CreateCatalogItemCommand>
    {
        private readonly IAggregateRepository<CatalogItem, Guid> _aggregateRepository;
        private readonly ICatalogItemRepository _catalogItemRepository;

        public CreateCatalogItemCommandHandler(IAggregateRepository<CatalogItem, Guid> aggregateRepository, 
            ICatalogItemRepository catalogItemRepository)
        {
            _aggregateRepository = aggregateRepository;
            _catalogItemRepository = catalogItemRepository;
        }

        public async Task Handle(CreateCatalogItemCommand notification, CancellationToken cancellationToken)
        {

    

            // Insert event into eventstore db
            var catalogItem = CatalogItem.Create(notification.Name, notification.Description, notification.Price, notification.AvailableStock,
                notification.RestockThreshold, notification.MaxStockThreshold, notification.OnReorder);
            await _aggregateRepository.AppendAsync(catalogItem);

            // Save data into database
            //await _catalogItemAggregateRepository.SaveAsync(catalogItem.Events.FirstOrDefault());
            await _catalogItemRepository.AddAsync(catalogItem);

            // Dispatch events to any event/service bus to do next actions
            // We can also register EventStore db Subscription to receive Event Notification
        }
    }
}
