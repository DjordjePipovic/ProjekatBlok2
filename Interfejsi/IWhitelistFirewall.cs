using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Interfejsi
{
    [ServiceContract]
    public interface IWhitelistFirewall
    {
        [OperationContract]
        string CheckPP(int port, string protocol);
        [OperationContract]
        void TestCommunication();
    }
}
