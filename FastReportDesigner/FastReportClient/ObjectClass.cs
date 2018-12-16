using FastReport;
using FastReportService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastReportDesigner
{
    public class ObjectClass
    {
        private string file_template;
        private string file_xml;
        private Report report;

        public ObjectClass(string _file_template, string _file_xml)
        {
            file_template = _file_template;
            file_xml = _file_xml;
            report = new Report();
            {
            if (!String.IsNullOrEmpty(file_template))
               {
               report.Load(file_template);
               }
            if (!String.IsNullOrEmpty(file_xml))
               {
               List<DataTable> list = ConverterClass.Converter(file_xml);
               foreach (DataTable st in list)
                  {
                        report.RegisterData(st, st.TableName.Trim());
                  }
               }
            }
        }

       
       public void OpenDoc()
        {
            report.Design();
        }

    }
}
