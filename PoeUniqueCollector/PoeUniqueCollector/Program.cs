using System;
using System.Threading;

namespace PoeUniqueCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            var api = new ApiRequester(30);
            var thread = new Thread(api.Run);
            
            thread.Start();

            string input;
            while((input = Console.ReadLine()) != "exit")
            {
                Console.WriteLine("USER INPUT: " + input);
                if (input == "compare") Helper.CompareDataLists();
                if (input == "sync") Helper.CompareDataLists(true);
            }
        }
    }
}
