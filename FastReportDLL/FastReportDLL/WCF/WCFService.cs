using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FastReportDLL.WCF
{
    public class WCFService : IForThread
    {
        private ServiceHost serviceHost;
        private bool stopper;
        private bool is_start;
        private int TimeOutLogEmpty;

        public WCFService(ConfigClass conf)
        {
            TimeOutLogEmpty = 5000;
            is_start = false;
            stopper = false;
            Initializer(conf);
            serviceHost.Open();
            is_start = true;
        }

        private void Initializer(ConfigClass conf)
        {
            TimeOutLogEmpty = Convert.ToInt32(conf.GetValue("TimeOutLogEmpty"));
            String addr = "http://"+ conf.GetValue("ServiceHost") + ":"+ conf.GetValue("ServicePort") + "/" + conf.GetValue("ServiceAddress");
            Uri uri = new Uri(addr);
            var rt = new FastReportDLL.WCF.FastReportWCF(conf);
            serviceHost = new ServiceHost(rt, uri);
            //serviceHost = new ServiceHost(typeof(FastReportDLL.WCF.FastReportWCF), uri);
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.MessageEncoding = WSMessageEncoding.Mtom;
            TimeSpan times = new TimeSpan(0, 5, 0);
            binding.CloseTimeout = times;
            binding.OpenTimeout = times;
            binding.ReceiveTimeout = times;
            binding.SendTimeout = times;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;// 2147483647;
            serviceHost.CloseTimeout = times;
            serviceHost.OpenTimeout = times;

            serviceHost.AddServiceEndpoint(typeof(FastReportDLL.WCF.IFastService), binding, "");

            ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
            behavior.HttpGetEnabled = true;
            serviceHost.Description.Behaviors.Add(behavior);

            ServiceThrottlingBehavior stb = new ServiceThrottlingBehavior
            {
                MaxConcurrentSessions = Convert.ToInt32(conf.GetValue("MaxConcurrentSessions")),
                MaxConcurrentCalls = Convert.ToInt32(conf.GetValue("MaxConcurrentCalls")),
                MaxConcurrentInstances = Convert.ToInt32(conf.GetValue("MaxConcurrentInstances"))
            };
            serviceHost.Description.Behaviors.Add(stb);

            if (conf.GetValue("ServiceMEX_IS").Equals("Да"))
                serviceHost.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

        }

        public void LOOP()
        {
            if(!stopper)
            {
                Thread.Sleep(TimeOutLogEmpty);
            }
            serviceHost.Close();
        }

        public bool Is_Started()
        {
            return stopper;
        }

        public void Set_Stop(bool stop)
        {
            stopper = stop;
        }
    }
}
