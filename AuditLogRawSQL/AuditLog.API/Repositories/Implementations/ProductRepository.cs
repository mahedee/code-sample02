using AuditLog.API.AuditTrail.Interfaces;
using AuditLog.API.Models;
using AuditLog.API.Persistence;
using AuditLog.API.Repositories.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace AuditLog.API.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private DBConnector _connector;
        private IAuditTrail<Product> _auditTrail;
        public ProductRepository(DBConnector dBConnector, IAuditTrail<Product> auditTrail)
        {
            _connector = dBConnector;
            _auditTrail = auditTrail;
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
                cmd.CommandText = @"INSERT INTO Products (Name, Price, Quantity) 
                OUTPUT inserted.Id                
                VALUES (@Name, @Price, @Quantity); SELECT SCOPE_IDENTITY();";
                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@Price", model.Price);
                cmd.Parameters.AddWithValue("@Quantity", model.Quantity);

                // cmd.ExecuteScalar() returns the first column of the first row in the result
                // set returned by the query. Additional columns or rows are ignored.
                int insertedId = Convert.ToInt32(cmd.ExecuteScalar());
                if (insertedId > 0)
                {
                    transaction.Commit();
                }

                model.Id = insertedId;

                // Add audit trail
                _auditTrail.Insert(model);
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
                cmd.CommandText = @"SELECT Id, Name, Price, Quantity FROM Products WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@Id", id);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    product.Id = Convert.ToInt32(reader["Id"]);
                    product.Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : null;
                    product.Price = Convert.ToDecimal(reader["Price"]);
                    product.Quantity = Convert.ToInt32(reader["Quantity"]);
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
                cmd.CommandText = @"SELECT Id, Name, Price, Quantity FROM Products";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var product = new Product()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : null,
                        Price = Convert.ToDecimal(reader["Price"]),
                        Quantity = Convert.ToInt32(reader["Quantity"])
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
            Product oldEntity = GetProduct(product.Id).Result;

            try
            {
                if(this._connector.connection.State == ConnectionState.Closed)
                    this._connector.connection.Open();
                var cmd = this._connector.connection.CreateCommand() as SqlCommand;
                SqlTransaction transaction = this._connector.connection.BeginTransaction("");
                cmd.Transaction = transaction;
                cmd.CommandText = @"UPDATE Products SET Name = @Name, Price = @Price, Quantity = @Quantity WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@Id", product.Id);
                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);

                int effectedRow = cmd.ExecuteNonQuery();
                if (effectedRow > 0)
                {
                    transaction.Commit();
                }

                // Update audit trail
                _auditTrail.Update(oldEntity, product);
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

        public Task<Product> DeleteProduct(int id)
        {
            Product oldEntity = GetProduct(id).Result;

            try
            {
                if (this._connector.connection.State == ConnectionState.Closed)
                    this._connector.connection.Open();
                var cmd = this._connector.connection.CreateCommand() as SqlCommand;
                SqlTransaction transaction = this._connector.connection.BeginTransaction("");
                cmd.Transaction = transaction;
                cmd.CommandText = @"DELETE FROM Products WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@Id", id);

                int effectedRow = cmd.ExecuteNonQuery();
                if (effectedRow > 0)
                {
                    transaction.Commit();
                }

                // Add audit trail
                _auditTrail.Delete(oldEntity);
                return Task.FromResult(oldEntity);
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
