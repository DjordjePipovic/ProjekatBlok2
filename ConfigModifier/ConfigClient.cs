using Interfejsi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ConfigModifier
{
    public class ConfigClient : ChannelFactory<IConfigPP>, IConfigPP, IDisposable
    {
        IConfigPP factory;

        public ConfigClient(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public void ChangeConfig()
        {
            try
            {
                factory.ChangeConfig();
            }catch(Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
        }
    }
}
