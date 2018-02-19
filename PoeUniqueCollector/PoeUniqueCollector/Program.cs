using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeUniqueCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            var api = new APIRequester(-10);
            api.Run();
            
            Console.WriteLine("Finished");
            Console.ReadLine();
        }
    }
}
