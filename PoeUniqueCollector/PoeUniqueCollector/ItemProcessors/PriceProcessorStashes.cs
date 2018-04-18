using PoeUniqueCollector.QuickType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeUniqueCollector.ItemProcessors
{
    public partial class PriceProcessor : IItemProcessorModule
    {
        private Dictionary<string, Dictionary<string, SimpleItem>> stashIDs = new Dictionary<string, Dictionary<string, SimpleItem>>();
        private string currentStashID;

        // todo: save+load this info to file!!

        public void ScanStashInfo(Stash stash)
        {
            this.currentStashID = stash.Id;

            if (this.stashIDs.ContainsKey(this.currentStashID))
            {
                // compare old and new stash content
                // -> index new items
                // -> evaluate removed items: were they sold? removed?

                this.DoScanStash = true; //todo change back to false
                this.CompareStashes(stash);
            }

            else
            {
                // index stash + all items
                this.DoScanStash = true;
                this.stashIDs.Add(stash.Id, new Dictionary<string, SimpleItem>());
            }
        }

        private void CompareStashes(Stash newStash)
        {
            var oldStash = this.stashIDs[newStash.Id];

            // todo: what happens with re-priced items?

            var newStashIDs = newStash.Items.Select(x => x.Id);
            var oldStashIDs = oldStash.Keys.ToList();
            var unchangedItemIDs = newStashIDs.Intersect(oldStashIDs).ToList();

            unchangedItemIDs.ForEach(x => this.CompareUnchangedItems(this.GetItemByID(x, newStash), oldStash[x]));
            var removedItems = oldStashIDs.Where(x => !newStashIDs.Contains(x)).Select(x => oldStash[x]).ToList();
            var newItems = newStashIDs.Where(x => !oldStashIDs.Contains(x)).Select(x => this.GetItemByID(x, newStash)).ToList();

            removedItems.ForEach(x => this.EvaluateRemovedItem(x));
            newItems.ForEach(x => this.ProcessItem(x));
        }

        private void CompareUnchangedItems(Item newItem, SimpleItem oldItem)
        {
            // compare prices?
        }

        private void EvaluateRemovedItem(SimpleItem item)
        {
            var value = this.GetAvgValueForCurrency(item.PriceCurrency) * item.PriceAmount;
            var average = this.GetAvgValueForCurrency(this.TranslateCurrencyToShort(item.TypeLine));
            var offValue = value > average ? value / average : average / value;

            // if offValue is big -> item was probably removed
            // if offValue is small -> item was sold for this price -> add it with big impact
            // if offValue is medium -> ???
        }

        private Item GetItemByID(string id, Stash stash)
        {
            foreach (var item in stash.Items)
            {
                if (item.Id == id) return item;
            }

            throw new KeyNotFoundException();
        }
    }
}
