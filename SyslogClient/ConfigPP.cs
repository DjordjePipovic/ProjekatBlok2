using Interfejsi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyslogClient
{
    public class ConfigPP : IConfigPP
    {
        public void ChangeConfig()
        {
            List<string> pp = new List<string>();
            string path = @"..\..\..\SyslogServer\bin\debug\WhiteListFireWall.txt";

            pp.Add("1234");
            pp.Add("5678");
            pp.Add("9876");
            pp.Add("4321");
            pp.Add("4545");
            pp.Add("tcp");
            pp.Add("http");

            string upis = "";

            foreach(string s in pp)
            {
                upis += s + Environment.NewLine;
            }

            File.WriteAllText(path, upis);

            Program.proxy.LogEvent("Konfiguracija je izmjenjena");
        }
    }
}
