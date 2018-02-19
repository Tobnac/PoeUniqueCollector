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
        public PoEObject PoEObject { get; set; }
        public List<IScannerConfig> Configs { get; set; } = new List<IScannerConfig>();

        private APIRequester dataSource;        

        public StashScanner(APIRequester dataSource)
        {
            this.dataSource = dataSource;
        }

        public PoEObject ParseToObject(string response)
        {
            this.PoEObject = PoEObject.FromJson(response);
            this.dataSource.NextID = this.PoEObject.NextChangeId;
            Console.WriteLine("Response parsing complete");
            return this.PoEObject;
        }

        public void ScanAllItems()
        {
            // async demo
            //for (int i = 0; i < 25; i++)
            //{
            //    Console.WriteLine("Calculating... (fake)");
            //    Thread.Sleep(100);
            //}

            foreach (var stash in this.PoEObject.Stashes)
            {
                foreach (var item in stash.Items)
                {
                    foreach (var config in this.Configs)
                    {
                        if (config.ScanItem(item))
                        {
                            config.ProcessItem(item);
                        }
                    }
                }
            }

            this.SaveToFile();
            Console.WriteLine("Item scanning complete");
        }

        private void SaveToFile()
        {
            this.Configs.ForEach(x => x.SaveToFile());
        }
    }
}
