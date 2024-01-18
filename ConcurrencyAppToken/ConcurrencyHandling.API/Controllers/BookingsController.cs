using ConcurrencyHandling.API.Data;
using ConcurrencyHandling.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConcurrencyHandling.API.Controllers
{
    // Concurrency check using application managed token
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBooking()
        {
            if (_context.Booking == null)
            {
                return NotFound();
            }
            return await _context.Booking.ToListAsync();
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            if (_context.Booking == null)
            {
                return NotFound();
            }
            var booking = await _context.Booking.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        // PUT: api/Bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            if (id != booking.BookingID)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                booking.RecordVersion = Guid.NewGuid();
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
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

        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(Booking booking)
        {
            if (_context.Booking == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Booking'  is null.");
            }
            booking.RecordVersion = Guid.NewGuid();
            _context.Booking.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooking", new { id = booking.BookingID }, booking);
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id, Booking booking)
        {
            if (_context.Booking == null)
            {
                return NotFound();
            }
            //var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            _context.Booking.Remove(booking);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
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

        private bool BookingExists(int id)
        {
            return (_context.Booking?.Any(e => e.BookingID == id)).GetValueOrDefault();
        }
    }
}
