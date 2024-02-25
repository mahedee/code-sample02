using System.Data.SqlClient;

namespace Concurrency.API.Persistence
{
    // This class is used to create a connection to the database.
    public class DBConnector : IDisposable
    {
        public SqlConnection connection;

        public DBConnector(string connectionString)
        {
            connection = new SqlConnection(connectionString);
            this.connection.Open();
        }

        public void Dispose()
        {
            connection.Close();
        }
    }
}
