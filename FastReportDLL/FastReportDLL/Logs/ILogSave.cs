using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastReportDLL.Logs
{
    interface ILogSave
    {
        void Create();
        void Open();
        void Close();
        void Write(MessageLog mes_log);
    }
}
