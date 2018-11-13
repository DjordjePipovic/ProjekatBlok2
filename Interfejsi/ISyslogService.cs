using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Interfejsi
{
    [ServiceContract]
    public interface ISyslogService
    {
        [OperationContract]
        string Read();

        [OperationContract]
        void Update(int id, string newMsg);

        [OperationContract]
        void Delete(int id);
    }
}
