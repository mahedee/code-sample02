using Accounting.API.Db;
using Accounting.API.Models;
using Accounting.API.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace Accounting.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionDetailsController : ControllerBase
    {
        private readonly AccountingContext _context;
        private readonly IDistributedCache _distributedCache;

        public TransactionDetailsController(AccountingContext context, IDistributedCache distributedCache)
        {
            _context = context;
            _distributedCache = distributedCache;

        }

        // GET: api/TransactionDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDetails>>> GetTransactionDetails()
        {
            if (_context.TransactionDetails == null)
            {
                return NotFound();
            }
            return await _context.TransactionDetails.ToListAsync();
        }

        // GET: api/TransactionDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDetails>> GetTransactionDetails(long id)
        {
            if (_context.TransactionDetails == null)
            {
                return NotFound();
            }
            var transactionDetails = await _context.TransactionDetails.FindAsync(id);

            if (transactionDetails == null)
            {
                return NotFound();
            }

            return transactionDetails;
        }

        // PUT: api/TransactionDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransactionDetails(long id, TransactionDetails transactionDetails)
        {
            if (id != transactionDetails.Id)
            {
                return BadRequest();
            }

            _context.Entry(transactionDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionDetailsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TransactionDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TransactionDetails>> PostTransactionDetails(TransactionDetails transactionDetails)
        {
            // Create a hash key using transaction id, dr and cr amount
            string idempotencyKey = HashGenerator.GetHash(transactionDetails.TransactionId.ToString() 
                + transactionDetails.DrAmount.ToString() + transactionDetails.CrAmount.ToString());


            // check hash key is exists in the redis cache
            var isCached = await _distributedCache.GetAsync(idempotencyKey).ConfigureAwait(false);

            if(isCached is not null)
            {
                // if same value is already exists in the cache then return existing value. 
                var decodedResult = JsonConvert.DeserializeObject<TransactionDetails>(Encoding.UTF8.GetString(isCached));
                //var employeeDecodedResult = _mapper.Map<EmployeeResponseDTO>(decodedResult);
                return decodedResult;
            }

            // if input object is null return with a problem
            if (_context.TransactionDetails == null)
            {
                return Problem("Entity set 'AccountingContext.TransactionDetails'  is null.");
            }


            // Save into database
            _context.TransactionDetails.Add(transactionDetails);
            await _context.SaveChangesAsync();

            // Set value into cache after save
            // value will be removed after 10 mins
            // It will be removed after 2 mins, if it is not requested within 2 mins

            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(10)).SetSlidingExpiration(TimeSpan.FromMinutes(2));
            await _distributedCache.SetAsync(idempotencyKey,
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(transactionDetails)), options);


            return CreatedAtAction("GetTransactionDetails", new { id = transactionDetails.Id }, transactionDetails);
        }

        // DELETE: api/TransactionDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionDetails(long id)
        {
            if (_context.TransactionDetails == null)
            {
                return NotFound();
            }
            var transactionDetails = await _context.TransactionDetails.FindAsync(id);
            if (transactionDetails == null)
            {
                return NotFound();
            }

            _context.TransactionDetails.Remove(transactionDetails);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionDetailsExists(long id)
        {
            return (_context.TransactionDetails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
