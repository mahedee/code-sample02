namespace DMS.Api.Models
{
    public class Document
    {
        public Guid Id { get; set; }
        public required string FileName { get; set; }          // stored in blob
        public required string OriginalFileName { get; set; }  // user file name
        public required string CustomerId { get; set; }
        public required string UploadedBy { get; set; }
        public DateTime UploadDate { get; set; }
        public required string ContentType { get; set; }
    }
}