using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data
{
    public class DbConnector
    {
        private readonly IConfiguration _configuration;

        protected DbConnector(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            string _connectionString = _configuration.GetConnectionString("DefaultConnection");
            return new SqliteConnection(_connectionString);
            //return new SqliteConnection("Db//Ordering.db");
            //return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
