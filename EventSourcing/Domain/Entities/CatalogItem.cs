using Domain.Entities.Common;
using Domain.Events.CatalogItem;

namespace Domain.Entities
{
    public class CatalogItem : BaseAggregateRoot<CatalogItem, Guid>
    {
        private CatalogItem()
        {

        }

        public CatalogItem(Guid id, string name, string description, double price, int availableStock, 
            int restockThreshold, int maxStockThreshold, bool onReorder) : base(id)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            AvailableStock = availableStock;
            RestockThreshold = restockThreshold;
            MaxStockThreshold = maxStockThreshold;
            OnReorder = onReorder;

            if (Version > 0)
            {
                throw new Exception("Catalog item already created");
            }

            if (string.IsNullOrEmpty(name))
            {
                //Validation Exception will be placed here
                throw new Exception("Name Can not be Empty");
            }

            if (price <= 0)
            {
                //Validation Exception will be placed here
                throw new Exception("Price must be positive value");
            }

            // Add CatalogItem Event Here to create
            AddEvent(new CatalogItemCreated(this));
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
        public bool IsDeleted { get; private set; } = false;

        public static CatalogItem Create(string name, string description, double price, int availableStock, 
            int restockThreshold, int maxStockThreshold, bool onReorder)
        {
            return new CatalogItem(Guid.NewGuid(), name, description, price, availableStock, restockThreshold, maxStockThreshold, onReorder); ;
        }

        public void Update(Guid id, string name, string description, double price, int availableStock,
         int restockThreshold, int maxStockThreshold, bool onReorder)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            AvailableStock = availableStock;
            RestockThreshold = restockThreshold;
            MaxStockThreshold = maxStockThreshold;
            OnReorder = onReorder;

            AddEvent(new CatalogItemUpdated(this));
        }

        public void Delete(Guid id)
        {
            Id = id;
            IsDeleted = true;
            AddEvent(new CatalogItemDeleted(this));
        }



        protected override void Apply(IDomainEvent<Guid> @event)
        {
            switch (@event)
            {
                case CatalogItemCreated catalogItemCreated: OnCatalogItemCreated(catalogItemCreated); break;
                case CatalogItemUpdated catalogItemUpdated: OnCatalogItemUpdated(catalogItemUpdated); break;
                case CatalogItemDeleted catalogItemDeleted: 
                    IsDeleted = catalogItemDeleted.IsDeleted;
                    break;

            }
        }

        // On Catalog Item Created Event
        private void OnCatalogItemCreated(CatalogItemCreated catalogItemCreated)
        {

            Id = catalogItemCreated.AggregateId; // Must have ID
            Name = catalogItemCreated.Name;
            Description= catalogItemCreated.Description;
            Price = catalogItemCreated.Price;
            AvailableStock = catalogItemCreated.AvailableStock;
            RestockThreshold = catalogItemCreated.RestockThreshold;
            MaxStockThreshold = catalogItemCreated.MaxStockThreshold;
            OnReorder = catalogItemCreated.OnReorder;
        }

        // On Catalog Item Updated Event
        private void OnCatalogItemUpdated(CatalogItemUpdated catalogItemUpdated)
        {
            Name = catalogItemUpdated.Name;
            Description = catalogItemUpdated.Description;
            Price = catalogItemUpdated.Price;
            AvailableStock = catalogItemUpdated.AvailableStock;
            RestockThreshold = catalogItemUpdated.RestockThreshold;
            MaxStockThreshold = catalogItemUpdated.MaxStockThreshold;
            OnReorder = catalogItemUpdated.OnReorder;
        }
    }
}
