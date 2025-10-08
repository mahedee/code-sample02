using ExportReportAPI.Models;

namespace ExportReportAPI.Generator.Interfaces
{
    /// <summary>
    /// Interface for Excel report generation
    /// </summary>
    public interface IExcelTemplate : IReportTemplate<Employee>
    {
        /// <summary>
        /// Generates an Excel report for employees
        /// </summary>
        /// <param name="employees">List of employees</param>
        /// <param name="reportTitle">Title of the report</param>
        /// <param name="additionalInfo">Additional report information</param>
        /// <returns>ExportResult with Excel file</returns>
        Task<ExportResult> GenerateExcelReportAsync(IEnumerable<Employee> employees, string reportTitle = "Employee Report", Dictionary<string, object>? additionalInfo = null);
    }
}