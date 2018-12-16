using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace FastConnect
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.DefaultConnectionLimit = 1000;
            Console.ReadLine();
            for (int i = 0; i < 1; i++)
            {
                //Console.WriteLine("Поток:" + i.ToString() + " Создается");
                Thread myThread = new Thread(new ParameterizedThreadStart(GetZapr));
                myThread.Start(i); // запускаем поток
                Thread.Sleep(0);
            }
            Console.WriteLine("КОНЕЦ");
            Console.ReadLine();
        }

      static void GetZapr(object i)
        {
            DateTime start = DateTime.Now;
            string j = ((int)i).ToString();
            //Console.WriteLine("Поток:" + j + " начал работу");
            //ServiceReference2.IdikbmClient wow = new ServiceReference2.IdikbmClient();// .StockServiceClient(); 
            ServiceReference1.FastServiceClient wow = new ServiceReference1.FastServiceClient();
              //  new ServiceReference1.FastServiceClient();

            String qw = "xml.xml";
            String qw1 = j + ".pdf";
            FileStream file = new FileStream(qw, FileMode.Open, FileAccess.Read);
            XElement ele = XElement.Load(file);
            //String xxx = ele.ToString();
            FileStream files = new FileStream(qw1, FileMode.OpenOrCreate, FileAccess.Write);
            TimeSpan time_sp = new TimeSpan(0, 5, 0);


            wow.Endpoint.Binding.CloseTimeout = time_sp;
            wow.Endpoint.Binding.SendTimeout = time_sp;
            wow.Endpoint.Binding.OpenTimeout = time_sp;
            wow.Endpoint.Binding.ReceiveTimeout = time_sp;
            //wow.Endpoint.Binding.
            //wow.Endpoint.Binding.
            try
            {               
                Stream input = wow.GetPrintFile(ele);
                var memoryStream = new MemoryStream();
                input.CopyTo(memoryStream);
                byte[] myBinary = memoryStream.ToArray();

                //memoryStream.CopyTo(files);
                

                // MemoryStream mem = (MemoryStream)input;
                //byte[] myBinary = new byte[input.Length];
                files.Write(myBinary, 0, myBinary.Length);
               files.Close();
                // paramFile.Read(myBinary, 0, (int)paramFile.Length);

                //Console.WriteLine("Поток:" + j + " ВЫЗОВ КБМ");
                // string input = wow.GetKbmTo(xxx);
                // ServiceReference2.GetKbmToResponse t = await wow.GetKbmToAsync(xxx);
                // string input = t.@return;
                Thread.Sleep(0);
                //Stream // GetPrintFile(ele);

  
                //files.Write(sas, 0, sas.Length);

                //var sti = Encoding.UTF8.GetBytes(input);
                //files.Write(sti, 0, sti.Length);
            }
            catch(Exception e)
            { var sti = Encoding.UTF8.GetBytes(e.Message);

                files.Write(sti, 0, sti.Length);
                Console.WriteLine("Поток:" + j + " ИСКЛЮЧЕНИЕ");
            }
            finally
            {
                //input.CopyTo(files);
                //files.Close();
            }
            DateTime finish = DateTime.Now;
            TimeSpan ts = finish - start;
            Console.WriteLine("Поток:" + j + " отработал секунд:" + (ts.TotalSeconds).ToString());
        }

    }
}
