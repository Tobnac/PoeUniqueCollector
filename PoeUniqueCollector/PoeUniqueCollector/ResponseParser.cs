using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeUniqueCollector
{
    public class ResponseParser
    {
        public Dictionary<string, List<string>> UniqueCollection { get; set; } = new Dictionary<string, List<string>>();

        private readonly string uniqueKeyword = "frameType\":3,";
        private readonly string nameKeyword = "\"name\":\"<<set:MS>><<set:M>><<set:S>>";
        private readonly string baseTypeKeyword = "\"typeLine\":\"";

        public void ParseResponse(string content)
        {
            int index;         

            while ((index = content.IndexOf(uniqueKeyword)) != -1)
            {
                // index is not on the verification of the unique rarity.
                // unique name and basetype are BEFORE that in the JSON string
                // so we first get all the content from BEFORE the index to work with
                var temp = content.Substring(0, index);

                // then, we search for the keyword for name and basetype and use the last match
                var uniqueName = ParsePhrase(temp, nameKeyword);            
                var baseType = ParsePhrase(temp, baseTypeKeyword);

                // with the first unique found, we delete the used content from the string and repeat
                content = content.Substring(index + uniqueKeyword.Length);
                
                if (Verify(uniqueName) && Verify(baseType))
                {
                    Save(baseType, uniqueName);
                }
            }

            string ParsePhrase(string text, string s)
            {
                var result = text.Split(s).Last();
                result = result.Substring(0, result.IndexOf('"'));
                return result;
            }

        }

        private bool Verify(string s)
        {
            s = this.Sanitise(s);
            if (s == null || s == "") return false;
            return true;
        }

        private string Sanitise(string s)
        {
            if (s.Contains("Superious"))
            {
                s = s.Substring("Superious ".Length);
            }
            return s;
        }

        private void Save(string baseType, string uniqueName)
        {       
            if (this.UniqueCollection.ContainsKey(baseType))
            {
                if (!this.UniqueCollection[baseType].Contains(uniqueName))
                {
                    this.UniqueCollection[baseType].Add(uniqueName);
                    Print();

                }
            }

            else
            {
                this.UniqueCollection.Add(baseType, new List<string>() { uniqueName });
                Print();
            }

            void Print() => Console.WriteLine(this.UniqueCollection.Count);
        }

        public void PrintBaseTypes()
        {
            foreach (KeyValuePair<string, List<string>> entry in this.UniqueCollection)
            {
                Console.WriteLine(entry.Key);
            }
        }
    }
}
