using OfficeOpenXml;
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
    }
}
