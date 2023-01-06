using Accounting.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Accounting.API.Db
{
    public class AccountingContext : DbContext
    {
        public AccountingContext(DbContextOptions<AccountingContext> options) : base(options)
        {

        }

        public DbSet<TransactionDetails> TransactionDetails { get; set; }
    }


}
