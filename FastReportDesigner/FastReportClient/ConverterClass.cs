using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace FastReportService
{
    static class ConverterClass
    {
        public static List<DataTable> Converter(string file_xml)
        {
            string textXML = System.IO.File.ReadAllText(file_xml);
            XDocument xml = XDocument.Parse(textXML);

            List<DataTable> list = new List<DataTable>();
            foreach (XElement polis in xml.Elements("Polis"))
            {
                int id = 0;
                foreach (XElement node_datatable in polis.Elements("DataTable"))
                {
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    DataTable dt = new DataTable("Data");
                    foreach (XElement node_datatable_f in node_datatable.Elements())
                    {
                        if (node_datatable_f.HasElements)
                        {
                            id++;
                            TableCreate(node_datatable_f, list);
                        }
                        else
                        {
                            TableInsertOrCreate(node_datatable_f, dt, dict);
                        }
                    }
                    DataRow dtr = dt.NewRow();
                    foreach (KeyValuePair<string, string> kvp in dict)
                    {
                        dtr[kvp.Key] = kvp.Value;
                    }
                    dt.Rows.Add(dtr);
                    list.Add(dt);
                }
            }
            return list;
        }

        private static void TableCreate(XElement node, List<DataTable> lst)
        {
            IEnumerable<XElement> s = node.Elements();
            DataTable dt = new DataTable(node.Name.ToString());
            if (s.First().HasElements)
            {
            foreach(XElement xe in s.First().Elements())
                {
                    TableInsertOrCreate(xe, dt, null);
                }
            foreach(XElement xe in node.Elements())
                {
                    DataRow dtr = dt.NewRow();
                    foreach (XElement xe_t in xe.Elements())
                    {
                        dtr[xe_t.Name.ToString()] = xe_t.Value;
                    }
                    dt.Rows.Add(dtr);
                }
            lst.Add(dt);
            }
            else
            {         
            Dictionary<string, string> dict = new Dictionary<string, string>();
            bool first = true;
            foreach (XElement node_datatable_f in node.Elements())
                {
                    if (first)
                    {
                        TableInsertOrCreate(node_datatable_f, dt, null);
                        first = false;
                    }
                    DataRow dtr = dt.NewRow();
                    dtr[node_datatable_f.Name.ToString()] = node_datatable_f.Value;
                    dt.Rows.Add(dtr);
                }
            lst.Add(dt);
            }
        }

        private static void TableInsertOrCreate(XElement node, DataTable tb, Dictionary<string, string> dict)
        {
            DataColumn dtc = new DataColumn(node.Name.ToString());
            dtc.DataType = System.Type.GetType("System.String");
            tb.Columns.Add(dtc);
            if(dict != null)
               dict.Add(node.Name.ToString(), node.Value);
        }
    }
}

               
        /*public static List<DataTable> Converter(string file_xml)
        {
            string textXML = System.IO.File.ReadAllText(file_xml);
            XDocument xml = XDocument.Parse(textXML);

            List<DataTable> list = new List<DataTable>();
            foreach (XElement polis in xml.Elements("Polis"))
            {
                    foreach (XElement node_datatable in polis.Elements("DataTable"))
                    {
                        DataTable dt = new DataTable(node_datatable.FirstNode.ToString());
                        bool init = false;
                        foreach (XElement Row in node_datatable.Elements("Row"))
                        {
                            if (!init)
                            {
                                foreach (XElement Col in Row.Elements("Col"))
                                {
                                    foreach (XAttribute xml_attr in Col.Attributes())
                                    {
                                        DataColumn dtc = new DataColumn(xml_attr.Name.ToString());
                                        dtc.DataType = System.Type.GetType(xml_attr.Value);
                                        dt.Columns.Add(dtc);
                                    }
                                }
                                init = true;
                            }

                            DataRow dtr = dt.NewRow();

                            foreach (XElement Col in Row.Elements("Col"))
                            {
                                foreach (XAttribute xml_attr in Col.Attributes())
                                {
                                    if (xml_attr.Value == "System.DateTime")
                                        dtr[xml_attr.Name.ToString()] = DateTime.Parse(Col.Value);
                                    else if (xml_attr.Value == "System.Int32")
                                        dtr[xml_attr.Name.ToString()] = Int32.Parse(Col.Value);
                                    else if (xml_attr.Value == "System.Int64")
                                        dtr[xml_attr.Name.ToString()] = Int64.Parse(Col.Value);
                                    else if (xml_attr.Value == "System.Double")
                                        dtr[xml_attr.Name.ToString()] = Double.Parse(Col.Value);
                                    else if (xml_attr.Value == "System.Boolean")
                                        dtr[xml_attr.Name.ToString()] = Boolean.Parse(Col.Value);
                                    else if (xml_attr.Value == "System.TimeSpan")
                                        dtr[xml_attr.Name.ToString()] = TimeSpan.Parse(Col.Value);
                                    else if (xml_attr.Value == "System.Decimal")
                                        dtr[xml_attr.Name.ToString()] = Decimal.Parse(Col.Value);
                                    else if (xml_attr.Value == "System.Byte[]")
                                    {
                                        dtr[xml_attr.Name.ToString()] = Convert.FromBase64String(Col.Value);
                                        //byte[] yyy = Convert.FromBase64String(Col.Value);
                                        //FileStream sd = new FileStream("666.jpg", FileMode.Create, FileAccess.Write);
                                        //sd.Write(yyy, 0, yyy.Length);
                                    }
                                    else if (xml_attr.Value == "System.String")
                                        dtr[xml_attr.Name.ToString()] = Col.Value;
                                    else
                                        dtr[xml_attr.Name.ToString()] = Col.Value;
                                }
                            }
                            dt.Rows.Add(dtr);
                        }
                    list.Add(dt);
                }
            }
            return list;
        }*/
