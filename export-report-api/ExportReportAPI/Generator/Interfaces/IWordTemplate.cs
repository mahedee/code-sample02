using ExportReportAPI.Models;

namespace ExportReportAPI.Generator.Interfaces
{
    /// <summary>
    /// Interface for Word report generation
    /// </summary>
    public interface IWordTemplate : IReportTemplate<Employee>
    {
        /// <summary>
        /// Generates a Word report for employees
        /// </summary>
        /// <param name="employees">List of employees</param>
        /// <param name="reportTitle">Title of the report</param>
        /// <param name="additionalInfo">Additional report information</param>
        /// <returns>ExportResult with Word file</returns>
        Task<ExportResult> GenerateWordReportAsync(IEnumerable<Employee> employees, string reportTitle = "Employee Report", Dictionary<string, object>? additionalInfo = null);
    }
}