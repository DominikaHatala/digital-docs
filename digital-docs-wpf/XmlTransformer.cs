using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace digital_docs_wpf
{
    class Employee
    {
        public String name;
        public String[] products;
    }

    /*
    <orders>
        <order>
            <orderNumber>orderNumber</orderNumber>
                <status>status</status>
                <client>
                    <id>id</id>
                    <name>name</name>
                    <surname>surname</surname>
                </client>
            <product>
                 <type>type</type>
                 <detail>detail</detail>
                 <amount>amount</amount>
                 <status>status</status>
            </product>
            <product>...</product>...
        </order>
    </orders>
    */

    public class XmlTransformer
    {
        public XmlTransformer()
        {
            String path1 = "..\\..\\xmls_result\\divided_xml_employee_1.xml";
            String path2 = "..\\..\\xmls_result\\divided_xml_employee_2.xml";

            String[] paths = { path1, path2};

            //MergeXmls(paths);

            //DivideOrders(path1);
        }

        public static void MergeXmls(String[] xmlPaths)
        {
            IEnumerable<IGrouping<string, XmlNode>> orders = GetOrders(xmlPaths);
            SaveMergedOrders(orders);
        }

        static IEnumerable<IGrouping<string, XmlNode>> GetOrders(String[] xmlPaths)
        {
            XmlDocument[] xmlDocs = new XmlDocument[xmlPaths.Length];
            List<XmlNode> orders = new List<XmlNode>();
            for (int i = 0; i < xmlPaths.Length; i++)
            {
                xmlDocs[i] = new XmlDocument();
                xmlDocs[i].Load(xmlPaths[i]);

                XmlReader reader = new XmlNodeReader(xmlDocs[i]);
                DataSet newDs = new DataSet();
                newDs.ReadXml(reader);
                DataTable dt = newDs.Tables[0];
                DataRelationCollection children = dt.ChildRelations;


                foreach (XmlNode xmlNode in xmlDocs[i].DocumentElement.ChildNodes)
                {
                    orders.Add(xmlNode);
                }

            }

            IEnumerable<IGrouping<string, XmlNode>> query = from order in orders
                                                            group order by order.FirstChild.InnerText;

            return query;
        }

        static void SaveMergedOrders(IEnumerable<IGrouping<string, XmlNode>> orders)
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlNode rootNode = xmlDoc.CreateElement("orders");
            xmlDoc.AppendChild(rootNode);

            foreach (var order in orders)
            {
                XmlNode orderNode = xmlDoc.CreateElement("order");
                rootNode.AppendChild(orderNode);

                String status = "";
                String clientId = "";
                String clientName = "";
                String clientSurname = "";
                foreach (XmlNode i in order)
                {
                    status = i.ChildNodes[1].InnerText;
                    clientId = i.ChildNodes[2].ChildNodes[0].InnerText;
                    clientName = i.ChildNodes[2].ChildNodes[1].InnerText;
                    clientSurname = i.ChildNodes[2].ChildNodes[2].InnerText;
                }

                AddOrderInformation(xmlDoc, orderNode, order.Key, status, clientId, clientName, clientSurname);
                
                foreach (XmlNode i in order)
                {
                    for (int j = 3; j < i.ChildNodes.Count; j++)
                    {
                        AddProduct(xmlDoc, orderNode, i.ChildNodes[j]);

                    }
                }

            }

            xmlDoc.Save("..\\..\\xmls_result\\merged_xml.xml");
        }

        public static void DivideOrders(String xmlPath)
        {
            int numberOfDividedFiles = 0;

            Employee[] employees = new Employee[2];
            employees[0] = new Employee();
            employees[0].name = "employee1";
            employees[0].products = new string[] { "1", "3" };
            employees[1] = new Employee();
            employees[1].name = "employee1";
            employees[1].products = new string[] { "2" };

            List<XmlNode> products = new List<XmlNode>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);

            XmlReader reader = new XmlNodeReader(xmlDoc);
            DataSet newDs = new DataSet();
            newDs.ReadXml(reader);
            DataTable dt = newDs.Tables[0];
            DataRelationCollection children = dt.ChildRelations;

            for (int i = 0; i < xmlDoc.DocumentElement.ChildNodes[0].ChildNodes.Count; i++)
            {
                String x = xmlDoc.DocumentElement.ChildNodes[0].ChildNodes[i].Name;
                if (xmlDoc.DocumentElement.ChildNodes[0].ChildNodes[i].Name == "product")
                {
                    products.Add(xmlDoc.DocumentElement.ChildNodes[0].ChildNodes[i]);
                }
            }

            for (int i = 0; i < employees.Length; i++)
            {
                XmlDocument xmlDocdest = new XmlDocument();

                XmlNode rootNode = xmlDocdest.CreateElement("orders");
                xmlDocdest.AppendChild(rootNode);

                XmlNode orderNode = xmlDocdest.CreateElement("order");
                rootNode.AppendChild(orderNode);

                AddOrderInformation(xmlDocdest,
                    orderNode,
                    xmlDoc.DocumentElement.ChildNodes[0].ChildNodes[0].InnerText,
                    xmlDoc.DocumentElement.ChildNodes[0].ChildNodes[1].InnerText,
                    xmlDoc.DocumentElement.ChildNodes[0].ChildNodes[2].ChildNodes[0].InnerText,
                    xmlDoc.DocumentElement.ChildNodes[0].ChildNodes[2].ChildNodes[1].InnerText,
                    xmlDoc.DocumentElement.ChildNodes[0].ChildNodes[2].ChildNodes[2].InnerText);

                if(products.Count > 0 )
                {
                    numberOfDividedFiles++;
                }
                foreach (XmlNode product in products)
                {
                    if (employees[i].products.Contains(product.ChildNodes[0].InnerText))
                    {
                        AddProduct(xmlDocdest, orderNode, product);
                    }

                }
                int employeeNumber = i + 2;
                String pathToXml = "..\\..\\xmls_result\\divided_xml_employee_" + employeeNumber + ".xml";
                xmlDocdest.Save(pathToXml);
                //xml to excel
                Mail mail = new Mail();
                mail.send(employeeNumber, pathToXml, 1);
            }
            int result = numberOfDividedFiles;
        }

        public static void AddOrderInformation(XmlDocument xmlDocdest, XmlNode orderNode, String orderNumber, String status, String clientId, String clientName, String clientSurname)
        {
            /*
            <orderNumber>orderNumber</orderNumber>
            <status>status</status>
            <client>
                <id>id</id>
                <name>name</name>
                <surname>surname</surname>
            </client>
            */
            XmlNode orderNumberNode = xmlDocdest.CreateElement("orderNumber");
            orderNumberNode.InnerText = orderNumber;
            orderNode.AppendChild(orderNumberNode);

            XmlNode statusNode = xmlDocdest.CreateElement("status");
            statusNode.InnerText = status;
            orderNode.AppendChild(statusNode);

            XmlNode clientNode = xmlDocdest.CreateElement("client");

            XmlNode clientIdNode = xmlDocdest.CreateElement("id");
            clientIdNode.InnerText = clientId;
            clientNode.AppendChild(clientIdNode);

            XmlNode clientNameNode = xmlDocdest.CreateElement("name");
            clientNameNode.InnerText = clientName;
            clientNode.AppendChild(clientNameNode);

            XmlNode clientSurnameNode = xmlDocdest.CreateElement("surname");
            clientSurnameNode.InnerText = clientSurname;
            clientNode.AppendChild(clientSurnameNode);

            orderNode.AppendChild(clientNode);
        }

        public static void AddProduct(XmlDocument xmlDocdest, XmlNode orderNode, XmlNode product)
        {
            /*
            <product>
                  <type>type</type>
                  <detail>detail</detail>
                  <amount>amount</amount>
                  <status>status</status>
            </product>
            */
            XmlNode productNode = xmlDocdest.CreateElement("product");

            XmlNode typeNode = xmlDocdest.CreateElement("type");
            typeNode.InnerText = product.ChildNodes[0].InnerText;
            productNode.AppendChild(typeNode);

            XmlNode detailNode = xmlDocdest.CreateElement("detail");
            detailNode.InnerText = product.ChildNodes[1].InnerText;
            productNode.AppendChild(detailNode);

            XmlNode amountNode = xmlDocdest.CreateElement("amount");
            amountNode.InnerText = product.ChildNodes[2].InnerText;
            productNode.AppendChild(amountNode);

            XmlNode productStatusNode = xmlDocdest.CreateElement("status");
            productStatusNode.InnerText = product.ChildNodes[3].InnerText;
            productNode.AppendChild(productStatusNode);

            orderNode.AppendChild(productNode);
        }
    }


}
