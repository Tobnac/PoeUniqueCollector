using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoeUniqueCollector.QuickType;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;

namespace PoeUniqueCollector.ItemProcessors
{
    //[Export(typeof(IItemProcessorModule))]
    public partial class PriceProcessor : IItemProcessorModule
    {
        public Dictionary<string, string> CurrencyTranslation { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, List<double>> PriceLists { get; set; } = new Dictionary<string, List<double>>();
        public string DataFilePath { get; set; } = "..\\..\\CurrencyPrices.txt";
        public bool DoScanStash { get; set; }
        public int CollectionSize
        {
            get
            {
                var res = 0;
                foreach (var entry in this.PriceLists)
                {
                    res += entry.Value.Count;
                }
                return res;
            }
        }       

        private Regex regExp = new Regex(@"~(?:(?:price )|(?:b\/o ))(.?\d+(?:(?:.\d+)|(?:\/\d+))?) (\w+)");

        public void ProcessItem(Item item)
        {
            var priceString = item.Note;
            var matches = this.regExp.Match(priceString);

            var amount = this.ParseToDouble(matches.Groups[1].Value);
            var priceCurrency = matches.Groups[2].Value;
            var currency = item.TypeLine;

            if (amount <= 0) return;

            var tempItem = new SimpleItem
            {
                Id = item.Id,
                PriceAmount = amount,
                PriceCurrency = priceCurrency,
                TypeLine = currency
            };
            this.stashIDs[this.currentStashID].Add(item.Id, tempItem);

            //Console.WriteLine($"Selling {currency} for {amount} {priceCurrency}!");
            this.EvaluatePrice(this.TranslateCurrencyToShort(currency), priceCurrency, amount);
        }

        private void EvaluatePrice(string sellItem, string buyItem, double buyAmount)
        {
            var expectedCost = this.GetAvgValueForCurrency(sellItem);
            var actualCost = this.GetAvgValueForCurrency(buyItem) * buyAmount;
            var dealQuality = expectedCost / actualCost;
            var legit = expectedCost > 0 && actualCost > 0;
            var profit = expectedCost - actualCost;

            // ignore VERY overprices deals
            if (legit && dealQuality < 0.26)
            {
                Console.WriteLine($"Ignoring terrible deal: {sellItem} (value: {expectedCost}) for {buyAmount} {buyItem} (value: {actualCost}). Deal quality: {dealQuality}");
                return;
            }

            // actually good deals we might take
            if (legit && dealQuality >= 1.2)
            {
                string mood;
                if (dealQuality >= 10) mood = "!!!AWESOME!!!";
                else if (dealQuality >= 5) mood = "GREAT";
                else if (dealQuality >= 2) mood = "very good";
                else if (dealQuality >= 1.5) mood = "nice";
                else mood = "\"okay\"";

                Console.WriteLine($"(DEAL) {mood} deal: Let's buy {sellItem} (value: {expectedCost}) for {buyAmount} {buyItem} (value: {actualCost}). Deal quality: {dealQuality}, profit: {profit}");
            }

            // someone sells chaos
            if (sellItem == "chaos")
            {
                this.AddToPriceList(buyItem, 1 / buyAmount);
            }

            // someone is selling something FOR chaos
            else if (buyItem == "chaos")
            {
                this.AddToPriceList(sellItem, buyAmount);
            }

            // trade without chaos
            else
            {
                var valueA = this.GetAvgValueForCurrency(buyItem);
                var valueB = this.GetAvgValueForCurrency(sellItem);

                if (valueA > 0)
                {
                    this.AddToPriceList(sellItem, valueA * buyAmount);
                }

                if (valueB > 0)
                {
                    this.AddToPriceList(buyItem, valueB / buyAmount);
                }
            }
        }

        private void AddToPriceList(string currency, double value)
        {
            if (this.PriceLists.ContainsKey(currency))
            {
                value = this.NormalizeValue(value, this.PriceLists[currency].Average());
                this.PriceLists[currency].Add(value);
                while (this.PriceLists[currency].Count > 50) this.PriceLists[currency].RemoveAt(0);
            }

            else
            {
                this.PriceLists.Add(currency, new List<double>() { value });
            }
        }

        private double GetAvgValueForCurrency(string currency)
        {
            if (currency == "chaos") return 1;

            if (!this.PriceLists.ContainsKey(currency))
            {
                return -1;
            }

            if (this.PriceLists[currency].Count < 5)
            {
                return -1;
            }

            return this.PriceLists[currency].Average();
        }

        private void SanetizeValues()
        {
            foreach (var entry in this.PriceLists)
            {
                if (entry.Value.Count < 5) continue;

                var list = entry.Value;
                var avg = list.Average();

                for (int i = 0; i < list.Count; i++)
                {
                    var bigger = list[i] > avg;
                    var offValue = bigger ? list[i] / avg : avg / list[i];

                    if (offValue > 10)
                    {
                        //Console.WriteLine($"Invalid: {entry.Key} with value {list[i]}. (avg: {avg})");
                        list[i] = bigger ? avg * 1.03 : avg * 0.97;
                        avg = list.Average();
                    }
                }
            }
        }

        private double NormalizeValue(double value, double average)
        {
            // offValue = a value to determine "how much" the new value is a outliers (Ausreißer) compared to the average
            // higher offValue -> more difference between average and the new value

            var bigger = value > average;
            var offValue = bigger ? value / average : average / value;

            var valParticipation = 1 / offValue;
            var avgParticipation = 1 - valParticipation;

            var res = value * valParticipation + average * avgParticipation;

            //Console.WriteLine($"Normalized {value} to {res}. (avg is {average}");
            if (res == 0) Console.WriteLine("FAIL!");
            return res;
        }
    }
}
