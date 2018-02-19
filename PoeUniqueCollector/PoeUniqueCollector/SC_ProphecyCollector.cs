using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoeUniqueCollector.QuickType;

namespace PoeUniqueCollector
{
    public class SC_ProphecyCollector : IScannerConfig
    {
        public List<string> Collection { get; set; } = new List<string>();
        public string DataFilePath { get; set; } = "..\\..\\ProphecyList.txt";
        public int CollectionSize { get { return this.Collection.Count; } private set => throw new NotImplementedException(); }

        public SC_ProphecyCollector()
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
                this.Collection.Add(line);
            }
        }

        public void SaveToFile()
        {
            var content = new List<string>();

            foreach (var prophecy in this.Collection)
            {
                content.Add(prophecy);
            }

            System.IO.File.WriteAllLines(this.DataFilePath, content.ToArray());
        }

        public bool ScanItem(Item item)
        {
            if (item.FrameType != 8) return false;

            return true;
        }

        public void ProcessItem(Item item)
        {
            var name = item.TypeLine;

            if (!this.Collection.Contains(name))
            {                
                this.Collection.Add(name);
                Console.WriteLine($"New Prophecy: {name} ({this.Collection.Count})");
            }
        }
    }
}
