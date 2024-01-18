using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ConcurrencyHandling.API.Models
{
    public class Account
    {
        public int AccountID { get; set; }

        public string Name { get; set; }

        public decimal Balance { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = new byte[8];
    }
}
