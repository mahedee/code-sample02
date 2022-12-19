namespace CleanArch.Core.Entities.Base
{
    public class BaseEntity<T> where T : struct
    {
        public T Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string AuthorizeStatus { get; set; } = "U";
        public string? AuthorizedBy { get; set; }
        public DateTime? AuthorizedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
