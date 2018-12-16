using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FastReportDLL
{
    class FolderProduct : IForThread
    {
        static object blocked;
        private bool is_start;
        private Dictionary<String, String> last_folder;
        private bool stopper = false;
        private int time_out;
        private String path;

        public FolderProduct(int _time_out, String _path)
        {
            blocked = new object();
            is_start = false;
            
            path = _path + "\\Report";
            last_folder = new Dictionary<String, String>();
            time_out = _time_out;
            UpdateList();
            is_start = true;
        }

        private void UpdateList()
        {
            Dictionary<String, String> temp = new Dictionary<String, String>();

            string[] products = Directory.GetDirectories(path);     
            foreach(String prod in products)
            {
                int len_prod = prod.Length;
                string[] versions = Directory.GetDirectories(prod);
                string folder_max = null;
                int folder_num = 0;
                foreach (String version in versions)
                {               
                    int len_ver = version.Length;
                    string folder = version.Substring(len_prod + 1, len_ver - len_prod - 1);
                    int folder_curr = Convert.ToInt32(folder);
                    if (folder_curr > folder_num)
                    {
                        folder_max = folder;
                    }
                }
                temp.Add(prod, folder_max);
            }
            lock(blocked)
            {
                if (last_folder.Count > 0) ClearLastFolder();
                last_folder = temp;
            }

            
        }

        public void ClearLastFolder()
        {
            last_folder.Clear();
        }

        public void AddLastFolder(String product, String folder)
        {
            last_folder.Add(product, folder);
        }

        public String GetFolderForProduct(String product)
        {
            lock (blocked)
            {
                return last_folder[product];
            }
        }

        public void LOOP()
        {
            while (!stopper)
            {
                UpdateList();
                Thread.Sleep(time_out);
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
