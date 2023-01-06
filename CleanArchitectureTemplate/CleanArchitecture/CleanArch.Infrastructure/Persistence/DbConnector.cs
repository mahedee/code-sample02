using System.Data;
using System.Data.SqlClient;
using CleanArch.Infrastructure.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CleanArch.Infrastructure.Persistence
{
    public class DbConnector
    {
        private readonly IConfiguration _configuration;
        private readonly ConfigurationSettings _settings;
        public DbConnector(IConfiguration configuration, IOptions<ConfigurationSettings> settings)
        {
            _configuration = configuration;
            _settings = settings.Value;
        }

        public IDbConnection CreateConnection()
        {
            var connectionString = _settings.ConnectionStrings.ConfigurationDbConnection;
            return new SqlConnection(connectionString);
        }
    }
}
