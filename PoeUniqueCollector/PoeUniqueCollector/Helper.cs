using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PoeUniqueCollector
{
    public static class Helper
    {
        public static void CompareDataLists()
        {
            //var fbFilePath = "..\\..\\BasetypeStorage.csv";
            var fbLocalPath = "C:\\Users\\Tobnac\\WebstormProjects\\fb\\datafiles\\other\\BasetypeStorage.csv";
            var myFilePath = "..\\..\\UniqueCollection.txt";
            //var fbOnlineLink = "http://filterblade.xyz/datafiles/other/BasetypeStorage.csv";

            var fbContent = System.IO.File.ReadAllLines(fbLocalPath);
            //var webClient = new WebClient();
            //fbContent = webClient.DownloadString(fbOnlineLink).Split("\n");
            var myContent = System.IO.File.ReadAllLines(myFilePath);

            var myBTL = new List<string>();
            myContent.ToList().ForEach(x => myBTL.Add(x.Split("-->>").First()));
            myBTL.Sort();

            var fbBTL = new List<string>();
            var fbProph = new List<string>();
            foreach (var line in fbContent)
            {
                var split = line.Split(',');
                var bt = split[1];
                var rarities = split[3];
                var type = split[13];
                
                if (type == "proph") fbProph.Add(bt);

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

            var myProph = System.IO.File.ReadLines("..\\..\\ProphecyList.txt").ToList();
            myProph.Where(x => !fbProph.Contains(x)).ToList().ForEach(x => Console.WriteLine("missing proph in fb list: " + x));
            fbProph.Where(x => !myProph.Contains(x)).ToList().ForEach(x => Console.WriteLine("missing proph in my list: " + x));
            
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
