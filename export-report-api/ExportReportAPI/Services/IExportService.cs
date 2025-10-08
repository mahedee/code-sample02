using ExportReportAPI.Models;

namespace ExportReportAPI.Services
{
    public interface IExportService
    {
        Task<ExportResult> ExportToExcelAsync(IEnumerable<Employee> employees);
        Task<ExportResult> ExportToPdfAsync(IEnumerable<Employee> employees);
        Task<ExportResult> ExportToWordAsync(IEnumerable<Employee> employees);
    }
}