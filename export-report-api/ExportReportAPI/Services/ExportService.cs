using ExportReportAPI.Generator.Interfaces;
using ExportReportAPI.Models;

namespace ExportReportAPI.Services
{
    /// <summary>
    /// Export service that uses template classes for report generation
    /// </summary>
    public class ExportService : IExportService
    {
        private readonly IExcelTemplate _excelTemplate;
        private readonly IPDFTemplate _pdfTemplate;
        private readonly IWordTemplate _wordTemplate;

        public ExportService(IExcelTemplate excelTemplate, IPDFTemplate pdfTemplate, IWordTemplate wordTemplate)
        {
            _excelTemplate = excelTemplate;
            _pdfTemplate = pdfTemplate;
            _wordTemplate = wordTemplate;
        }

        public async Task<ExportResult> ExportToExcelAsync(IEnumerable<Employee> employees)
        {
            return await _excelTemplate.GenerateExcelReportAsync(employees, "Employee Report");
        }

        public async Task<ExportResult> ExportToPdfAsync(IEnumerable<Employee> employees)
        {
            return await _pdfTemplate.GeneratePDFReportAsync(employees, "Employee Report");
        }

        public async Task<ExportResult> ExportToWordAsync(IEnumerable<Employee> employees)
        {
            return await _wordTemplate.GenerateWordReportAsync(employees, "Employee Report");
        }

        /// <summary>
        /// Export to Excel with custom title and additional information
        /// </summary>
        public async Task<ExportResult> ExportToExcelAsync(IEnumerable<Employee> employees, string reportTitle, Dictionary<string, object>? additionalInfo = null)
        {
            return await _excelTemplate.GenerateExcelReportAsync(employees, reportTitle, additionalInfo);
        }

        /// <summary>
        /// Export to PDF with custom title and additional information
        /// </summary>
        public async Task<ExportResult> ExportToPdfAsync(IEnumerable<Employee> employees, string reportTitle, Dictionary<string, object>? additionalInfo = null)
        {
            return await _pdfTemplate.GeneratePDFReportAsync(employees, reportTitle, additionalInfo);
        }

        /// <summary>
        /// Export to Word with custom title and additional information
        /// </summary>
        public async Task<ExportResult> ExportToWordAsync(IEnumerable<Employee> employees, string reportTitle, Dictionary<string, object>? additionalInfo = null)
        {
            return await _wordTemplate.GenerateWordReportAsync(employees, reportTitle, additionalInfo);
        }
    }
}