using Domain.Entities.Common;

namespace Domain.Events.CatalogItem
{
    /// <summary>
    /// Catalog item created event
    /// </summary>
    public class CatalogItemCreated: BaseDomainEvent<Entities.CatalogItem, Guid>
    {

        private CatalogItemCreated()
        {

        }
        public CatalogItemCreated(Entities.CatalogItem catalogItem) : base(catalogItem)
        {
            //Id = catalogItem.Id;
            Name = catalogItem.Name;
            Description = catalogItem.Description;
            Price = catalogItem.Price;
            AvailableStock = catalogItem.AvailableStock;
            RestockThreshold = catalogItem.RestockThreshold;
            MaxStockThreshold = catalogItem.MaxStockThreshold;
            OnReorder = catalogItem.OnReorder;
            IsDeleted = catalogItem.IsDeleted;
            
        }

        //public Guid Id { get; set; }

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

        public bool IsDeleted { get; private set; }

    }
}
