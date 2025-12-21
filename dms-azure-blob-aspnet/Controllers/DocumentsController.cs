using DMS.Api.Data;
using DMS.Api.Models;
using DMS.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly BlobStorageService _blobService;
        private readonly ApplicationDbContext _db;

        public DocumentsController(BlobStorageService blobService, ApplicationDbContext db)
        {
            _blobService = blobService;
            _db = db;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(
            IFormFile file,
            string customerId,0
            string uploadedBy)
        {
            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            await _blobService.UploadAsync(uniqueFileName, file.OpenReadStream());

            var document = new Document
            {
                Id = Guid.NewGuid(),
                FileName = uniqueFileName,
                OriginalFileName = file.FileName,
                CustomerId = customerId,
                UploadedBy = uploadedBy,
                UploadDate = DateTime.UtcNow,
                ContentType = file.ContentType
            };

            _db.Documents.Add(document);
            await _db.SaveChangesAsync();

            return Ok(document);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Download(Guid id)
        {
            var doc = await _db.Documents.FindAsync(id);
            if (doc == null) return NotFound();

            var stream = await _blobService.DownloadAsync(doc.FileName);
            return File(stream, doc.ContentType, doc.OriginalFileName);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomer(string customerId)
        {
            var docs = await _db.Documents
                .Where(d => d.CustomerId == customerId)
                .ToListAsync();

            return Ok(docs);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDocuments()
        {
            var docs = await _db.Documents
                .Select(d => new
                {
                    d.Id,
                    d.FileName,
                    d.OriginalFileName,
                    d.CustomerId
                })
                .ToListAsync();

            return Ok(docs);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var doc = await _db.Documents.FindAsync(id);
            if (doc == null) return NotFound();

            await _blobService.DeleteAsync(doc.FileName);

            _db.Documents.Remove(doc);
            await _db.SaveChangesAsync();

            return Ok("Deleted");
        }
    }
}