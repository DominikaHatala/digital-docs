using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace digital_docs_wpf
{
    /// <summary>
    /// Interaction logic for ExcelDialog.xaml
    /// </summary>
    public partial class ExcelDialog : Window
    {
        public ExcelDialog()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            var ordersCount = Int32.Parse(offerNumberTextbox.Text);

            using (ExcelPackage excel = new ExcelPackage())
            {
                excel.Workbook.Worksheets.Add("Order");

                FileInfo excelFile = new FileInfo(@"order.xlsx");

                List<string[]> headerRow = new List<string[]>()
                {
                  new string[] { "TYPE", "DETAIL", "AMOUNT", "STATUS" }
                };

                string headerRange = "B1:E1";

                var worksheet = excel.Workbook.Worksheets["Order"];
                worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                worksheet.Cells[headerRange].Style.Font.Bold = true;
                worksheet.Cells[headerRange].Style.Font.Size = 14;
                worksheet.Cells[headerRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.Black);

                string orderRange = "A2:A" + (ordersCount + 1);
                var orderStringArray = new List<string>();
                for (int i = 1; i <= ordersCount; i++) orderStringArray.Add("Order " + i);

                for (int i = 0; i < ordersCount; i++)
                {
                    var cellAdress = "A" + (i + 2);
                    worksheet.Cells[cellAdress].Value = orderStringArray[i];
                    worksheet.Cells[cellAdress].Style.Font.Bold = true;
                    worksheet.Cells[cellAdress].Style.Font.Size = 12;
                    worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.RoyalBlue);

                }

                worksheet.Cells[headerRange].AutoFitColumns();
                excel.SaveAs(excelFile);
            }
        }
    }
}
