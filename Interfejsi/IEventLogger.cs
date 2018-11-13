using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Interfejsi
{
    [ServiceContract]
    public interface IEventLogger
    {
        [OperationContract]
        void LogEvent(string msg);

        [OperationContract]
        void TestCommunication();
    }
}
