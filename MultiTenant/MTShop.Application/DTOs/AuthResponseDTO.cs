namespace MTShop.Application.DTOs
{
    public class AuthResponseDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
