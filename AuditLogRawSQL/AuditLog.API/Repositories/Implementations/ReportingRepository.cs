using AuditLog.API.AuditTrail.Interfaces;
using AuditLog.API.Models;
using AuditLog.API.Persistence;
using AuditLog.API.Repositories.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace AuditLog.API.Repositories.Implementations
{
    public class ReportingRepository : IReportingRepository
    {
        private DBConnector _connector;
        public ReportingRepository(DBConnector dBConnector) 
        {
            _connector = dBConnector;
        }
        public Task<IEnumerable<dynamic>> GetChangeLogDynamic(string EntityName)
        {
            try
            {
                if (this._connector.connection.State == ConnectionState.Closed)
                    this._connector.connection.Open();
                var cmd = this._connector.connection.CreateCommand() as SqlCommand;
                cmd.CommandText = @"  SELECT ae.AuditEntryID, ae.EntityTypeName, ae.StateName, ae.CreatedDate, aep.AuditEntryPropertyID, aep.PropertyName, aep.OldValue, aep.NewValue, aep.AppplicationName
                                  FROM AuditEntry ae INNER JOIN AuditEntryProperty aep
                                  ON ae.AuditEntryID = aep.AuditEntryID
                                  WHERE ae.EntityTypeName = @EntityName
                                  ORDER by ae.CreatedDate";
                cmd.Parameters.AddWithValue("@EntityName", EntityName);
                var reader = cmd.ExecuteReader();
                var dt = new DataTable();
                dt.Load(reader);
                var list = new List<dynamic>();
                foreach (DataRow row in dt.Rows)
                {
                    var expandoObject = new ExpandoObject() as IDictionary<string, object>;
                    foreach (DataColumn col in dt.Columns)
                    {
                        expandoObject.Add(col.ColumnName, row[col]);
                    }
                    list.Add(expandoObject);
                }
                return Task.FromResult(list.AsEnumerable());
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
    }
}
