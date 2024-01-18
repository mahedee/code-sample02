using System.ComponentModel.DataAnnotations;

namespace ConcurrencyHandling.API.Models
{
    public class Booking
    {
        public int BookingID { get; set; }
        public string CustomerName { get; set; }
        public int RoomID { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        // ConcurrencyCheck is specifies that this property participates in optimistic concurrency checking.
        [ConcurrencyCheck]
        public Guid RecordVersion { get; set; }
    }
}
