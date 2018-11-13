using Contracts;
using Interfejsi;
using Sertifikati;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace SyslogServer
{
    public class Program
    {
        public static BackupServerProxy proxy;
        public static Dictionary<int, MyEvent> eventi = new Dictionary<int, MyEvent>();

        static void Main(string[] args)
        {            
            #region SyslogService

            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9000/SyslogService";

            ServiceHost host = new ServiceHost(typeof(SyslogService));
            host.AddServiceEndpoint(typeof(ISyslogService), binding, address);
            host.Authorization.ServiceAuthorizationManager = new MyServiceAuthorizationManager();

            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new CustomAuthorizationPolicy());
            host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();

            host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            #endregion

            /// srvCertCN.SubjectName should be set to the service's username. .NET WindowsIdentity class provides information about Windows user running the given process
            string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            NetTcpBinding binding2 = new NetTcpBinding();
            binding2.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            #region EventLogger  

            string address2 = "net.tcp://localhost:9001/EventLogger";

            ServiceHost host2 = new ServiceHost(typeof(EventLogger));
            host2.AddServiceEndpoint(typeof(IEventLogger), binding2, address2);
             
            ///If CA doesn't have a CRL associated, WCF blocks every client because it cannot be validated
            host2.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            host2.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            ///Set appropriate service's certificate on the host. Use CertManager class to obtain the certificate based on the "srvCertCN"
            host2.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);

            host2.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            host2.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            #endregion

            #region WhitelistFirewall

            WhitelistFirewall wlf = new WhitelistFirewall();

            // string s = binding.GetType().ToString();
            //Console.WriteLine(s);

            string address3 = "net.tcp://localhost:9002/WhitelistFirewall";

            ServiceHost host3 = new ServiceHost(typeof(WhitelistFirewall));
            host3.AddServiceEndpoint(typeof(IWhitelistFirewall), binding2, address3);
            
            ///If CA doesn't have a CRL associated, WCF blocks every client because it cannot be validated
            host3.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            host3.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            ///Set appropriate service's certificate on the host. Use CertManager class to obtain the certificate based on the "srvCertCN"
            host3.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);

            host3.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            host3.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            #endregion

            #region PodizanjeHostova

            host.Open();

            try
            {
                host2.Open();
                Console.WriteLine("Syslog/EventLogger is started.");
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                Console.WriteLine("[StackTrace] {0}", e.StackTrace);
            }

            try
            {
                host3.Open();
                Console.WriteLine("Syslog/WhitelistFirewall is started.");

            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                Console.WriteLine("[StackTrace] {0}", e.StackTrace);
            }

            Console.WriteLine("SyslogService is opened. Press <enter> to finish...");

            #endregion
            
            #region BackupService

            NetTcpBinding binding3 = new NetTcpBinding();
            
            binding3.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            /// Use CertManager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, "wcfbackup");
            EndpointAddress address4 = new EndpointAddress(new Uri("net.tcp://localhost:9003/BackupService"),
                                      new X509CertificateEndpointIdentity(srvCert));

            proxy = new BackupServerProxy(binding3, address4);
            proxy.TestCommunication();
            eventi = proxy.LoadLog();

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(60);

            var timer = new System.Threading.Timer((e) =>
            {
                proxy.BackupLog(eventi);
            }, null, startTimeSpan, periodTimeSpan);

            #endregion

            Console.ReadLine();

            host.Close();
            host2.Close();
            host3.Close();
        }        
    }
}
