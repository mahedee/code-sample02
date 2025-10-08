using ExportReportAPI.Generator.Interfaces;
using ExportReportAPI.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ExportReportAPI.Generator.Templates
{
    /// <summary>
    /// Professional PDF report template with company branding
    /// </summary>
    public class PDFTemplate : IPDFTemplate
    {
        private readonly string _logoPath;

        public PDFTemplate()
        {
            _logoPath = Path.Combine(AppContext.BaseDirectory, "images", "mahedeedotnet_log.png");
        }

        public async Task<ExportResult> GenerateReportAsync(IEnumerable<Employee> data, string reportTitle = "Report", Dictionary<string, object>? additionalInfo = null)
        {
            return await GeneratePDFReportAsync(data, reportTitle, additionalInfo);
        }

        public async Task<ExportResult> GeneratePDFReportAsync(IEnumerable<Employee> employees, string reportTitle = "Employee Report", Dictionary<string, object>? additionalInfo = null)
        {
            using var memoryStream = new MemoryStream();
            var document = new Document(PageSize.A4.Rotate());
            var writer = PdfWriter.GetInstance(document, memoryStream);
            
            // Add header and footer event handler
            var events = new PDFHeaderFooterEvent();
            writer.PageEvent = events;
            
            document.Open();
            
            try
            {
                // Add company header section
                AddCompanyHeader(document);
                
                // Add report title
                AddReportTitle(document, reportTitle);
                
                // Add additional info if provided
                if (additionalInfo != null && additionalInfo.Any())
                {
                    AddAdditionalInfo(document, additionalInfo);
                }
                
                // Add data table
                AddDataTable(document, employees);
                
                document.Close();
            }
            catch (Exception ex)
            {
                document.Close();
                throw new Exception($"Error generating PDF report: {ex.Message}", ex);
            }
            
            var result = new ExportResult
            {
                Data = memoryStream.ToArray(),
                ContentType = "application/pdf",
                FileName = $"{reportTitle.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
            };

            return await Task.FromResult(result);
        }

        private void AddCompanyHeader(Document document)
        {
            // Add company logo
            try
            {
                if (File.Exists(_logoPath))
                {
                    var logo = Image.GetInstance(_logoPath);
                    logo.ScaleToFit(100f, 50f);
                    logo.Alignment = Element.ALIGN_LEFT;
                    document.Add(logo);
                }
                else
                {
                    // Try alternative path
                    var alternativeLogoPath = Path.Combine(Directory.GetCurrentDirectory(), "images", "mahedeedotnet_log.png");
                    if (File.Exists(alternativeLogoPath))
                    {
                        var logo = Image.GetInstance(alternativeLogoPath);
                        logo.ScaleToFit(100f, 50f);
                        logo.Alignment = Element.ALIGN_LEFT;
                        document.Add(logo);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logo loading failed: {ex.Message}");
            }
            
            // Add company information
            var companyFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
            var sloganFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
            var addressFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            
            var companyName = new Paragraph("Mahedee.net", companyFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 5
            };
            document.Add(companyName);
            
            var slogan = new Paragraph("Think Simple", sloganFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 5
            };
            document.Add(slogan);
            
            var address = new Paragraph("Toronto, ON, Canada", addressFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 20
            };
            document.Add(address);
        }

        private void AddReportTitle(Document document, string title)
        {
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            var titleParagraph = new Paragraph(title, titleFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 20
            };
            document.Add(titleParagraph);
        }

        private void AddAdditionalInfo(Document document, Dictionary<string, object> additionalInfo)
        {
            var infoFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            
            foreach (var info in additionalInfo)
            {
                var infoParagraph = new Paragraph($"{info.Key}: {info.Value}", infoFont)
                {
                    SpacingAfter = 5
                };
                document.Add(infoParagraph);
            }
            
            document.Add(new Paragraph(" ", infoFont) { SpacingAfter = 10 }); // Add space
        }

        private void AddDataTable(Document document, IEnumerable<Employee> employees)
        {
            var table = new PdfPTable(8);
            table.WidthPercentage = 100;
            
            // Set column widths
            table.SetWidths(new float[] { 1f, 2f, 2f, 1.5f, 2.5f, 1.5f, 1.5f, 1.5f });
            
            // Add headers
            var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9);
            var headers = new[] { "ID", "Full Name", "Designation", "Department", "Email", "Phone", "Salary", "Join Date" };
            
            foreach (var header in headers)
            {
                var cell = new PdfPCell(new Phrase(header, headerFont))
                {
                    BackgroundColor = new BaseColor(173, 216, 230),
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };
                table.AddCell(cell);
            }
            
            // Add data rows
            var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 8);
            foreach (var emp in employees)
            {
                table.AddCell(CreateDataCell(emp.Id.ToString(), cellFont));
                table.AddCell(CreateDataCell(emp.FullName, cellFont));
                table.AddCell(CreateDataCell(emp.Designation, cellFont));
                table.AddCell(CreateDataCell(emp.Department, cellFont));
                table.AddCell(CreateDataCell(emp.Email, cellFont));
                table.AddCell(CreateDataCell(emp.Phone, cellFont));
                table.AddCell(CreateDataCell(emp.Salary.ToString("C"), cellFont));
                table.AddCell(CreateDataCell(emp.JoinDate.ToString("yyyy-MM-dd"), cellFont));
            }
            
            document.Add(table);
        }

        private PdfPCell CreateDataCell(string content, Font font)
        {
            return new PdfPCell(new Phrase(content, font))
            {
                Padding = 3,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
        }
    }

    /// <summary>
    /// Event handler for PDF header and footer
    /// </summary>
    public class PDFHeaderFooterEvent : PdfPageEventHelper
    {
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            
            // Add footer using direct content
            var cb = writer.DirectContent;
            var dateFont = FontFactory.GetFont(FontFactory.HELVETICA, 8);
            
            // Add date on the left
            var dateText = $"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm}";
            ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(dateText, dateFont), 36, 30, 0);
            
            // Add page number on the right
            var pageText = $"Page {writer.PageNumber}";
            ColumnText.ShowTextAligned(cb, Element.ALIGN_RIGHT, new Phrase(pageText, dateFont), document.PageSize.Width - 36, 30, 0);
        }
    }
}