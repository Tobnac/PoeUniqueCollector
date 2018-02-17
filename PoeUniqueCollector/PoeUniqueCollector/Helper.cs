using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeUniqueCollector
{
    public static class Helper
    {
    }

    public static class ClassExtensions
    {
        // string.Split() overload for a simple string split
        public static string[] Split(this string s, string keyword)
        {
            return s.Split(new string[] { keyword }, StringSplitOptions.None);
        }
    }
}
