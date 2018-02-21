using PoeUniqueCollector.ItemProcessors;
using PoeUniqueCollector.QuickType;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PoeUniqueCollector
{
    public class StashScanner
    {
        [ImportMany(typeof(IItemProcessorModule))]
        public List<IItemProcessorModule> ItemProcessors { get; set; }
        public PoEObject PoEObject { get; set; }

        private APIRequester dataSource;        

        public StashScanner(APIRequester dataSource)
        {
            this.dataSource = dataSource;
            this.ComposeScanModules();
        }

        private void ComposeScanModules()
        {
            var asmCata = new AssemblyCatalog(GetType().Assembly);
            var container = new CompositionContainer(asmCata);
            container.ComposeParts(this);
        }

        public PoEObject ParseToObject(string response)
        {
            this.PoEObject = PoEObject.FromJson(response);
            this.dataSource.NextID = this.PoEObject.NextChangeId;
            Console.WriteLine("(INFO) Response parsing complete");
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

            var count = 0;

            foreach (var stash in this.PoEObject.Stashes)
            {
                foreach (var item in stash.Items)
                {
                    count++;

                    foreach (var processor in this.ItemProcessors)
                    {
                        if (processor.ScanItem(item))
                        {
                            processor.ProcessItem(item);
                        }
                    }
                }
            }

            this.SaveToFile();
            Console.WriteLine($"(INFO) Item scanning complete: {count} items scanned.");
        }

        private void SaveToFile()
        {
            this.ItemProcessors.ForEach(x => x.SaveToFile());
        }
    }
}
