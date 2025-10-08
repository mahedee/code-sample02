using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ExportReportAPI.Generator.Interfaces;
using ExportReportAPI.Models;

namespace ExportReportAPI.Generator.Templates
{
    /// <summary>
    /// Professional Word report template with company branding
    /// </summary>
    public class WordTemplate : IWordTemplate
    {
        private readonly string _logoPath;

        public WordTemplate()
        {
            _logoPath = Path.Combine(AppContext.BaseDirectory, "images", "mahedeedotnet_log.png");
        }

        public async Task<ExportResult> GenerateReportAsync(IEnumerable<Employee> data, string reportTitle = "Report", Dictionary<string, object>? additionalInfo = null)
        {
            return await GenerateWordReportAsync(data, reportTitle, additionalInfo);
        }

        public async Task<ExportResult> GenerateWordReportAsync(IEnumerable<Employee> employees, string reportTitle = "Employee Report", Dictionary<string, object>? additionalInfo = null)
        {
            var memoryStream = new MemoryStream();
            
            using (var wordDocument = WordprocessingDocument.Create(memoryStream, WordprocessingDocumentType.Document))
            {
                var mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                var body = mainPart.Document.AppendChild(new Body());
                
                try
                {
                    // Add company header section
                    AddCompanyHeader(body, mainPart);
                    
                    // Add report title
                    AddReportTitle(body, reportTitle);
                    
                    // Add additional info if provided
                    if (additionalInfo != null && additionalInfo.Any())
                    {
                        AddAdditionalInfo(body, additionalInfo);
                    }
                    
                    // Add data table
                    AddDataTable(body, employees);
                    
                    // Add footer
                    AddFooter(body);
                    
                    mainPart.Document.Save();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error generating Word report: {ex.Message}", ex);
                }
            }
            
            var data = memoryStream.ToArray();
            memoryStream.Dispose();
            
            var result = new ExportResult
            {
                Data = data,
                ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                FileName = $"{reportTitle.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.docx"
            };
            
            return await Task.FromResult(result);
        }

        private void AddCompanyHeader(Body body, MainDocumentPart mainPart)
        {
            // Add company logo - temporarily simplified to avoid crashes
            try
            {
                // For now, use text placeholder to ensure stability
                var logoParagraph = new Paragraph();
                var logoRun = new Run();
                var logoText = new Text("[COMPANY LOGO - Mahedee.net]");
                logoRun.RunProperties = new RunProperties(new Bold(), new FontSize { Val = "12" });
                logoRun.Append(logoText);
                logoParagraph.Append(logoRun);
                logoParagraph.ParagraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center });
                body.Append(logoParagraph);
                
