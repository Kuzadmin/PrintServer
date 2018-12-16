using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using FastReportDLL;
using System.Configuration;

namespace FastReportService
{
    public partial class ServiceFR : ServiceBase
    {

        public ServiceFR()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }

        public void Start()
        {
            this.OnStart(null);
        }

        public void Stop()
        {
            this.OnStop();
        }

        protected override void OnStart(string[] args)
        {
            System.Diagnostics.Debugger.Launch();
            string path;
            string[] list = ConfigurationManager.AppSettings.GetValues("PathService");
            path = list[0];
            string[] dllFileNames = Directory.GetFiles(path, "*.dll");
            
            foreach (string fi in dllFileNames)
            {
                if (fi.Contains("FastReportDLL.dll"))
                {
                    path = fi;
                    break;
                }
            }
            Assembly a = Assembly.LoadFrom(path);
            Type[] list_type = a.GetTypes();
            //Type[] list_empty = Type.EmptyTypes;
            Type ty = null;

            foreach (Type temp in list_type)
            {
                if (temp.Name == "MainClass")
                {
                    ty = temp;
                    object item = Activator.CreateInstance(ty);
                    MethodInfo mi = ty.GetMethod("Curcle");                   
                    //Thread loggerThread = new Thread(new ThreadStart());
                    mi.Invoke(item, null);
                    break;
                }
            }



            /* String dll = "C:\\Users\\IIKuznetsov\\Documents\\Visual Studio 2015\\Projects\\FR\\FastReportDLL\\FastReportService\\bin\\Debug\\FastReportDLL.dll";
             Assembly a = Assembly.Load(dll);
             Object o = a.CreateInstance("vscode");
             Type t = a.GetType("Main");
             Object[] numbers = new Object[2];
             MethodInfo mi = t.GetMethod("Start");
             mi.Invoke(o, null);*/
        }

        protected override void OnStop()
        {
        }
    }
}
