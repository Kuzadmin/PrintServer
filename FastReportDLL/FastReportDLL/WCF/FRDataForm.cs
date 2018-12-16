using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastReportDLL.WCF
{
    public class FRDataForm
    {
        public string product { get; set; }
        public string form    { get; set; }
        public string version { get; set; }
        public string format  { get; set; }

        public FRDataForm()
           {
            product = "";
            form = "";
            version = "";
            format = "PDF";
           }
    }
}
