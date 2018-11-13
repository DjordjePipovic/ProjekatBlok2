using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Contracts
{
    [DataContract]
    public class MyEvent
    {
        [DataMember]
        public string criticality { get; set; }
        [DataMember]
        public DateTime timestamp { get; set; }
        [DataMember]
        public string source { get; set; }
        [DataMember]
        public string message { get; set; }
        [DataMember]
        public EnumState eState { get; set; }
        
        public MyEvent(string c, DateTime t, string s, string m, EnumState es)
        {
            this.criticality = c;
            this.timestamp = t;
            this.source = s;
            this.message = m;
            this.eState = es;
        }

        public override string ToString()
        {
            string ret = "Criticality=" + criticality + ";Timestamp=" + timestamp.ToString() + ";Source=" + source + ";Message=" + message + ";State=" + eState.ToString();

            return ret;
        }

        public string Ispis()
        {
            string ret = criticality + "\t" + timestamp.ToString() + "\t" + source + "\t" + message + "\t" + eState.ToString();

            return ret;
        }
    }
}
