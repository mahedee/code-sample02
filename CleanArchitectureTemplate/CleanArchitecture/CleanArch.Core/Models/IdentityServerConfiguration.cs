namespace CleanArch.Core.Models
{
    public class IdentityServerConfiguration
    {
        public string Authority { get; set; }
        public bool RequireHttpsMetaData { get; set; }
        public string SwaggerUIClientId { get; set; }
        public string ApiName { get; set; }
        public string ApiDisplayName { get; set; }
        public string ApiBaseUrl { get; set; }
        public bool CorsAllowAnyOrigin { get; set; }
    }
}
