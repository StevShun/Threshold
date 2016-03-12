using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace threshold.Tools
{
    public static class Data
    {
        public static List<string> ToList(string multiline)
        {
            var list = new List<string>();

            if (!String.IsNullOrWhiteSpace(multiline))
            {
                // StringReader code example sourced from:
                // http://stackoverflow.com/a/1500257
                try
                {
                    using (StringReader strReader = new StringReader(multiline))
                    {
                        string line;
                        while ((line = strReader.ReadLine()) != null)
                        {
                            line = line.Trim();
                            list.Add(line);
                        }
                    }
                }
                catch
                {
                    list = Enumerable.Empty<String>().ToList<String>();
                }
            }
            else
            {
                list = Enumerable.Empty<String>().ToList<String>();
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

        public static string GetMd5Hash(string filePath)
        {
            string md5Hash;

            if (File.Exists(filePath))
            {
                // MD5 hash example taken from:
                // http://stackoverflow.com/a/10520086
                try
                {
                    using (var md5 = MD5.Create())
                    {
                        using (var stream = File.OpenRead(filePath))
                        {
                            md5Hash = BitConverter.ToString(
                                md5.ComputeHash(stream)
                                ).Replace("-", "").ToLower();
                        }
                    }
                }
                catch
                {
                    md5Hash = "";
                }
            }
            else
            {
                md5Hash = "";
            }

            return md5Hash;
        }
    }
}
