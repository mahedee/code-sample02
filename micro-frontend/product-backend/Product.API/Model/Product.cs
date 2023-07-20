namespace Product.API.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ShortName { get; set;}
        public double Price { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
