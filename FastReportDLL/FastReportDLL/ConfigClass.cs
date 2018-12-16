using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace FastReportDLL
{
    public class ConfigClass
    {
        private Dictionary<string, string> Dict;

        private void AddDict(string key, string value)
        {
            Dict.Add(key, value);
        }

        public string GetValue(string key)
        {
            var appSettings = Dict[key];
            if (appSettings != null)
                return appSettings;
            else return "";
        }

        public ConfigClass()
        {
            Dict = new Dictionary<string, string>();
            LoadConfig();
        }

        public void LoadConfig()
        {
            string[] list = ConfigurationManager.AppSettings.AllKeys;
            foreach (string key in list)
            {
                AddDict(key, ConfigurationManager.AppSettings[key]);
            }
        }
    }
}
