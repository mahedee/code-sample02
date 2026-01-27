namespace EventBus.Events
{
    public class ProductUpdatedEvent : BaseEvent
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public decimal PreviousPrice { get; set; }
        public int PreviousStock { get; set; }
    }
}