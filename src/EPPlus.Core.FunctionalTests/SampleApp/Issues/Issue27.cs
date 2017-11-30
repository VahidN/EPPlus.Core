using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfficeOpenXml;

namespace EPPlus.Core.FunctionalTests.SampleApp.Issues
{
    [TestClass]
    public class Issue27
    {
        [TestMethod]
        public void RunIssue27()
        {
            var contracts = getContracts();
            using (var package = new ExcelPackage())
            {
                foreach (var contract in contracts)
                {
                    var worksheet = package.Workbook.Worksheets.Add(contract.Name);
                    var dt = contract.Activities.Select(r => new
                    {
                        r.Name,
                        r.User,
                        r.Status,
                        r.Time
                    })
                    .ToList();
                    worksheet.Cells["A1"].LoadFromCollection(dt, true, OfficeOpenXml.Table.TableStyles.Medium2);
                }

                var result = package.GetAsByteArray();
                Assert.IsNotNull(result);
            }
        }

        private static IList<Contract> getContracts()
        {
            return new List<Contract>
            {
                 new Contract
                 {
                     Name = "C 1",
                     Activities = new List<Activity>
                    {
                        new Activity
                        {
                            Name = "A 1",
                            User = "U 1",
                            Status ="S 1",
                            Time = DateTime.Now
                        }
                    }
                 },
                 new Contract
                 {
                     Name = "C 2",
                     Activities = new List<Activity>
                    {
                        new Activity
                        {
                            Name = "A 2",
                            User = "U 2",
                            Status ="S 2",
                            Time = DateTime.Now
                        }
                    }
                 }
            };
        }
    }

    public class Contract
    {
        public string Name { set; get; }
        public IList<Activity> Activities { set; get; }
    }

    public class Activity
    {
        public string Name { set; get; }
        public string User { set; get; }
        public string Status { set; get; }
        public DateTime Time { set; get; }
    }
}