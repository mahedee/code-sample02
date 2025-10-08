using ExportReportAPI.Models;

namespace ExportReportAPI.Generator.Interfaces
{
    /// <summary>
    /// Interface for PDF report generation
    /// </summary>
    public interface IPDFTemplate : IReportTemplate<Employee>
    {
        /// <summary>
        /// Generates a PDF report for employees
        /// </summary>
        /// <param name="employees">List of employees</param>
        /// <param name="reportTitle">Title of the report</param>
        /// <param name="additionalInfo">Additional report information</param>
        /// <returns>ExportResult with PDF file</returns>
        Task<ExportResult> GeneratePDFReportAsync(IEnumerable<Employee> employees, string reportTitle = "Employee Report", Dictionary<string, object>? additionalInfo = null);
    }
}