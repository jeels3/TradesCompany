using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesCompany.Application.DTOs;

namespace TradesCompany.Infrastructure.Services
{
    public class ExcelService : Controller
    {
        public IActionResult GenerateExcelFile(ChartModel model)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Data");

                // Add headers
                var rowcnt = 1; 
                foreach (var row in model.Data)
                {
                    worksheet.Cell(rowcnt,1).Value = row.Label;
                    worksheet.Cell(rowcnt,2).Value = row.Value;
                    rowcnt++;
                }
                worksheet.Columns().AdjustToContents();
                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0; // Reset stream position for reading

                    // 4. Return the file as a FileContentResult
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MyData.xlsx");
            }
        }
    }
}
