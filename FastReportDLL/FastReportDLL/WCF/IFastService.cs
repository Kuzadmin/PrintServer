using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FastReportDLL.WCF
{
    [ServiceContract]
    interface IFastService
    {
        [OperationContract]  //(Name ="GetReport")
        Stream GetPrintFile(XmlElement xml);
    }
}
