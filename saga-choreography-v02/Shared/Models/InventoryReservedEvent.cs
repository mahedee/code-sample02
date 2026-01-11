namespace Shared.Models
{
    public class InventoryReservedEvent
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public decimal TotalAmount { get; set; }
        public string CustomerEmail { get; set; }
        public int ReservedQuantity { get; set; }
    }
}