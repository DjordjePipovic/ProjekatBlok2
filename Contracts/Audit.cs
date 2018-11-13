using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class Audit
    {
        EventLog newLog;

        public Audit()
        {
            if (!EventLog.SourceExists("ProjekatBlok2"))
            {
                EventLog.CreateEventSource("ProjekatBlok2", "Application");
            }
            newLog = new EventLog("Application", Environment.MachineName, "ProjekatBlok2");
        }

        public void LogEventAudit(MyEvent mye, int id)
        {
            newLog.WriteEntry(mye.message, EventLogEntryType.Information, id, 0);
        }
    }
}
