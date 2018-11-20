using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace digital_docs_wpf
{
    public static class ExcelManager
    {
        public static void ExcelToXML(string pathToExcel)
        {
            var excelFile = new FileInfo(pathToExcel);

            var xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("orders");
            xmlDoc.AppendChild(rootNode);


            using (var excel = new ExcelPackage(excelFile))
            {
                foreach (var worksheet in excel.Workbook.Worksheets)
                {
                    var orderNode = xmlDoc.CreateElement("order");
                    rootNode.AppendChild(orderNode);

                    var orderNumber = worksheet.Cells["G2"].Value.ToString();
                    var status = (string)worksheet.Cells["H2"].Value.ToString();
                    var clientId = (string)worksheet.Cells["I2"].Value.ToString();
                    var clientName = (string)worksheet.Cells["J2"].Value.ToString();
                    var clientSurname = (string)worksheet.Cells["K2"].Value.ToString();

                    XmlTransformer.AddOrderInformation(xmlDoc, orderNode, orderNumber, status, clientId, clientName, clientSurname);

                    var orderColumn = "A";
                    var orderIndex = 1;

                    do
                    {
                        orderIndex++;
                        try
                        {
                            var cellValue = worksheet.Cells[orderColumn + orderIndex].Value.ToString();
                        }
                        catch (Exception e)
                        {
                            break;
                        }

                        var type = (string)worksheet.Cells["B" + orderIndex].Value.ToString();
                        var detail = (string)worksheet.Cells["C" + orderIndex].Value.ToString();
                        var amount = (string)worksheet.Cells["D" + orderIndex].Value.ToString();
                        var product_status = (string)worksheet.Cells["E" + orderIndex].ToString();

                        var productNode = xmlDoc.CreateElement("order");

                        var typeNode = xmlDoc.CreateElement("type");
                        typeNode.InnerText = type;

                        var detailNode = xmlDoc.CreateElement("detail");
                        detailNode.InnerText = detail;

                        var amountNode = xmlDoc.CreateElement("type");
                        amountNode.InnerText = amount;

                        var productStatusNode = xmlDoc.CreateElement("type");
                        productStatusNode.InnerText = product_status;

                        productNode.AppendChild(typeNode);
                        productNode.AppendChild(detailNode);
                        productNode.AppendChild(amountNode);
                        productNode.AppendChild(productStatusNode);


                        XmlTransformer.AddProduct(xmlDoc, orderNode, productNode);
                    } while (true);
                }
            }

            String orderPath = "..\\..\\xmls_result\\excel_order.xml";
            xmlDoc.Save(orderPath);
            XmlTransformer.DivideOrders(orderPath);
        }

        public static void XmlToExcel(string pathToXML, string excelDestinationPath)
        {
            var xmlDoc = new XmlDataDocument();
            FileInfo excelFile = new FileInfo(excelDestinationPath);


            var fs = new FileStream(pathToXML, FileMode.Open, FileAccess.Read);
            xmlDoc.Load(fs);

            var root = xmlDoc.FirstChild;

            var orders = root.ChildNodes;

            using (var excel = new ExcelPackage())
            {
                var orderCount = 0;
                foreach (XmlNode order in orders)
                {

                    excel.Workbook.Worksheets.Add("Order " + orderCount);

                    var orderChildren = order.ChildNodes;

                    #region boring_excel_header_voodo
                    List<string[]> headerRow = new List<string[]>()
                    {
                      new string[] { "TYPE", "DETAIL", "AMOUNT", "STATUS" }
                    };

                    string headerRange = "B1:E1";

                    var worksheet = excel.Workbook.Worksheets["Order " + orderCount];
                    worksheet.Cells[headerRange].LoadFromArrays(headerRow);

                    worksheet.Cells[headerRange].Style.Font.Bold = true;
                    worksheet.Cells[headerRange].Style.Font.Size = 14;
                    worksheet.Cells[headerRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[headerRange].Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    List<string[]> infoheaderRow = new List<string[]>()
                {
                  new string[] { "Order Number", "Status", "Client ID", "Client Name", "Client Surname"}
                };

                    var infoheaderRange = "G1:K1";
                    worksheet.Cells[infoheaderRange].LoadFromArrays(infoheaderRow);

                    worksheet.Cells[infoheaderRange].Style.Font.Bold = true;
                    worksheet.Cells[infoheaderRange].Style.Font.Size = 12;
                    worksheet.Cells[infoheaderRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[infoheaderRange].Style.Font.Color.SetColor(System.Drawing.Color.MediumVioletRed);




                    #endregion

                    // General info
                    var orderNumber = orderChildren[0].InnerText;
                    worksheet.Cells["G2"].Value = orderNumber;

                    var status = orderChildren[1].InnerText;
                    worksheet.Cells["H2"].Value = status;

                    // Client info
                    var clientNode = orderChildren[2].ChildNodes;

                    var clientId = clientNode[0].InnerText;
                    worksheet.Cells["I2"].Value = clientId;

                    var clientName = clientNode[1].InnerText;
                    worksheet.Cells["J2"].Value = clientName;

                    var clientSurname = clientNode[2].InnerText;
                    worksheet.Cells["K2"].Value = clientSurname;


                    // Parsing products
                    for (int i = 3; i < orderChildren.Count; i++)
                    {
                        var productChildren = orderChildren[i].ChildNodes;

                        var CellId = (i - 1);
                        worksheet.Cells["A" + CellId].Value = "Product " + (i - 2);

                        
                        var type = productChildren[0].InnerText;
                        worksheet.Cells["B" + CellId].Value = type;

                        var detail = productChildren[1].InnerText;
                        worksheet.Cells["C" + CellId].Value = detail;

                        var amount = productChildren[2].InnerText;
                        worksheet.Cells["D" + CellId].Value = amount;

                        var productStatus = productChildren[3].InnerText;
                        worksheet.Cells["E" + CellId].Value = productStatus;

                    }

                    worksheet.Cells[headerRange].AutoFitColumns();
                    worksheet.Cells[infoheaderRange].AutoFitColumns();

                    orderCount++;
                    excel.SaveAs(excelFile);
                }

            }

        }
    }
}
