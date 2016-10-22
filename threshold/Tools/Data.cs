using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Cryptography;

namespace threshold.Tools
{
    public static class DataHelper
    {
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
                return -999;
            }
        }

        public static List<string> ToList(string multiline)
        {
            var list = new List<string>();

            if (!string.IsNullOrWhiteSpace(multiline))
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
                    list = Enumerable.Empty<string>().ToList<string>();
                }
            }
            else
            {
                list = Enumerable.Empty<string>().ToList<string>();
            }

            return list;
        }

        public static string GetMd5Hash(string filePath)
        {
            string md5Hash;

            if (File.Exists(filePath))
            {
                // MD5 hash example sourced from:
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

        // List IsNullOrEmpty methods sourced from:
        // http://danielvaughan.org/post/IEnumerable-IsNullOrEmpty.aspx
        /// <summary>
        /// Determines whether the collection is null or contains no elements.
        /// </summary>
        /// <typeparam name="T">The IEnumerable type.</typeparam>
        /// <param name="enumerable">The enumerable, which may be null or empty.</param>
        /// <returns>
        ///     <c>true</c> if the IEnumerable is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return true;
            }
            /* If this is a list, use the Count property. 
             * The Count property is O(1) while IEnumerable.Count() is O(N). */
            var collection = enumerable as ICollection<T>;
            if (collection != null)
            {
                return collection.Count < 1;
            }
            return enumerable.Any();
        }

        /// <summary>
        /// Determines whether the collection is null or contains no elements.
        /// </summary>
        /// <typeparam name="T">The IEnumerable type.</typeparam>
        /// <param name="collection">The collection, which may be null or empty.</param>
        /// <returns>
        ///     <c>true</c> if the IEnumerable is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            if (collection == null)
            {
                return true;
            }
            return collection.Count < 1;
        }
    }
}
