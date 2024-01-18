namespace AuditLog.API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public decimal Price { get; set; } = 0;
        public int Quantity { get; set; } = 0;
    }
}
