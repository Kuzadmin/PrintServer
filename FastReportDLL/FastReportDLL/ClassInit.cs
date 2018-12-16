using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastReportDLL
{
    class ClassInit
    {
        private static ClassInit instance;
        private Dictionary<String, String> list_param;
        public String GetParam(String key)
        {
            bool is_value = list_param.ContainsKey(key);
            if (is_value) return list_param[key];
            else return "";
        }

        public static ClassInit getInstance(String path)
        {
            if (instance == null)
            {
                if(String.IsNullOrEmpty(path)) return instance;
                instance = new ClassInit(path);
            }
            return instance;
        }

        private ClassInit(String path)
        {
            list_param = new Dictionary<string, string>();
            try
            {
                FileStream file = new FileStream(path,FileMode.Open, FileAccess.Read);
                file.Close();
            }
            catch(FileNotFoundException)
            {

            }
        }
    }
}
