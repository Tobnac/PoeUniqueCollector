using PoeUniqueCollector.QuickType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PoeUniqueCollector
{
    public class StashScanner
    {
        public Dictionary<string, List<string>> UniqueCollection { get; set; } = new Dictionary<string, List<string>>();
        public PoEObject PoEObject { get; set; }

        private APIRequester dataSource;
        private string savedDataFilePath = "..\\..\\UniqueCollection.txt";

        public StashScanner(APIRequester dataSource)
        {
            this.dataSource = dataSource;
            this.LoadFromFile();
        }

        public PoEObject ParseToObject(string response)
        {
            this.PoEObject = PoEObject.FromJson(response);
            this.dataSource.NextID = this.PoEObject.NextChangeId;
            Console.WriteLine("Response parsing complete");
            return this.PoEObject;
        }

        public void ScanUniques()
        {
            foreach (var stash in this.PoEObject.Stashes)
            {
                foreach (var item in stash.Items)
                {
                    ScanUniqueItem(item);
                }
            }

            this.SaveToFile();
            Console.WriteLine("Unique scanning complete");
        }

        private void ScanUniqueItem(Item item)
        {
            // catch all non-uniques
            if (item.FrameType != 3) return;

            // no item name --> not identified
            if (item.Name == "" && item.Identified == false)
            {
                return; // still save that the baseType HAS any uniques?
            }

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

        private void SaveUnique(string baseType, string uniqueName)
        {
            if (this.UniqueCollection.ContainsKey(baseType))
            {
                if (!this.UniqueCollection[baseType].Contains(uniqueName))
                {
                    this.UniqueCollection[baseType].Add(uniqueName);
                    Console.WriteLine("New Unique: " + uniqueName + " (" + this.UniqueCollection[baseType].Count + ")");
                }
            }

            else
            {
                this.UniqueCollection.Add(baseType, new List<string>() { uniqueName });
                Console.WriteLine("New BaseType: " + baseType + " (" + this.UniqueCollection.Count + ")");
            }
        }

        public void PrintBaseTypes()
        {
            foreach (KeyValuePair<string, List<string>> entry in this.UniqueCollection)
            {
                Console.WriteLine(entry.Key);
            }
        }

        private void SaveToFile()
        {
            var content = new List<string>();

            foreach (var entry in this.UniqueCollection)
            {
                var line = entry.Key;
                line += "-->>";
                line += String.Join(";;", entry.Value);
                content.Add(line);
            }

            System.IO.File.WriteAllLines(this.savedDataFilePath, content.ToArray());
        }

        private void LoadFromFile()
        {
            string[] content;

            try
            {
                content = System.IO.File.ReadAllLines(this.savedDataFilePath);
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

                this.UniqueCollection.Add(baseType, names.ToList());
            }
        }
    }
}
