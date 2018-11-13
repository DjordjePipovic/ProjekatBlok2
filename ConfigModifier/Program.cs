using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ConfigModifier
{
    public class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9004/ConfigPP";
            ConfigClient client = new ConfigClient(binding, new EndpointAddress(new Uri(address)));

            client.ChangeConfig();

            Console.WriteLine("Konfiguracija izmjenjena...");
            Console.ReadLine();
        }
    }
}
