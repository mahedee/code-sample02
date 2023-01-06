using Domain.Entities.Common;

namespace Domain.Events.CatalogItem
{
    /// <summary>
    /// Catalog item created event
    /// </summary>
    public class CatalogItemDeleted: BaseDomainEvent<Entities.CatalogItem, Guid>
    {

        private CatalogItemDeleted()
        {

        }
        public CatalogItemDeleted(Entities.CatalogItem catalogItem) : base(catalogItem)
        {
            IsDeleted = catalogItem.IsDeleted;
        }
        public bool IsDeleted { get; private set; }

    }
}
