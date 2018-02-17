using PoeUniqueCollector.QuickType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeUniqueCollector
{
    public class StashScanner
    {
        public Dictionary<string, List<string>> UniqueCollection { get; set; } = new Dictionary<string, List<string>>();
        public PoEObject PoEObject { get; set; }
        private APIRequester dataSource;

        public StashScanner(APIRequester dataSource)
        {
            this.dataSource = dataSource;
        }

        public PoEObject ParseToObject(string response)
        {
            this.PoEObject = PoEObject.FromJson(response);
            this.dataSource.NextID = this.PoEObject.NextChangeId;
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
                    Print();

                }
            }

            else
            {
                this.UniqueCollection.Add(baseType, new List<string>() { uniqueName });
                Print();
            }

            void Print() => Console.WriteLine(this.UniqueCollection.Count);
        }

        public void PrintBaseTypes()
        {
            foreach (KeyValuePair<string, List<string>> entry in this.UniqueCollection)
            {
                Console.WriteLine(entry.Key);
            }
        }
    }
}
