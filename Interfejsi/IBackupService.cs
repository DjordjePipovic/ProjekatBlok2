using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contracts;

namespace Interfejsi
{
    [ServiceContract]
    public interface IBackupService
    {
        [OperationContract]
        void TestCommunication();

        [OperationContract]
        void BackupLog(Dictionary<int, MyEvent> dic);

        [OperationContract]
        Dictionary<int, MyEvent> LoadLog();
    }
}
