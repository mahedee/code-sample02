namespace MTShop.Application.DTOs
{
    public class CustomerDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string FathersName { get; set; }
        public string MothersName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string TenantId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
