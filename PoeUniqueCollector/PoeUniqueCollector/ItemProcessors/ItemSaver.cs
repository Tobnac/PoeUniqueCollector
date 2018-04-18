using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using PoeUniqueCollector.QuickType;

namespace PoeUniqueCollector.ItemProcessors
{
    [Export(typeof(IItemProcessorModule))]
    public class ItemSaver : IItemProcessorModule
    {
        public string DataFilePath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int CollectionSize => 0;

        public bool DoScanStash { get; set; } = true;

        public void CreateFormattedFile()
        {
        }

        public void LoadFromFile()
        {
        }

        public void ProcessItem(Item item)
        {
            //var i = new TinyItem(item);

            //var j = new TinyItem(i.ToJSON());
            //if (item.Verified) Console.WriteLine(item.Verified);
            Console.WriteLine(item.League);

        }

        public void SaveToFile()
        {
        }

        public bool ScanItem(Item item)
        {
            return false;
        }

        public void ScanStashInfo(Stash stash)
        {
            //Console.WriteLine(stash.Public);
        }
    }

    public class TinyItem
    {
        public Dictionary<string, string> Stats { get; set; } = new Dictionary<string, string>();

        private readonly static string StatSeparator = "---";
        private readonly static string KeyValSeparator = ":";

        public TinyItem(Item fullItem)
        {
            this.Stats.Add("TypeLine", fullItem.TypeLine);
            this.Stats.Add("FrameType", fullItem.FrameType.ToString());
        }

        public TinyItem(string jsonString)
        {
            foreach (var stat in jsonString.Split(TinyItem.StatSeparator))
            {
                var keyVal = stat.Split(TinyItem.KeyValSeparator);
                if (keyVal.Length != 2) throw new Exception();
                this.Stats.Add(keyVal.First(), keyVal.Last());
            }
        }

        public string ToJSON()
        {
            var res = this.Stats.Keys.Select(x => $"{x}{TinyItem.KeyValSeparator}{this.Stats[x]}");
            var result = String.Join(TinyItem.StatSeparator, res);
            return result;
        }
    }
}
