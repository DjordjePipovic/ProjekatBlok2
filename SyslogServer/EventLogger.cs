using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Interfejsi;

namespace SyslogServer
{
    public class EventLogger : IEventLogger
    {
        Audit a = new Audit();

        public void LogEvent(string msg)
        {
            MyEvent mye = new MyEvent("Kriticnost", DateTime.Now, "izvor", msg, EnumState.Closed);

            int eventID = GetId();
            Program.eventi.Add(eventID, mye);

            a.LogEventAudit(mye, eventID);

            Console.WriteLine("EventLogger/LogEvent()");
        }

        public int GetId()
        {
            int i = 0;

            while(i <= Program.eventi.Count)
            {
                if (!Program.eventi.ContainsKey(i))
                {
                    return i;
                }

                i++;
            }

            return i;
        }

        public void TestCommunication()
        {
            Console.WriteLine("Komunikacija sa EventLogger-om je uspostavljena.");
        }
    }
}
