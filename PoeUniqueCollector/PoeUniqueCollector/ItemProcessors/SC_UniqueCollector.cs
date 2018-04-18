using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoeUniqueCollector.QuickType;
using System.ComponentModel.Composition;

namespace PoeUniqueCollector.ItemProcessors
{
    [Export(typeof(IItemProcessorModule))]
    public class SC_UniqueCollector : IItemProcessorModule
    {
        public Dictionary<string, List<string>> Collection { get; set; } = new Dictionary<string, List<string>>();
        public string DataFilePath { get; set; } = "..\\..\\UniqueCollection.txt";
        public bool DoScanStash { get; set; }

        public int CollectionSize
        {
            get
            {
                var res = 0;
                foreach (var entry in this.Collection)
                {
                    res += entry.Value.Count;
                }
                return res;
            }

            private set => throw new NotImplementedException();
        }

        public SC_UniqueCollector()
        {
            this.LoadFromFile();
        }

        public void LoadFromFile()
        {
            string[] content;

            try
            {
                content = System.IO.File.ReadAllLines(this.DataFilePath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while reading UniqueData file");
                Console.WriteLine(e.Message);
                Console.ReadLine();
                return;
            }

            foreach (var line in content)
            {
                var split = line.Split("-->>");
                var baseType = split[0];
                var names = split[1].Split(";;");

                this.Collection.Add(baseType, names.ToList());
            }
        }

        public void SaveToFile()
        {
            var content = new List<string>();

            foreach (var entry in this.Collection)
            {
                var line = entry.Key;
                line += "-->>";
                line += String.Join(";;", entry.Value);
                content.Add(line);
            }

            System.IO.File.WriteAllLines(this.DataFilePath, content.ToArray());
        }

        private void SaveUnique(string baseType, string uniqueName)
        {
            if (this.Collection.ContainsKey(baseType))
            {
                if (!this.Collection[baseType].Contains(uniqueName))
                {
                    this.Collection[baseType].Add(uniqueName);
                    Console.WriteLine("New Unique: " + uniqueName + " (" + this.Collection[baseType].Count + ")");
                }
            }

            else
            {
                this.Collection.Add(baseType, new List<string>() { uniqueName });
                Console.WriteLine("New BaseType: " + baseType + " (" + this.Collection.Count + ")");
            }
        }

        public bool ScanItem(Item item)
        {
            // catch all non-uniques
            if (item.FrameType != 3) return false;

            // no item name --> not identified
            if (item.Name == "" && item.Identified == false)
            {
                return false; // still save that the baseType HAS any uniques?
            }

            return true;
        }

        public void ProcessItem(Item item)
        {
            // fix weird unique name
            if (item.Name.Contains("<<set:MS>><<set:M>><<set:S>>"))
            {
                item.Name = item.Name.Replace("<<set:MS>><<set:M>><<set:S>>", "");
            }

            // delete "Superiour" for items with quality
            if (item.Name.Contains("Superiour"))
            {
                item.Name = item.Name.Substring("Superiour ".Length);
            }

            SaveUnique(item.TypeLine, item.Name);
        }

        public void CreateFormattedFile()
        {
            var content = new List<string>();

            foreach (var entry in this.Collection)
            {
                var line = entry.Key;
                line += "-->>";
                line += String.Join(";;", entry.Value);
                content.Add(line);
            }

            System.IO.File.WriteAllLines(this.DataFilePath, content.ToArray());
        }

        public void ScanStashInfo(Stash stash)
        {
            this.DoScanStash = true;
        }
    }
}
