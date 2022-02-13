using Microsoft.EntityFrameworkCore;


namespace Location.API.Persistence
{
    public class LocationDbContext : DbContext
    {
        public LocationDbContext(DbContextOptions<LocationDbContext> options) : base(options)
        {

        }
    }
}
