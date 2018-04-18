using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PoeUniqueCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            var api = new APIRequester(15);
            var thread = new Thread(new ThreadStart(api.Run));
            
            thread.Start();

            string input;
            while((input = Console.ReadLine()) != "exit")
            {
                Console.WriteLine("USER INPUT: " + input);
                if (input == "compare") Helper.CompareDataLists();
            }
        }
    }
}
