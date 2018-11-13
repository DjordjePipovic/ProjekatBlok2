using Contracts;
using Interfejsi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace BackupServer
{
    public class BackupService : IBackupService
    {
        public void BackupLog(Dictionary<int, MyEvent> dic)
        {
            string s = "";

            foreach(KeyValuePair<int, MyEvent> kvp in dic)
            {
                s += kvp.Key.ToString() + " " + kvp.Value.ToString() + Environment.NewLine;
            }

            File.WriteAllText("BackupServer.txt", s);
            Console.WriteLine("BackupServer/BackupLog()");
        }

        public Dictionary<int, MyEvent> LoadLog()
        {
            Dictionary<int, MyEvent> ret = new Dictionary<int, MyEvent>();
            string[] split = File.ReadAllLines("BackupServer.txt");

            foreach(string s in split)
            {
                ret.Add(konverzija(s).Key, konverzija(s).Value);
            }

            Console.WriteLine("BackupServer/LoadLog()");
            return ret;
        }

        public KeyValuePair<int, MyEvent> konverzija(string s)
        {
            string[] s1 = s.Split(';');

            int id = Int32.Parse(s.Split(' ')[0]);
            string c = s1[0].Split('=')[1];
            DateTime dt = Convert.ToDateTime(s1[1].Split('=')[1]);
            string sc = s1[2].Split('=')[1];
            string m = s1[3].Split('=')[1];
            Enum.TryParse(s1[4].Split('=')[1], out EnumState es);

            MyEvent me = new MyEvent(c, dt, sc, m, es);

            return new KeyValuePair<int, MyEvent>(id, me);
        }

        public void TestCommunication()
        {
            Console.WriteLine("Komunikacija sa Backup Server-om je uspostavljena.");
        }
    }
}
