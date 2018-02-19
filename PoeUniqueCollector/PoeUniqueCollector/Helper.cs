using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeUniqueCollector
{
    public static class Helper
    {
        public static void CompareDataLists()
        {
            var fbFilePath = "..\\..\\BasetypeStorage.csv";
            var myFilePath = "..\\..\\UniqueCollection.txt";

            var fbContent = System.IO.File.ReadAllLines(fbFilePath);
            var myContent = System.IO.File.ReadAllLines(myFilePath);

            var myBTL = new List<string>();
            myContent.ToList().ForEach(x => myBTL.Add(x.Split("-->>").First()));
            myBTL.Sort();

            var fbBTL = new List<string>();
            foreach (var line in fbContent)
            {
                var split = line.Split(',');
                var bt = split[1];
                var rarities = split[3];

                if (rarities.Contains("U"))
                {
                    fbBTL.Add(bt);
                }
            }

            foreach (var bt in fbBTL)
            {
                if (!myBTL.Contains(bt))
                {
                    Console.WriteLine("my list doesnt contain " + bt);
                }
            }

            foreach (var bt in myBTL)
            {
                if (bt.Contains("Map")) continue;

                if (!fbBTL.Contains(bt))
                {
                    Console.WriteLine("fb list doesnt contain " + bt);
                }
            }

            
            Console.Read();
        }
    }

    public static class ClassExtensions
    {
        // string.Split() overload for a simple string split
        public static string[] Split(this string s, string keyword)
        {
            return s.Split(new string[] { keyword }, StringSplitOptions.None);
        }
    }
}
