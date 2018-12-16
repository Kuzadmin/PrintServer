using FastReportDLL.Logs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FastReportDLL
{
    class WriterLog : ILogSave, IForThread
    {
        private bool is_start;
        private int time_out; 
        private FileStream file;
        private String path;
        private static WriterLog instance;
        private bool stopper;

        public static WriterLog getInstance(String _path, int _time_out)
        {
            if (instance == null)
            {
                instance = new WriterLog(_path, _time_out);
            }
            return instance;
        }


        WriterLog(String _path, int _time_out)
        {
            is_start = false;
            if(_path != null)
            {

            }
            String date = DateTime.Now.ToString("yyyy-MM-dd");
            path = _path + "\\" + date+".log";
            //path = _path;
            time_out = _time_out;
            Create();
            Open();
            stopper = false;
            is_start = true;
        }


        public void Close()
        {
            if (file != null)
            {
                byte[] text = Encoding.UTF8.GetBytes("Файл закрыт\r\n");
                file.Write(text, 0, text.Length);
                file.Close();
            }
            file = null;
        }

        public void Create()
        {
            if (!File.Exists(path))
            {
                file = File.Create(path);
                file.Close();
            }
        }

        public void LOOP()
        {
           Logger log = Logger.getInstance();
           while (!stopper)
            {
                if (!log.Is_Empty())
                {
                    Write(log.Read());
                }
                else
                {
                    Close();
                    Thread.Sleep(time_out);
                    Open();
                }

            }
           while (!log.Is_Empty())
            {
                Write(log.Read());
            }
           Close();
        }

        public void Open()
        {
            file = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Write);
            byte[] text = Encoding.UTF8.GetBytes("Файл открыт\r\n");
            file.Write(text, 0, text.Length);
        }

        public void Write(MessageLog mes_log)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(mes_log.Read());
            file.Write(bytes, 0, bytes.Length);
        }

        public void Write(MessageList log_list)
        {
            for(int i=0; i < log_list.Count(); i++)
            {
                Write(log_list.Get_MessageLogID(i));
            }
        }

        public void Set_Stop(bool stop)
        {
            stopper = stop;
        }

        public bool Is_Started()
        {
            return is_start;
        }
    }
}
