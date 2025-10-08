using ExportReportAPI.Models;

namespace ExportReportAPI.Generator.Interfaces
{
    /// <summary>
    /// Base interface for all report templates
    /// </summary>
    /// <typeparam name="T">The data type for the report</typeparam>
    public interface IReportTemplate<T>
    {
        /// <summary>
        /// Generates a report based on the provided data
        /// </summary>
        /// <param name="data">The data to include in the report</param>
        /// <param name="reportTitle">The title of the report</param>
        /// <param name="additionalInfo">Any additional information for the report</param>
        /// <returns>ExportResult containing the generated report</returns>
        Task<ExportResult> GenerateReportAsync(IEnumerable<T> data, string reportTitle = "Report", Dictionary<string, object>? additionalInfo = null);
    }
}