                // TODO: Add proper image embedding after fixing the crash
                /*
                if (File.Exists(_logoPath))
                {
                    var imagePart = mainPart.AddImagePart(ImagePartType.Png);
                    
                    using (var stream = new FileStream(_logoPath, FileMode.Open))
                    {
                        imagePart.FeedData(stream);
                    }
                    
                    var logoParagraph = CreateImageParagraph(mainPart.GetIdOfPart(imagePart), "Company Logo", 150, 75);
                    logoParagraph.ParagraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center });
                    body.Append(logoParagraph);
                }
                */
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logo embedding failed: {ex.Message}");
                // Fallback to text placeholder
                var logoParagraph = new Paragraph();
                var logoRun = new Run();
                var logoText = new Text("[COMPANY LOGO - Mahedee.net]");
                logoRun.RunProperties = new RunProperties(new Bold(), new FontSize { Val = "12" });
                logoRun.Append(logoText);
                logoParagraph.Append(logoRun);
                logoParagraph.ParagraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center });
                body.Append(logoParagraph);
            }
            
            // Company name
            var companyParagraph = new Paragraph();
            var companyRun = new Run();
            var companyText = new Text("Mahedee.net");
            companyRun.RunProperties = new RunProperties(new Bold(), new FontSize { Val = "20" });
            companyRun.Append(companyText);
            companyParagraph.Append(companyRun);
            companyParagraph.ParagraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center });
            body.Append(companyParagraph);
            
            // Company slogan
            var sloganParagraph = new Paragraph();
            var sloganRun = new Run();
            var sloganText = new Text("Think Simple");
            sloganRun.RunProperties = new RunProperties(new FontSize { Val = "16" });
            sloganRun.Append(sloganText);
            sloganParagraph.Append(sloganRun);
            sloganParagraph.ParagraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center });
            body.Append(sloganParagraph);
            
            // Company address
            var addressParagraph = new Paragraph();
            var addressRun = new Run();
            var addressText = new Text("Toronto, ON, Canada");
            addressRun.RunProperties = new RunProperties(new FontSize { Val = "14" });
            addressRun.Append(addressText);
            addressParagraph.Append(addressRun);
            addressParagraph.ParagraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center });
            body.Append(addressParagraph);
            
            // Add spacing
            body.Append(new Paragraph());
        }

        private void AddReportTitle(Body body, string title)
        {
            var titleParagraph = new Paragraph();
            var titleRun = new Run();
            var titleText = new Text(title);
            titleRun.RunProperties = new RunProperties(new Bold(), new FontSize { Val = "24" });
            titleRun.Append(titleText);
            titleParagraph.Append(titleRun);
            titleParagraph.ParagraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center });
            body.Append(titleParagraph);
            
            // Add spacing
            body.Append(new Paragraph());
        }

        private void AddAdditionalInfo(Body body, Dictionary<string, object> additionalInfo)
        {
            foreach (var info in additionalInfo)
            {
                var infoParagraph = new Paragraph();
                var keyRun = new Run();
                keyRun.RunProperties = new RunProperties(new Bold());
                keyRun.Append(new Text($"{info.Key}: "));
                
                var valueRun = new Run();
                valueRun.Append(new Text(info.Value?.ToString() ?? ""));
                
                infoParagraph.Append(keyRun);
                infoParagraph.Append(valueRun);
                body.Append(infoParagraph);
            }
            
            // Add spacing
            body.Append(new Paragraph());
        }

        private void AddDataTable(Body body, IEnumerable<Employee> employees)
        {
            var table = new Table();
            
            // Table properties
            var tableProperties = new TableProperties(
                new TableBorders(
                    new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 }
                ),
                new TableWidth { Width = "5000", Type = TableWidthUnitValues.Pct }
            );
            table.AppendChild(tableProperties);
            
            // Add header row
            var headerRow = new TableRow();
            var headers = new[] { "ID", "Full Name", "Designation", "Department", "Email", "Phone", "Salary", "Join Date" };
            
            foreach (var header in headers)
            {
                var cell = CreateTableCell(header, true);
                headerRow.Append(cell);
            }
            table.Append(headerRow);
            
            // Add data rows
            foreach (var emp in employees)
            {
                var row = new TableRow();
                
                var data = new[]
                {
                    emp.Id.ToString(),
                    emp.FullName,
                    emp.Designation,
                    emp.Department,
                    emp.Email,
                    emp.Phone,
                    emp.Salary.ToString("C"),
                    emp.JoinDate.ToString("yyyy-MM-dd")
                };
                
                foreach (var cellData in data)
                {
                    var cell = CreateTableCell(cellData, false);
                    row.Append(cell);
                }
                table.Append(row);
            }
            
            body.Append(table);
        }

        private TableCell CreateTableCell(string text, bool isHeader)
        {
            var cell = new TableCell();
            var paragraph = new Paragraph();
            var run = new Run();
            
            if (isHeader)
            {
                run.RunProperties = new RunProperties(new Bold());
                cell.TableCellProperties = new TableCellProperties(
                    new Shading { Val = ShadingPatternValues.Clear, Fill = "D3D3D3" }
                );
            }
            
            run.Append(new Text(text));
            paragraph.Append(run);
            cell.Append(paragraph);
            
            return cell;
        }

        private void AddFooter(Body body)
        {
            // Add spacing
            body.Append(new Paragraph());
            
            var footerParagraph = new Paragraph();
            var footerRun = new Run();
            var footerText = new Text($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm}");
            footerRun.RunProperties = new RunProperties(new FontSize { Val = "16" }, new Italic());
            footerRun.Append(footerText);
            footerParagraph.Append(footerRun);
            footerParagraph.ParagraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Center });
            body.Append(footerParagraph);
        }

        // TODO: Re-implement CreateImageParagraph method after fixing Word export issues
        /*
        private Paragraph CreateImageParagraph(string relationshipId, string imageName, int widthEmus, int heightEmus)
        {
            // Convert pixels to EMUs (English Metric Units)
            // 1 pixel = 9525 EMUs at 96 DPI
            var finalWidth = widthEmus * 9525L;
            var finalHeight = heightEmus * 9525L;

            var element = new Drawing(
                new DW.Inline(
                    new DW.Extent() { Cx = finalWidth, Cy = finalHeight },
                    new DW.EffectExtent() { LeftEdge = 0L, TopEdge = 0L, RightEdge = 0L, BottomEdge = 0L },
                    new DW.DocProperties() { Id = 1U, Name = imageName },
                    new DW.NonVisualGraphicFrameDrawingProperties(
                        new A.GraphicFrameLocks() { NoChangeAspect = true }),
                    new A.Graphic(
                        new A.GraphicData(
                            new PIC.Picture(
                                new PIC.NonVisualPictureProperties(
                                    new PIC.NonVisualDrawingProperties() { Id = 0U, Name = imageName },
                                    new PIC.NonVisualPictureDrawingProperties()),
                                new PIC.BlipFill(
                                    new A.Blip() { Embed = relationshipId },
                                    new A.Stretch(new A.FillRectangle()))))
                        { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" }))
                );

            return new Paragraph(new Run(element));
        }
        */
    }
}