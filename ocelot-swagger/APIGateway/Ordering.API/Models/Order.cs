namespace Ordering.API.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Address { get; set; }

        public DateTime OrderDate { get; set; }

        public string Comments { get; set; }
    }
}
