using ExportReportAPI.Generator.Interfaces;
using ExportReportAPI.Models;
using OfficeOpenXml;

namespace ExportReportAPI.Generator.Templates
{
    /// <summary>
    /// Professional Excel report template with company branding
    /// </summary>
    public class ExcelTemplate : IExcelTemplate
    {
        private readonly string _logoPath;

        public ExcelTemplate()
        {
            _logoPath = Path.Combine(AppContext.BaseDirectory, "images", "mahedeedotnet_log.png");
        }

        public async Task<ExportResult> GenerateReportAsync(IEnumerable<Employee> data, string reportTitle = "Report", Dictionary<string, object>? additionalInfo = null)
        {
            return await GenerateExcelReportAsync(data, reportTitle, additionalInfo);
        }

        public async Task<ExportResult> GenerateExcelReportAsync(IEnumerable<Employee> employees, string reportTitle = "Employee Report", Dictionary<string, object>? additionalInfo = null)
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Employees");
            
            int currentRow = 1;
            
            // Add company header section
            currentRow = AddCompanyHeader(worksheet, currentRow);
            
            // Add report title
            currentRow = AddReportTitle(worksheet, reportTitle, currentRow);
            
            // Add additional info if provided
            if (additionalInfo != null && additionalInfo.Any())
            {
                currentRow = AddAdditionalInfo(worksheet, additionalInfo, currentRow);
            }
            
            // Add data table
            currentRow = AddDataTable(worksheet, employees, currentRow);
            
            // Add footer
            AddFooter(worksheet, currentRow);
            
            // Apply styling and formatting
            ApplyWorksheetFormatting(worksheet);

            var result = new ExportResult
            {
                Data = package.GetAsByteArray(),
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileName = $"{reportTitle.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            };

            return await Task.FromResult(result);
        }

        private int AddCompanyHeader(ExcelWorksheet worksheet, int startRow)
        {
            // Add company logo
            if (File.Exists(_logoPath))
            {
                var picture = worksheet.Drawings.AddPicture("CompanyLogo", new FileInfo(_logoPath));
                picture.SetPosition(0, 0, 0, 0);
                picture.SetSize(150, 75);
            }
            
            int currentRow = startRow + 4; // Leave space for logo
            
            // Company name
            worksheet.Cells[currentRow, 1].Value = "Mahedee.net";
            worksheet.Cells[currentRow, 1].Style.Font.Bold = true;
            worksheet.Cells[currentRow, 1].Style.Font.Size = 16;
            worksheet.Cells[currentRow, 1, currentRow, 8].Merge = true;
            worksheet.Cells[currentRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            currentRow++;
            
            // Company slogan
            worksheet.Cells[currentRow, 1].Value = "Think Simple";
            worksheet.Cells[currentRow, 1].Style.Font.Size = 12;
            worksheet.Cells[currentRow, 1, currentRow, 8].Merge = true;
            worksheet.Cells[currentRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            currentRow++;
            
            // Company address
            worksheet.Cells[currentRow, 1].Value = "Toronto, ON, Canada";
            worksheet.Cells[currentRow, 1].Style.Font.Size = 10;
            worksheet.Cells[currentRow, 1, currentRow, 8].Merge = true;
            worksheet.Cells[currentRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            currentRow += 2;
            
            return currentRow;
        }

        private int AddReportTitle(ExcelWorksheet worksheet, string title, int startRow)
        {
            worksheet.Cells[startRow, 1].Value = title;
            worksheet.Cells[startRow, 1].Style.Font.Bold = true;
            worksheet.Cells[startRow, 1].Style.Font.Size = 14;
            worksheet.Cells[startRow, 1, startRow, 8].Merge = true;
            worksheet.Cells[startRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            
            return startRow + 2;
        }

        private int AddAdditionalInfo(ExcelWorksheet worksheet, Dictionary<string, object> additionalInfo, int startRow)
        {
            foreach (var info in additionalInfo)
            {
                worksheet.Cells[startRow, 1].Value = $"{info.Key}:";
                worksheet.Cells[startRow, 1].Style.Font.Bold = true;
                worksheet.Cells[startRow, 2].Value = info.Value?.ToString();
                startRow++;
            }
            
            return startRow + 1;
        }

        private int AddDataTable(ExcelWorksheet worksheet, IEnumerable<Employee> employees, int startRow)
        {
            // Add headers
            string[] headers = { "ID", "Full Name", "Designation", "Department", "Email", "Phone", "Salary", "Join Date" };
            
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[startRow, i + 1].Value = headers[i];
            }
            
            // Style headers
            using (var range = worksheet.Cells[startRow, 1, startRow, headers.Length])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            }

            startRow++;
            
            // Add data
            var employeeList = employees.ToList();
            for (int i = 0; i < employeeList.Count; i++)
            {
                var emp = employeeList[i];
                worksheet.Cells[startRow + i, 1].Value = emp.Id;
                worksheet.Cells[startRow + i, 2].Value = emp.FullName;
                worksheet.Cells[startRow + i, 3].Value = emp.Designation;
                worksheet.Cells[startRow + i, 4].Value = emp.Department;
                worksheet.Cells[startRow + i, 5].Value = emp.Email;
                worksheet.Cells[startRow + i, 6].Value = emp.Phone;
                worksheet.Cells[startRow + i, 7].Value = emp.Salary;
                worksheet.Cells[startRow + i, 7].Style.Numberformat.Format = "$#,##0.00";
                worksheet.Cells[startRow + i, 8].Value = emp.JoinDate.ToString("yyyy-MM-dd");
                
                // Add borders
                using (var range = worksheet.Cells[startRow + i, 1, startRow + i, headers.Length])
                {
                    range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                }
            }
            
            return startRow + employeeList.Count + 2;
        }

        private void AddFooter(ExcelWorksheet worksheet, int startRow)
        {
            worksheet.Cells[startRow, 1].Value = $"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm}";
            worksheet.Cells[startRow, 1, startRow, 8].Merge = true;
            worksheet.Cells[startRow, 1].Style.Font.Size = 9;
            worksheet.Cells[startRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            worksheet.Cells[startRow, 1].Style.Font.Italic = true;
        }

        private void ApplyWorksheetFormatting(ExcelWorksheet worksheet)
        {
            // Auto-fit columns
            worksheet.Cells.AutoFitColumns();
            
            // Set minimum column widths
            for (int col = 1; col <= 8; col++)
            {
                if (worksheet.Column(col).Width < 10)
                {
                    worksheet.Column(col).Width = 10;
                }
            }
        }
    }
}