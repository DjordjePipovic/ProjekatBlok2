﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Interfejsi;
using Sertifikati;
using System.Security.Principal;
using System.Security.Cryptography.X509Certificates;

namespace SyslogServer
{
    public class BackupServerProxy : ChannelFactory<IBackupService>, IBackupService, IDisposable
    {
        IBackupService factory;

        public BackupServerProxy(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            /// cltCertCN.SubjectName should be set to the client's username. .NET WindowsIdentity class provides information about Windows user running the given process
			string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
            this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            /// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

            factory = this.CreateChannel();
        }

        public void BackupLog(Dictionary<int, MyEvent> dic)
        {
            try
            {
                factory.BackupLog(dic);
                Console.WriteLine("BackupServer/BackupLog()");
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
        }

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }

        public Dictionary<int, MyEvent> LoadLog()
        {
            Dictionary<int, MyEvent> dic = new Dictionary<int, MyEvent>();
            try
            {
                dic = factory.LoadLog();
                Console.WriteLine("BackupServer/LoadLog()");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
            return dic;
        }

        public void TestCommunication()
        {
            try
            {
                factory.TestCommunication();
                Console.WriteLine("Test komunikacija sa BackupServerom.");
            }
            catch (Exception e)
            {
                Console.WriteLine("[TestCommunication] ERROR = {0}", e.Message);
            }
        }
    }
}
