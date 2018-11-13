using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Interfejsi
{
    [ServiceContract]
    public interface IConfigPP
    {
        [OperationContract]
        void ChangeConfig();
    }
}
