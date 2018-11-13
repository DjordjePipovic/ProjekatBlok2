using Contracts;
using Interfejsi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SyslogServer
{
    public class SyslogService : ISyslogService
    {
        public string Read()
        {
            string ret = "";

            CustomPrincipal cp = new CustomPrincipal(Thread.CurrentPrincipal.Identity as WindowsIdentity);

            ret += "Lista dogadjaja:\n";

            foreach(KeyValuePair<int, MyEvent> kvp in Program.eventi)
            {
                ret += "ID [" + kvp.Key.ToString() + "]" + kvp.Value.Ispis() + "\n";
            }

            return ret;
        }
        
        public void Update(int id, string newMsg)
        {
            CustomPrincipal cp = new CustomPrincipal(Thread.CurrentPrincipal.Identity as WindowsIdentity);

            if (cp.IsInRole("Update"))
            {
                if(!Program.eventi.ContainsKey(id))
                {
                    throw new Exception("Nepostojeci ID.");
                }

                MyEvent temp = Program.eventi[id];
                temp.message = newMsg;

                Program.eventi[id] = temp;
            }
            else
            {
                throw new SecurityException("Access is denied.");
            }
        }
        
        public void Delete(int id)
        {
            CustomPrincipal cp = new CustomPrincipal(Thread.CurrentPrincipal.Identity as WindowsIdentity);

            if (cp.IsInRole("Delete"))
            {
                if (!Program.eventi.ContainsKey(id))
                {
                    throw new Exception("Nepostojeci ID.");
                }

                Program.eventi.Remove(id);
            }
            else
            {
                throw new SecurityException("Access is denied.");
            }
        }
    }
}
