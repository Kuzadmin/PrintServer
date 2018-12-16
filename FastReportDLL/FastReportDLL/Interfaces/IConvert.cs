using FastReportDLL.WCF;
using FastReportService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FastReportDLL.Interfaces
{
    interface IConvert
    {
        List<DataTable> Converter(XDocument xml, out FRDataForm fr);
    }
}
