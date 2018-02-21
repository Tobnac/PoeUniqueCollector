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
    public class FlavourTextPrinter : IItemProcessorModule
    {
        public string DataFilePath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int CollectionSize { get { return knownTexts.Count; } }

        private HashSet<string> knownTexts = new HashSet<string>();

        public bool ScanItem(Item item)
        {
            return false;
            //return item.FlavourText != null && String.Join(" ", item.FlavourText).Contains("Malachai");
        }

        public void ProcessItem(Item item)
        {
            var text = "Unique: " + item.Name.Replace("<<set:MS>><<set:M>><<set:S>>", "") + "\n";
            text += String.Join("\n", item.FlavourText);

            if (this.knownTexts.Contains(text))
            {
                return;
            }
            else
            {
                this.knownTexts.Add(text);
            }

            Console.WriteLine("\n" + text + "\n");
        }

        public void CreateFormattedFile()
        {
            throw new NotImplementedException();
        }

        public void LoadFromFile()
        {
            //
        }

        public void SaveToFile()
        {
            //
        }
    }
}
