using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Consumer
{
    public class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9000/SyslogService";
            ConsumerClient client= new ConsumerClient(binding, new EndpointAddress(new Uri(address)));
            
            int op = -1;
            do
            {
                Console.WriteLine("Odaberite operaciju: ");
                Console.WriteLine("1. Read()");
                Console.WriteLine("2. Update()");
                Console.WriteLine("3. Delete()");
                Console.WriteLine("0. Exit");

                Int32.TryParse(Console.ReadLine(), out op);

                if(op == 1)
                {
                    Console.WriteLine(client.Read());
                }
                else if(op == 2)
                {
                    Console.WriteLine("Unesite ID dogadjaja koji zelite da izmjenite: ");
                    int id = -1;
                    Int32.TryParse(Console.ReadLine(), out id);

                    Console.WriteLine("Unesite novu poruku: ");
                    string newMsg = Console.ReadLine();

                    client.Update(id, newMsg);
                }
                else if(op == 3)
                {
                    Console.WriteLine("Unesite ID dogadjaja koji zelite da obrisete: ");
                    int id = -1;
                    Int32.TryParse(Console.ReadLine(), out id);

                    client.Delete(id);
                }
            } while (op != 0);

            Console.ReadLine();
        }
        
    }
}
