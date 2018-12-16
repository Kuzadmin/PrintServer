using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastReportDLL
{
    class MessageList
    {
        private List<MessageLog> mes_list;
        public MessageList()
        {
            mes_list = new List<MessageLog>();
        }

        public void Add_Message(MessageLog mes_log)
        {
            mes_list.Add(mes_log);
        }

        public String Get_MessageLog(int id)
        {
            return mes_list[id].Read();
        }

        public MessageLog Get_MessageLogID(int id)
        {
            return mes_list[id];
        }

        public int Count()
        {
            return mes_list.Count;
        }

    }

    class MessageLog
    {
        private DateTime dt;
        private String type;
        private String operation;
        private String mess;

        public MessageLog(String _type, String _operation, String _mess)
        {
            dt = DateTime.Now;
            type = _type;
            operation = _operation;
            mess = _mess;
        }

        public String Read()
        {
            return dt.ToString("yyyy.MM.dd HH:mm:ss.fff") + " " + type + " " + operation + " " + mess + "\r\n";       
        }

        public static string INFO = "INFO";
        public static string ERROR = "ERROR";
    }
}
