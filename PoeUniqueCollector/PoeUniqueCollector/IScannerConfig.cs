using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoeUniqueCollector.QuickType;

namespace PoeUniqueCollector
{
    public interface IScannerConfig
    {
        string DataFilePath { get; set; }
        int CollectionSize { get; }

        void SaveToFile();
        void LoadFromFile();
        void ProcessItem(Item item);
        bool ScanItem(Item item);
    }
}
