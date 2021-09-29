namespace Shared.Models
{
    public class OrderRequest
    {
        public int OrderId { get; set; }
        public int CatalogId { get; set; }
        public int Units { get; set; }
        public string Name { get; set; }
    }
}
