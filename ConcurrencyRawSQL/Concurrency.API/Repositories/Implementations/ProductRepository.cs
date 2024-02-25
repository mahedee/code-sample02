using Concurrency.API.Models;
using Concurrency.API.Persistence;
using Concurrency.API.Repositories.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace Concurrency.API.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private DBConnector _connector;
        public ProductRepository(DBConnector dBConnector)
        {
            _connector = dBConnector;
        }
        public Task<Product> AddProduct(Product model)
        {
            try
            {   
                if(this._connector.connection.State == ConnectionState.Closed)
                    this._connector.connection.Open();
                var cmd = this._connector.connection.CreateCommand() as SqlCommand;
                SqlTransaction transaction = this._connector.connection.BeginTransaction("");
                cmd.Transaction = transaction;
                cmd.CommandText = @"INSERT INTO Products (Name, Price, Quantity, RowVersion) 
                OUTPUT inserted.Id                
                VALUES (@Name, @Price, @Quantity, @RowVersion); SELECT SCOPE_IDENTITY();";

                Guid rowVersion = Guid.NewGuid();
                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@Price", model.Price);
                cmd.Parameters.AddWithValue("@Quantity", model.Quantity);
                cmd.Parameters.AddWithValue("@RowVersion", rowVersion);

                // cmd.ExecuteScalar() returns the first column of the first row in the result
                // set returned by the query. Additional columns or rows are ignored.
                int insertedId = Convert.ToInt32(cmd.ExecuteScalar());
                if (insertedId > 0)
                {
                    transaction.Commit();
                }

                model.Id = insertedId;
                model.RowVersion = rowVersion;
                return Task.FromResult(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // close connection
                this._connector.Dispose();
            }
            return null;
        }

        public Task<Product> GetProduct(int id)
        {
            var product = new Product();
            try
            {
                if (this._connector.connection.State == ConnectionState.Closed)
                    this._connector.connection.Open();
                var cmd = this._connector.connection.CreateCommand() as SqlCommand;
                cmd.CommandText = @"SELECT Id, Name, Price, Quantity, RowVersion FROM Products WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@Id", id);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    product.Id = Convert.ToInt32(reader["Id"]);
                    product.Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : String.Empty;
                    product.Price = reader["Price"] != DBNull.Value ? Convert.ToDecimal(reader["Price"]) : 0;
                    product.Quantity = reader["Quantity"] != DBNull.Value ? Convert.ToInt32(reader["Quantity"]) : 0;
                    product.RowVersion = reader["RowVersion"] != DBNull.Value ? (Guid)reader["RowVersion"] : Guid.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // close connection
                this._connector.Dispose();
            }

            // Task.FromResult is a helper method that creates a Task that's completed successfully
            // with the specified result.
            return Task.FromResult(product);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = new List<Product>();

            try
            {
                if (this._connector.connection.State == ConnectionState.Closed)
                    this._connector.connection.Open();
                var cmd = this._connector.connection.CreateCommand() as SqlCommand;
                cmd.CommandText = @"SELECT Id, Name, Price, Quantity, RowVersion FROM Products";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var product = new Product()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : String.Empty,
                        Price = reader["Price"] != DBNull.Value ? Convert.ToDecimal(reader["Price"]) : 0,
                        Quantity = reader["Quantity"] != DBNull.Value ? Convert.ToInt32(reader["Quantity"]) : 0,
                        RowVersion = reader["RowVersion"] != DBNull.Value ? (Guid)reader["RowVersion"] : Guid.Empty
                    };
                    products.Add(product);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // close connection
                this._connector.Dispose();
            }
            return products;
        }

        public Task<Product> UpdateProduct(Product product)
        {

            try
            {
                if(this._connector.connection.State == ConnectionState.Closed)
                    this._connector.connection.Open();
                var cmd = this._connector.connection.CreateCommand() as SqlCommand;
                SqlTransaction transaction = this._connector.connection.BeginTransaction("");
                cmd.Transaction = transaction;
                cmd.CommandText = @"UPDATE Products SET Name = @Name, Price = @Price, Quantity = @Quantity, RowVersion = @RowVersion
                    WHERE Id = @Id AND RowVersion = @RowVersion";

                Guid rowVersion = Guid.NewGuid();
                cmd.Parameters.AddWithValue("@Id", product.Id);
                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@RowVersion", rowVersion);

                int effectedRow = cmd.ExecuteNonQuery();
                if (effectedRow > 0)
                {
                    transaction.Commit();
                }

                product.RowVersion = rowVersion;
                return Task.FromResult(product);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // close connection
                this._connector.Dispose();
            }
        }

        public Task<Product> DeleteProduct(int id, Guid rowVersion)
        {
            try
            {
                if (this._connector.connection.State == ConnectionState.Closed)
                    this._connector.connection.Open();
                var cmd = this._connector.connection.CreateCommand() as SqlCommand;
                SqlTransaction transaction = this._connector.connection.BeginTransaction("");
                cmd.Transaction = transaction;
                cmd.CommandText = @"DELETE FROM Products WHERE Id = @Id AND RowVersion = @RowVersion";
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@RowVersion", rowVersion);

                int effectedRow = cmd.ExecuteNonQuery();
                if (effectedRow > 0)
                {
                    transaction.Commit();
                }
                return Task.FromResult(new Product());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // close connection
                this._connector.Dispose();
            }
        }
    }
}
