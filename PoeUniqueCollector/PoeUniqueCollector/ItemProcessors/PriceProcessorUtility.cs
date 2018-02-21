using PoeUniqueCollector.QuickType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeUniqueCollector.ItemProcessors
{
    public partial class PriceProcessor
    {
        public PriceProcessor()
        {
            this.CurrencyTranslation.Add("Chaos Orb", "chaos");
            this.CurrencyTranslation.Add("Vaal Orb", "vaal");
            this.CurrencyTranslation.Add("Exalted Orb", "exa");
            this.CurrencyTranslation.Add("Orb of Alchemy", "alch");
            this.CurrencyTranslation.Add("Orb of Scouring", "scour");
            this.CurrencyTranslation.Add("Orb of Chance", "chance");
            this.CurrencyTranslation.Add("Cartographer's Chisel", "chisel");
            this.CurrencyTranslation.Add("Jeweller's Orb", "jew");
            this.CurrencyTranslation.Add("Orb of Alteration", "alt");
            this.CurrencyTranslation.Add("Chromatic Orb", "chrom");
            this.CurrencyTranslation.Add("Gemcutter's Prism", "gcp");
            this.CurrencyTranslation.Add("Orb of Fusing", "fuse");
            this.CurrencyTranslation.Add("Divine Orb", "divine");
            this.CurrencyTranslation.Add("Silver Coin", "silver");
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
                this.InitializeCurrencyValues();
                this.SaveToFile();
                return;
            }

            foreach (var line in content)
            {
                var split = line.Split("-->>");
                var currency = split[0];
                var numberStringList = split[1].Split(", ");
                var numbers = numberStringList.Select(x => Double.Parse(x));

                this.PriceLists.Add(currency, numbers.ToList());
            }
        }

        public void SaveToFile()
        {
            this.SanetizeValues();
            var content = new List<string>();
            var dic = new Dictionary<string, List<double>>(this.PriceLists);
            var ddic = dic.OrderByDescending(x => x.Value.Count);

            foreach (var entry in ddic)
            {
                var line = entry.Key;
                var temp = entry.Value.Select(x => System.Math.Round(x, 3));
                line += "-->>";
                line += String.Join(", ", temp);
                content.Add(line);
            }

            System.IO.File.WriteAllLines(this.DataFilePath, content.ToArray());
        }

        public bool ScanItem(Item item)
        {
            // no price
            if (item.Note == null) return false;

            // unrecognized price tag
            if (!this.regExp.IsMatch(item.Note))
            {
                return false;
            }

            // non-currency
            if (item.FrameType != 5) return false;

            // weird curreny
            string[] filteredCurrency = { "Essence", "Splinter", "Blessing of" };
            if (filteredCurrency.Any(x => item.TypeLine.Contains(x))) return false;

            // league
            if (item.League != "Abyss") return false;

            return true;
        }

        private double ParseToDouble(string s)
        {
            if (s.Contains('/'))
            {
                var splits = s.Split('/');
                var a = int.Parse(splits[0]);
                var b = int.Parse(splits[1]);
                return a / b;
            }

            else if (s[0] == '.')
            {
                s = "0" + s;
                return Double.Parse(s);
            }

            else
            {
                Double.TryParse(s, out double res);
                return res;
            }
        }

        public void CreateFormattedFile()
        {
            //
        }

        private void InitializeCurrencyValues()
        {
            Console.WriteLine("Do you really want to init the values? Type 'y' to confirm.");
            if (Console.ReadLine() != "y") return;

            InitCurr("vaal", 1.5);
            InitCurr("exa", 35);
            InitCurr("alch", 0.3);
            InitCurr("scour", 0.3);
            InitCurr("chance", 0.09);
            InitCurr("chisel", 1 / 3);
            InitCurr("jew", 1 / 11);
            InitCurr("alt", 1 / 13);
            InitCurr("chrom", 1 / 15);
            InitCurr("gcp", 1.5);
            InitCurr("fuse", 1 / 2.5);
            InitCurr("divine", 7);
            InitCurr("Orb of Augmentation", 0.05);

            void InitCurr(string cur, double value)
            {
                this.PriceLists.Add(cur, new List<double>());

                for (int i = 0; i < 25; i++)
                {
                    this.PriceLists[cur].Add(value);
                }
            }
        }

        public string TranslateCurrencyToShort(string s)
        {
            if (this.CurrencyTranslation.ContainsKey(s))
            {
                return this.CurrencyTranslation[s];
            }

            return s;
        }

        public void PrintPrices()
        {
            var dic = new Dictionary<string, List<double>>(this.PriceLists);
            var ddic = dic.OrderByDescending(x => x.Value.Count);

            foreach (var entry in ddic)
            {
                Console.WriteLine($"{entry.Key}: {entry.Value.Average()} (based on {entry.Value.Count})");
            }
        }
    }
}
