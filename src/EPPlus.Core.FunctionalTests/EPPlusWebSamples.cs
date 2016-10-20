using System;
using System.Drawing;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace EPPlus.Core.Tests
{
    [TestClass]
    public class EPPlusWebSamples
    {
        [TestMethod]
        public void Verify_DemonstrateTheSaveAsMethod_CanBeCreated()
        {
            ExcelPackage pck = new ExcelPackage();
            var ws = pck.Workbook.Worksheets.Add("Sample1");

            ws.Cells["A1"].Value = "Sample 1";
            ws.Cells["A1"].Style.Font.Bold = true;
            var shape = ws.Drawings.AddShape("Shape1", eShapeStyle.Rect);
            shape.SetPosition(50, 200);
            shape.SetSize(200, 100);
            shape.Text = "Sample 1 saves to the Response.OutputStream";

            pck.SaveAs(new FileInfo(Path.Combine("bin", "Web.Sample1.xlsx")));
        }

        [TestMethod]
        public void Verify_DemonstrateTheGetAsByteArrayMethod_CanBeCreated()
        {
            ExcelPackage pck = new ExcelPackage();
            var ws = pck.Workbook.Worksheets.Add("Sample2");

            ws.Cells["A1"].Value = "Sample 2";
            ws.Cells["A1"].Style.Font.Bold = true;
            var shape = ws.Drawings.AddShape("Shape1", eShapeStyle.Rect);
            shape.SetPosition(50, 200);
            shape.SetSize(200, 100);
            shape.Text = "Sample 2 outputs the sheet using the Response.BinaryWrite method";

            var data = pck.GetAsByteArray();
            File.WriteAllBytes(Path.Combine("bin", "Web.Sample2.xlsx"), data);
        }

        [TestMethod]
        public void Verify_Uses_A_CachedTemplate_CanBeCreated()
        {
            //Here we create the template.
            //As an alternative the template could be loaded from disk or from a resource.
            ExcelPackage pckTemplate = new ExcelPackage();
            var wsTemplate = pckTemplate.Workbook.Worksheets.Add("Sample3");

            wsTemplate.Cells["A1"].Value = "Sample 3";
            wsTemplate.Cells["A1"].Style.Font.Bold = true;
            var shape = wsTemplate.Drawings.AddShape("Shape1", eShapeStyle.Rect);
            shape.SetPosition(50, 200);
            shape.SetSize(200, 100);
            shape.Text = "Sample 3 uses a template that is stored in the application cashe.";
            pckTemplate.Save();

            var Sample3Template = pckTemplate.Stream;

            //Open the new package with the template stream.
            //The template stream is copied to the new stream in the constructor
            ExcelPackage pck = new ExcelPackage(new MemoryStream(), Sample3Template);
            var ws = pck.Workbook.Worksheets[1];
            int row = new Random().Next(10) + 10;   //Pick a random row to print the text
            ws.Cells[row, 1].Value = "We make a small change here, after the template has been loaded...";
            ws.Cells[row, 1, row, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[row, 1, row, 5].Style.Fill.BackgroundColor.SetColor(Color.LightGoldenrodYellow);

            var data = pck.GetAsByteArray();
            File.WriteAllBytes(Path.Combine("bin", "Web.Sample3.xlsx"), data);
        }

        [TestMethod]
        public void Verify_VBA_Sample_CanBeCreated()
        {
            ExcelPackage pck = new ExcelPackage();

            //Add a worksheet.
            var ws = pck.Workbook.Worksheets.Add("VBA Sample");
            ws.Drawings.AddShape("VBASampleRect", eShapeStyle.RoundRect);

            //Create a vba project
            pck.Workbook.CreateVBAProject();

            //Now add some code that creates a bubble chart...
            var sb = new StringBuilder();

            sb.AppendLine("Private Sub Workbook_Open()");
            sb.AppendLine("    [VBA Sample].Shapes(\"VBASampleRect\").TextEffect.Text = \"This text is set from VBA!\"");
            sb.AppendLine("End Sub");
            pck.Workbook.CodeModule.Code = sb.ToString();

            var data = pck.GetAsByteArray();
            File.WriteAllBytes(Path.Combine("bin", "Web.Sample4.xlsm"), data);
        }
    }
}