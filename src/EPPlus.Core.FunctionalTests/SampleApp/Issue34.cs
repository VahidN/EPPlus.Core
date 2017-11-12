using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfficeOpenXml;

namespace EPPlus.Core.FunctionalTests.SampleApp
{
    [TestClass]
    public class Issue34
    {
        [TestMethod]
        public void RunIssue34()
        {
            string file = Path.Combine("bin", "issue34.xlsx");
            const string password = "EPPlus";
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Employee");

                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Gender";
                worksheet.Cells[1, 4].Value = "Salary (in $)";

                worksheet.Cells[2, 1].Value = 1000;
                worksheet.Cells[2, 2].Value = "Jon";
                worksheet.Cells[2, 3].Value = "M";
                worksheet.Cells[2, 4].Value = 3000;

                package.SaveAs(new FileInfo(file), password);  //default encryption (AES128)
            }

            using (var package = new ExcelPackage(new FileInfo(file), password))
            {
                var worksheet = package.Workbook.Worksheets[1];
                Console.WriteLine(worksheet.Cells[2, 1].Value);
                Assert.AreEqual("1000", worksheet.Cells[2, 1].Value.ToString());
            }
        }
    }
}