using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfficeOpenXml;

namespace EPPlus.Core.FunctionalTests.SampleApp.Issues
{
    [TestClass]
    public class Issue38
    {
        [TestMethod]
        public void RunIssue38()
        {
            var fileInfo = new FileInfo("../../../SampleApp/Issues/Files/Issue38.xlsx");
            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[1];
                Assert.IsNull(worksheet.Dimension);
            }
        }
    }
}