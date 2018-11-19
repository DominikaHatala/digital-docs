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
            FileInfo excelFile = new FileInfo(@"order.xlsx");

            var productCountList = new List<int>();
            var productCountStringList = productListTextBox.Text.Split(',');

            foreach (var pCount in productCountStringList) productCountList.Add(Int32.Parse(pCount.Trim()));


            using (ExcelPackage excel = new ExcelPackage())
            {
                for (int z = 0; z < ordersCount; z++)
                {
                    excel.Workbook.Worksheets.Add("Order " + z);

                    List<string[]> headerRow = new List<string[]>()
                {
                  new string[] { "TYPE", "DETAIL", "AMOUNT", "STATUS" }
                };

                    string headerRange = "B1:E1";

                    var worksheet = excel.Workbook.Worksheets["Order " + z];
                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                    worksheet.Cells[headerRange].Style.Font.Bold = true;
                    worksheet.Cells[headerRange].Style.Font.Size = 14;
                    worksheet.Cells[headerRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    string orderRange = "A2:A" + (productCountList[z] + 1);
                    var orderStringArray = new List<string>();
                    for (int i = 1; i <= productCountList[z]; i++) orderStringArray.Add("Product " + i);

                    for (int i = 0; i < productCountList[z]; i++)
                    {
                        var cellAdress = "A" + (i + 2);
                        worksheet.Cells[cellAdress].Value = orderStringArray[i];
                        worksheet.Cells[cellAdress].Style.Font.Bold = true;
                        worksheet.Cells[cellAdress].Style.Font.Size = 12;
                        worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.RoyalBlue);

                    }

                    worksheet.Cells[headerRange].AutoFitColumns();

                    List<string[]> infoheaderRow = new List<string[]>()
                {
                  new string[] { "Order Number", "Status", "Client ID", "Client Name", "Client Surname"}
                };

                    headerRange = "G1:K1";
                    worksheet.Cells[headerRange].LoadFromArrays(infoheaderRow);
                    worksheet.Cells[headerRange].AutoFitColumns();

                    worksheet.Cells[headerRange].Style.Font.Bold = true;
                    worksheet.Cells[headerRange].Style.Font.Size = 12;
                    worksheet.Cells[headerRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.MediumVioletRed);

                }
                excel.SaveAs(excelFile);

                bool isExcelInstalled = Type.GetTypeFromProgID("Excel.Application") != null ? true : false;
                if (isExcelInstalled)
                {
                    var process = System.Diagnostics.Process.Start(excelFile.ToString());
                    process.WaitForExit();
                    ExcelManager.ExcelToXML(@"order.xlsx");
                }

                this.Close();
            }
        }
    }
}
