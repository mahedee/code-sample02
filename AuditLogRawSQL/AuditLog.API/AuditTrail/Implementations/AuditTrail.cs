using AuditLog.API.AuditTrail.Enums;
using AuditLog.API.AuditTrail.Interfaces;
using AuditLog.API.Persistence;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace AuditLog.API.AuditTrail.Implementations
{
    // This class is used to insert audit log into database.
    // This is the core class of the AuditTrail project.
    public class AuditTrail<T> : IAuditTrail<T> where T : class
    {
        private DBConnector _connector;
        public AuditTrail(DBConnector dBConnector)
        {
            _connector = dBConnector;
        }

        public void Insert(T entity)
        {
            if (entity == null)
            {
                return;
            }


            Type type = typeof(T);
            PropertyInfo[] propertyInfos = type.GetProperties();
            string applicationName = ApplicationEnum.AuditLogAPP.GetApplication();

            var cmd = _connector.connection.CreateCommand() as SqlCommand;
            SqlTransaction transaction = _connector.connection.BeginTransaction("");
            cmd.Transaction = transaction;


            try
            {

                // Insert into AuditEntry table
                cmd.CommandText = @"INSERT INTO [AuditEntry] (EntitySetName, EntityTypeName, State, StateName, CreatedBy, CreatedDate, Discriminator, AppplicationName) 
                                    OUTPUT inserted.AuditEntryID
                                    VALUES (@EntitySetName, @EntityTypeName, @State,@StateName, @CreatedBy, @CreatedDate, @Discriminator, @AppplicationName);
                                    SELECT SCOPE_IDENTITY();";

                cmd.Parameters.AddWithValue("@EntitySetName", type.Name);
                cmd.Parameters.AddWithValue("@EntityTypeName", type.Name);
                cmd.Parameters.AddWithValue("@State", StateName.EntityAdded); // 0 for insert
                cmd.Parameters.AddWithValue("@StateName", StateName.EntityAdded.ToString()); // EntityAdded for insert
                cmd.Parameters.AddWithValue("@CreatedBy", "mahedee"); // It will come from session
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@Discriminator", "CustomAuditEntry"); // CustomAuditEntry Model
                cmd.Parameters.AddWithValue("@AppplicationName", applicationName); // Name of the application

                int auditEntryId = Convert.ToInt32(cmd.ExecuteScalar());

                cmd.Parameters.Clear();


                // Insert into AuditEntryProperty table
                // Values of each property

                cmd.CommandText = @"INSERT INTO [AuditEntryProperty] (AuditEntryID, RelationName, PropertyName, OldValue, NewValue, Discriminator, AppplicationName) 
                                    VALUES (@AuditEntryID, @RelationName, @PropertyName, @OldValue, @NewValue, @DiscriminatorProperty, @AppplicationNameProperty);
                                    SELECT SCOPE_IDENTITY();";

                cmd.Parameters.Add(new SqlParameter("@AuditEntryID", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@RelationName", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("@PropertyName", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("@OldValue", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("@NewValue", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("@DiscriminatorProperty", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("@AppplicationNameProperty", SqlDbType.NVarChar));


                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    cmd.Parameters["@AuditEntryID"].Value = auditEntryId;
                    cmd.Parameters["@RelationName"].Value = DBNull.Value;
                    cmd.Parameters["@PropertyName"].Value = propertyInfo.Name; // property name
                    cmd.Parameters["@OldValue"].Value = DBNull.Value; // Null for insert
                    cmd.Parameters["@NewValue"].Value = propertyInfo.GetValue(entity); // property value
                    cmd.Parameters["@DiscriminatorProperty"].Value = "CustomAuditEntryProperty";
                    cmd.Parameters["@AppplicationNameProperty"].Value = applicationName;
                    cmd.ExecuteScalar();
                }

                // Commit transaction
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                // close connection
                this._connector.Dispose();
            }
        }

        public void Update(T oldEntity, T newEntity)
        {
            if (oldEntity is null || newEntity is null)
            {
                return;
            }

            // Get the properties from type
            Type type = typeof(T);
            PropertyInfo[] propertyInfos = type.GetProperties();
            string applicationName = ApplicationEnum.AuditLogAPP.GetApplication();

            var cmd = _connector.connection.CreateCommand() as SqlCommand;
            SqlTransaction transaction = _connector.connection.BeginTransaction("");
            cmd.Transaction = transaction;


            try
            {

                // Insert into AuditEntry
                cmd.CommandText = @"INSERT INTO [AuditEntry] (EntitySetName, EntityTypeName, State, StateName, CreatedBy, CreatedDate, Discriminator, AppplicationName) 
                                    OUTPUT inserted.AuditEntryID
                                    VALUES (@EntitySetName, @EntityTypeName, @State,@StateName, @CreatedBy, @CreatedDate, @Discriminator, @AppplicationName);
                                    SELECT SCOPE_IDENTITY();";

                cmd.Parameters.AddWithValue("@EntitySetName", type.Name);
                cmd.Parameters.AddWithValue("@EntityTypeName", type.Name);
                cmd.Parameters.AddWithValue("@State", StateName.EntityModified); // 1 for update
                cmd.Parameters.AddWithValue("@StateName", StateName.EntityModified.ToString()); // EntityAdded for update // Come from an enum
                cmd.Parameters.AddWithValue("@CreatedBy", "mahedee"); // It will come from session
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@Discriminator", "CustomAuditEntry"); // CustomAuditEntry Model
                cmd.Parameters.AddWithValue("@AppplicationName", applicationName); // Name of the application // Get from config enum

                int auditEntryId = Convert.ToInt32(cmd.ExecuteScalar());

                cmd.Parameters.Clear();


                // Insert into AuditEntryProperty
                // Values of each property

                cmd.CommandText = @"INSERT INTO [AuditEntryProperty] (AuditEntryID, RelationName, PropertyName, OldValue, NewValue, Discriminator, AppplicationName) 
                                    VALUES (@AuditEntryID, @RelationName, @PropertyName, @OldValue, @NewValue, @DiscriminatorProperty, @AppplicationNameProperty);
                                    SELECT SCOPE_IDENTITY();";

                cmd.Parameters.Add(new SqlParameter("@AuditEntryID", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@RelationName", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("@PropertyName", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("@OldValue", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("@NewValue", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("@DiscriminatorProperty", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("@AppplicationNameProperty", SqlDbType.NVarChar));


                foreach (PropertyInfo propertyInfo in propertyInfos)
                {


                    cmd.Parameters[0].Value = auditEntryId;
                    cmd.Parameters[1].Value = DBNull.Value;
                    cmd.Parameters[2].Value = propertyInfo.Name; // property name
                    cmd.Parameters[3].Value = propertyInfo.GetValue(oldEntity); // old value
                    cmd.Parameters[4].Value = propertyInfo.GetValue(newEntity); // new value
                    cmd.Parameters[5].Value = "CustomAuditEntryProperty";
                    cmd.Parameters[6].Value = applicationName;
                    cmd.ExecuteScalar();
                }

                // Commit transaction
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }

        public void Delete(T oldEntity)
        {
            if (oldEntity == null)
            {
                return;
            }


            // Get the properties from type
            Type type = typeof(T);
            PropertyInfo[] propertyInfos = type.GetProperties();
            string applicationName = ApplicationEnum.AuditLogAPP.GetApplication();

            var cmd = _connector.connection.CreateCommand() as SqlCommand;
            SqlTransaction transaction = _connector.connection.BeginTransaction("");
            cmd.Transaction = transaction;


            try
            {

                // Insert into AuditEntry


                cmd.CommandText = @"INSERT INTO [AuditEntry] (EntitySetName, EntityTypeName, State, StateName, CreatedBy, CreatedDate, Discriminator, AppplicationName) 
                                    OUTPUT inserted.AuditEntryID
                                    VALUES (@EntitySetName, @EntityTypeName, @State,@StateName, @CreatedBy, @CreatedDate, @Discriminator, @AppplicationName);
                                    SELECT SCOPE_IDENTITY();";

                cmd.Parameters.AddWithValue("@EntitySetName", type.Name);
                cmd.Parameters.AddWithValue("@EntityTypeName", type.Name);
                cmd.Parameters.AddWithValue("@State", StateName.EntityDeleted); // 2 for delete
                cmd.Parameters.AddWithValue("@StateName", StateName.EntityDeleted.ToString()); // EntityAdded for insert
                cmd.Parameters.AddWithValue("@CreatedBy", "mahedee"); // It will come from session
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@Discriminator", "CustomAuditEntry"); // CustomAuditEntry Model
                cmd.Parameters.AddWithValue("@AppplicationName", applicationName); // Name of the application

                int auditEntryId = Convert.ToInt32(cmd.ExecuteScalar());

                cmd.Parameters.Clear();


                // Insert into AuditEntryProperty
                // Values of each property

                cmd.CommandText = @"INSERT INTO [AuditEntryProperty] (AuditEntryID, RelationName, PropertyName, OldValue, NewValue, Discriminator, AppplicationName) 
                                    VALUES (@AuditEntryID, @RelationName, @PropertyName, @OldValue, @NewValue, @DiscriminatorProperty, @AppplicationNameProperty);
                                    SELECT SCOPE_IDENTITY();";

                cmd.Parameters.Add(new SqlParameter("@AuditEntryID", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@RelationName", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("@PropertyName", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("@OldValue", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("@NewValue", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("@DiscriminatorProperty", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("@AppplicationNameProperty", SqlDbType.NVarChar));


                foreach (PropertyInfo propertyInfo in propertyInfos)
                {

                    cmd.Parameters["@AuditEntryID"].Value = auditEntryId;
                    cmd.Parameters["@RelationName"].Value = DBNull.Value;
                    cmd.Parameters["@PropertyName"].Value = propertyInfo.Name; // property name
                    cmd.Parameters["@OldValue"].Value = propertyInfo.GetValue(oldEntity); // property value
                    cmd.Parameters["@NewValue"].Value = DBNull.Value; // Null for delete
                    cmd.Parameters["@DiscriminatorProperty"].Value = "CustomAuditEntryProperty";
                    cmd.Parameters["@AppplicationNameProperty"].Value = applicationName;
                    cmd.ExecuteScalar();
                }

                // Commit transaction
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
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
