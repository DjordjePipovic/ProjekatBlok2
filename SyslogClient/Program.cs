using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Sertifikati;
using Interfejsi;
using System.Net;
using System.Net.NetworkInformation;
using System.ServiceModel.Description;

namespace SyslogClient
{
    class Program
    {
        public static EventLoggerClient proxy;
        static void Main(string[] args)
        {
            #region SyslogClient

            /// Define the expected service certificate. It is required to establish cmmunication using certificates.
            string srvCertCN = "wcfservice";

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            /// Use CertManager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9001/EventLogger"),
                                      new X509CertificateEndpointIdentity(srvCert));

            EndpointAddress address2 = new EndpointAddress(new Uri("net.tcp://localhost:9002/WhitelistFirewall"),
                                      new X509CertificateEndpointIdentity(srvCert));


            proxy = new EventLoggerClient(binding, address);
            WhitelistFirewallClient proxy2 = new WhitelistFirewallClient(binding, address2);

            proxy.TestCommunication();
            proxy2.TestCommunication();

            #endregion

            #region ConfigPP

            NetTcpBinding binding3 = new NetTcpBinding();
            string address3 = "net.tcp://localhost:9004/ConfigPP";

            ServiceHost host = new ServiceHost(typeof(ConfigPP));
            host.AddServiceEndpoint(typeof(IConfigPP), binding3, address3);

            host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            host.Open();

            #endregion

            #region PreuzimanjeKonekcija

            Dictionary<int, string> dic = new Dictionary<int, string>();
            var ip = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties();
            
            foreach (var tcp in ip.GetActiveTcpListeners())
            {
                if (!dic.ContainsKey(tcp.Port) && (tcp.Address.ToString() == "0.0.0.0" || tcp.Address.ToString() == "127.0.0.1"))
                {
                    dic.Add(tcp.Port, "tcp");                    
                }

            }
            Console.WriteLine("UDP");
            List<IPEndPoint> listaUdp = ip.GetActiveUdpListeners().ToList<IPEndPoint>();
            for (int i = 0; i < 3; i++)
            {
                if (!dic.ContainsKey(listaUdp[i].Port) && (listaUdp[i].Address.ToString() == "0.0.0.0" || listaUdp[i].Address.ToString() == "127.0.0.1"))
                {
                    dic.Add(listaUdp[i].Port, "udp");
                }
            }

            #endregion

            Console.WriteLine("CheckPP svih aktivnih konekcija.");

            string msg;
            foreach (var v in dic)
            {
                msg = proxy2.CheckPP(v.Key, v.Value);
                if (msg != "")
                {
                    proxy.LogEvent(msg);
                }
            }

            Console.ReadLine();

            host.Close();
        }
    }
}
