using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Ordering.API.Repositories
{
    public class DbConnector
    {
        private readonly IConfiguration _configuration;

        protected DbConnector(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected IDbConnection CreateConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
