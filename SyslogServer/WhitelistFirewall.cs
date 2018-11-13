using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Interfejsi;

namespace SyslogServer
{
    public class WhitelistFirewall : IWhitelistFirewall
    {
        public List<int> portovi = new List<int>();
        public List<string> protokoli = new List<string>();

        public WhitelistFirewall()
        {
            int port;
            string[] redovi=  File.ReadAllLines("WhiteListFireWall.txt");

            foreach (var r in redovi)
            {
                if (Int32.TryParse(r, out port))
                {
                    portovi.Add(port);
                }
                else
                {
                    protokoli.Add(r);
                }
            }
            
        }
        public string CheckPP(int port, string protocol)
        {
            Console.WriteLine("WhitelistFirewall/CheckPP()");
            string ret = "";

            if(!portovi.Contains(port) && !protokoli.Contains(protocol))
            {
                ret = string.Format("Port i protokol [{0}, {1}] nisu pronadjeni u WhiteList Firewall-u.", port.ToString(), protocol);
                return ret;
            }

            if (!portovi.Contains(port))
            {
                ret = string.Format("Port [{0}] nije pronadjen u WhiteList Firewall-u.", port.ToString());
                return ret;
            }
            else
            {
                if (!protokoli.Contains(protocol))
                {
                    ret = string.Format("Protokol [{0}] nije pronadjen u WhiteList Firewall-u.", protocol.ToUpper());
                    return ret;
                }
            }         

            return ret;
        }

        public void TestCommunication()
        {
            Console.WriteLine("Komunikacija sa Whitelist Firewall-om je uspostavljena.");
        }
    }
}
