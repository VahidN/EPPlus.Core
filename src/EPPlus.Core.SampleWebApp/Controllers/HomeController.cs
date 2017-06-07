using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace EPPlus.Core.SampleWebApp.Controllers
{
    public class HomeController : Controller
    {
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// /Home/FileReport
        /// </summary>
        public IActionResult FileReport()
        {
            var fileDownloadName = "report.xlsx";
            var reportsFolder = "reports";

            using (var package = createExcelPackage())
            {
                package.SaveAs(new FileInfo(Path.Combine(_hostingEnvironment.WebRootPath, reportsFolder, fileDownloadName)));
            }
            return File($"~/{reportsFolder}/{fileDownloadName}", XlsxContentType, fileDownloadName);
        }

        /// <summary>
        /// An in-memory report
        /// </summary>
        public IActionResult Index()
        {
            byte[] reportBytes;
            using (var package = createExcelPackage())
            {
                reportBytes = package.GetAsByteArray();
            }

            return File(reportBytes, XlsxContentType, "report.xlsx");
        }

        /// <summary>
        /// /Home/ReadFile
        /// </summary>
        public IActionResult ReadFile()
        {
            var fileDownloadName = "report.xlsx";
            var reportsFolder = "reports";
            var fileInfo = new FileInfo(Path.Combine(_hostingEnvironment.WebRootPath, reportsFolder, fileDownloadName));
            if (!fileInfo.Exists)
            {
                using (var package = createExcelPackage())
                {
                    package.SaveAs(fileInfo);
                }
            }

            return Content(readExcelPackage(fileInfo, worksheetName: "Employee"));
        }

        private string readExcelPackage(FileInfo fileInfo, string worksheetName)
        {
            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[worksheetName];
                int rowCount = worksheet.Dimension.Rows;
                int ColCount = worksheet.Dimension.Columns;

                var sb = new StringBuilder();
                for (int row = 1; row <= rowCount; row++)
                {
                    for (int col = 1; col <= ColCount; col++)
                    {
                        sb.AppendFormat("{0}\t", worksheet.Cells[row, col].Value);
                    }
                    sb.Append(Environment.NewLine);
                }
                return sb.ToString();
            }
        }

        private ExcelPackage createExcelPackage()
        {
            var package = new ExcelPackage();
            package.Workbook.Properties.Title = "Salary Report";
            package.Workbook.Properties.Author = "Vahid N.";
            package.Workbook.Properties.Subject = "Salary Report";
            package.Workbook.Properties.Keywords = "Salary";


            var worksheet = package.Workbook.Worksheets.Add("Employee");

            //First add the headers
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Name";
            worksheet.Cells[1, 3].Value = "Gender";
            worksheet.Cells[1, 4].Value = "Salary (in $)";

            //Add values

            var numberformat = "#,##0";
            var dataCellStyleName = "TableNumber";
            var numStyle = package.Workbook.Styles.CreateNamedStyle(dataCellStyleName);
            numStyle.Style.Numberformat.Format = numberformat;

            worksheet.Cells[2, 1].Value = 1000;
            worksheet.Cells[2, 2].Value = "Jon";
            worksheet.Cells[2, 3].Value = "M";
            worksheet.Cells[2, 4].Value = 5000;
            worksheet.Cells[2, 4].Style.Numberformat.Format = numberformat;

            worksheet.Cells[3, 1].Value = 1001;
            worksheet.Cells[3, 2].Value = "Graham";
            worksheet.Cells[3, 3].Value = "M";
            worksheet.Cells[3, 4].Value = 10000;
            worksheet.Cells[3, 4].Style.Numberformat.Format = numberformat;

            worksheet.Cells[4, 1].Value = 1002;
            worksheet.Cells[4, 2].Value = "Jenny";
            worksheet.Cells[4, 3].Value = "F";
            worksheet.Cells[4, 4].Value = 5000;
            worksheet.Cells[4, 4].Style.Numberformat.Format = numberformat;

            // Add to table / Add summary row
            var tbl = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 1, fromCol: 1, toRow: 4, toColumn: 4), "Data");
            tbl.ShowHeader = true;
            tbl.TableStyle = TableStyles.Dark9;
            tbl.ShowTotal = true;
            tbl.Columns[3].DataCellStyleName = dataCellStyleName;
            tbl.Columns[3].TotalsRowFunction = RowFunctions.Sum;
            worksheet.Cells[5, 4].Style.Numberformat.Format = numberformat;

            // AutoFitColumns
            worksheet.Cells[1, 1, 4, 4].AutoFitColumns();

            return package;
        }
    }
}