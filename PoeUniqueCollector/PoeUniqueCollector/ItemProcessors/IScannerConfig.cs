using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoeUniqueCollector.QuickType;

namespace PoeUniqueCollector.ItemProcessors
{
    public interface IItemProcessorModule
    {
        string DataFilePath { get; set; }
        int CollectionSize { get; }

        void ProcessItem(Item item);
        bool ScanItem(Item item);
        void SaveToFile();
        void LoadFromFile();
        void CreateFormattedFile();
    }
}
