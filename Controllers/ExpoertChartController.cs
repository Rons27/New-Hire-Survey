using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace VXI_New_Hire_Survey_Tool.Controllers
{
    public class ExpoertChartController : Controller
    {
        // POST: ExpoertChart/GenerateExcelWithCharts
        public ActionResult GenerateExcelWithCharts(List<List<List<object>>> worksheetData)
        {
            // Ensure non-commercial license for EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Survey Data");
                int rowIndex = 1;

                // Insert headers once at the top of the worksheet
                worksheet.Cells[rowIndex, 1].Value = "Category";
                worksheet.Cells[rowIndex, 2].Value = "Strongly Disagree";
                worksheet.Cells[rowIndex, 3].Value = "Disagree";
                worksheet.Cells[rowIndex, 4].Value = "Neutral";
                worksheet.Cells[rowIndex, 5].Value = "Agree";
                worksheet.Cells[rowIndex, 6].Value = "Strongly Agree";
                rowIndex++;

                // Log received data to verify the structure
                Console.WriteLine("Received Worksheet Data Count: " + worksheetData.Count);

                // Loop through data and insert it into the worksheet
                foreach (var categoryData in worksheetData)
                {
                    // Insert category data
                    foreach (var rowData in categoryData)
                    {
                        // Check if the rowData has at least 6 elements and handle accordingly
                        worksheet.Cells[rowIndex, 1].Value = rowData.Count > 0 ? rowData[0]?.ToString() ?? "Missing Data" : "Missing Data";
                        worksheet.Cells[rowIndex, 2].Value = rowData.Count > 1 ? TryParseDouble(rowData[1]) : 0;
                        worksheet.Cells[rowIndex, 3].Value = rowData.Count > 2 ? TryParseDouble(rowData[2]) : 0;
                        worksheet.Cells[rowIndex, 4].Value = rowData.Count > 3 ? TryParseDouble(rowData[3]) : 0;
                        worksheet.Cells[rowIndex, 5].Value = rowData.Count > 4 ? TryParseDouble(rowData[4]) : 0;
                        worksheet.Cells[rowIndex, 6].Value = rowData.Count > 5 ? TryParseDouble(rowData[5]) : 0;

                        rowIndex++;
                    }

                    // Add a blank row for spacing between categories
                    rowIndex++;
                }

                // Add a chart to the worksheet
                var chart = worksheet.Drawings.AddChart("SurveyDataChart", eChartType.BarClustered);
                var chartRange = worksheet.Cells[2, 2, rowIndex - 1, 6]; // Select the range for chart data
                chart.SetPosition(5, 0, 7, 0);  // Set position of the chart
                chart.SetSize(600, 400);  // Set chart size
                chart.Series.Add(chartRange, worksheet.Cells[2, 1, rowIndex - 1, 1]);

                // Save the Excel file to a MemoryStream
                using (var stream = new MemoryStream())
                {
                    package.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SurveyDataCharts.xlsx");
                }
            }
        }

        // Method to safely parse the value to a double
        private double TryParseDouble(object value)
        {
            if (value != null && double.TryParse(value.ToString(), out double result))
            {
                return result;
            }
            return 0; // Default value if parsing fails
        }


    }
}
