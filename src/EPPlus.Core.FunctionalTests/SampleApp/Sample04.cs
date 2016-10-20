/*******************************************************************************
 * You may amend and distribute as you like, but don't remove this header!
 *
 * All rights reserved.
 *
 * EPPlus is an Open Source project provided under the
 * GNU General Public License (GPL) as published by the
 * Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
 *
 * EPPlus provides server-side generation of Excel 2007 spreadsheets.
 * See http://www.codeplex.com/EPPlus for details.
 *
 *
 * The GNU General Public License can be viewed at http://www.opensource.org/licenses/gpl-license.php
 * If you unfamiliar with this license or have questions about it, here is an http://www.gnu.org/licenses/gpl-faq.html
 *
 * The code for this project may be used and redistributed by any means PROVIDING it is
 * not sold for profit without the author's written consent, and providing that this notice
 * and the author's name and all copyright notices remain intact.
 *
 * All code and executables are provided "as is" with no warranty either express or implied.
 * The author accepts no liability for any damage or loss of business that this product may cause.
 *
 *
 * Code change notes:
 *
 * Author							Change						Date
 *******************************************************************************
 * Jan Källman		Added		10-SEP-2009
 *******************************************************************************/

using System;
using System.Drawing;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;

namespace EPPlus.Core.Tests.SampleApp
{
    [TestClass]
    public class Sample04
    {
        /// <summary>
        /// This sample creates a new workbook from a template file containing a chart and populates it with Exchangrates from
        /// the Adventureworks database and set the three series on the chart.
        /// </summary>
        [TestMethod]
        public void RunSample4()
        {
            using (ExcelPackage p = new ExcelPackage(new FileInfo("SampleApp/GraphTemplate.xlsx"), true))
            {
                //Set up the headers
                ExcelWorksheet ws = p.Workbook.Worksheets[1];
                ws.Cells["A20"].Value = "Date";
                ws.Cells["B20"].Value = "EOD Rate";
                ws.Cells["B20:D20"].Merge = true;
                ws.Cells["E20"].Value = "Change";
                ws.Cells["E20:G20"].Merge = true;
                ws.Cells["B20:E20"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                using (ExcelRange r = ws.Cells["A20:G20"])
                {
                    r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.Font.Bold = true;
                }
                ws.Cells["B21"].Value = "USD/JPY";
                ws.Cells["C21"].Value = "USD/EUR";
                ws.Cells["D21"].Value = "USD/GBP";
                ws.Cells["E21"].Value = "USD/JPY";
                ws.Cells["F21"].Value = "USD/EUR";
                ws.Cells["G21"].Value = "USD/GBP";
                using (ExcelRange r = ws.Cells["A21:G21"])
                {
                    r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.Font.Bold = true;
                }

                int startRow = 22;
                //Connect to the database and fill the data
                int row = startRow;
                // get the data and fill rows 22 onwards
                var rnd = new Random();
                while (row<50)
                {
                    ws.Cells[row, 1].Value = rnd.Next();
                    ws.Cells[row, 2].Value = rnd.Next();
                    ws.Cells[row, 3].Value = rnd.Next();
                    ws.Cells[row, 4].Value = rnd.Next();
                    row++;
                }
                //Set the numberformat
                ws.Cells[startRow, 1, row - 1, 1].Style.Numberformat.Format = "yyyy-mm-dd";
                ws.Cells[startRow, 2, row - 1, 4].Style.Numberformat.Format = "#,##0.0000";
                //Set the Formulas
                ws.Cells[startRow + 1, 5, row - 1, 7].Formula = string.Format("B${0}/B{1}-1", startRow, startRow + 1);
                ws.Cells[startRow, 5, row - 1, 7].Style.Numberformat.Format = "0.00%";

                //Set the series for the chart. The series must exist in the template or the program will crash.
                ExcelChart chart = ((ExcelChart)ws.Drawings["SampleChart"]);
                chart.Title.Text = "Exchange rate %";
                chart.Series[0].Header = "USD/JPY";
                chart.Series[0].XSeries = "'" + ws.Name + "'!" + ExcelRange.GetAddress(startRow + 1, 1, row - 1, 1);
                chart.Series[0].Series = "'" + ws.Name + "'!" + ExcelRange.GetAddress(startRow + 1, 5, row - 1, 5);

                chart.Series[1].Header = "USD/EUR";
                chart.Series[1].XSeries = "'" + ws.Name + "'!" + ExcelRange.GetAddress(startRow + 1, 1, row - 1, 1);
                chart.Series[1].Series = "'" + ws.Name + "'!" + ExcelRange.GetAddress(startRow + 1, 6, row - 1, 6);

                chart.Series[2].Header = "USD/GBP";
                chart.Series[2].XSeries = "'" + ws.Name + "'!" + ExcelRange.GetAddress(startRow + 1, 1, row - 1, 1);
                chart.Series[2].Series = "'" + ws.Name + "'!" + ExcelRange.GetAddress(startRow + 1, 7, row - 1, 7);

                //Get the documet as a byte array from the stream and save it to disk.  (This is usefull in a webapplication) ...
                Byte[] bin = p.GetAsByteArray();

                string file = Path.Combine("bin", "sample4.xlsx");
                File.WriteAllBytes(file, bin);
            }
        }
    }
}