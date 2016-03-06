using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace threshold.Tools
{
    public static class DataHelper
    {
        public static List<string> ToList(string multiline)
        {
            var list = new List<string>();

            // StringReader code example sourced from:
            // http://stackoverflow.com/a/1500257
            using (StringReader strReader = new StringReader(multiline))
            {
                string line;
                while ((line = strReader.ReadLine()) != null)
                {
                    line = line.Trim();
                    list.Add(line);
                }
            }

            return list;
        }

        public static int ToInt(string str)
        {
            int newInt;
            bool conversionResult = int.TryParse(str, out newInt);

            if (conversionResult == true)
            {
                return newInt;
            }
            else
            {
                return 0;
            }
        }
    }
}
