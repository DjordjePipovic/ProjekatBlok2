using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using System.ServiceModel;
using Interfejsi;
using System.Threading;
using System.Security.Principal;

namespace Consumer
{
    public class ConsumerClient : ChannelFactory<ISyslogService>, ISyslogService, IDisposable
    {
        ISyslogService factory;        

        public ConsumerClient(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        #region Read()

        public string Read()
        {
            try
            {
                return factory.Read();                
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to ExecuteCommand(). {0}", e.Message);
            }

            return "";
        }

        #endregion

        #region Update()

        public void Update(int id, string newMsg)
        {
            try
            {
                factory.Update(id, newMsg);
                Console.WriteLine("Dogadjaj[{0}] uspjesno update-ovan.", id);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to ExecuteCommand(). {0}", e.Message);
            }
        }

        #endregion

        #region Delete()

        public void Delete(int id)
        {
            try
            {
                factory.Delete(id);
                Console.WriteLine("Dogadjaj[{0}] uspjesno obrisan.", id);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to ExecuteCommand(). {0}", e.Message);
            }
        }

        #endregion

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }

        
    }
}
