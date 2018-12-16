using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplicationTest
{
    class Program
    {
        object item;

        static void Main(string[] args)
        {
            Thread newThread = new Thread(new ParameterizedThreadStart(Curcle));           
            newThread.Start("");
            bool i = true;
            while (i)
            {
                Thread.Sleep(300000);
            }
            //Object[] numbers = new Object[2];
        }

        static public void  Curcle(object _path)
        {
            string path = (string)_path;
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
                    mi.Invoke(item, null);
                    break;
                }
            }
            bool i = true;
            while (i)
            {
                Thread.Sleep(30000);
            }
        }
    }
}
