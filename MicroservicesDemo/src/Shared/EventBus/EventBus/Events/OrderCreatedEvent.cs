namespace EventBus.Events
{
    public class OrderCreatedEvent : BaseEvent
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string CustomerEmail { get; set; }
    }
}