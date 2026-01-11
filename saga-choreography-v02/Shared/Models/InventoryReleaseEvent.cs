namespace Shared.Models
{
    public class InventoryReleaseEvent
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; }
    }
}