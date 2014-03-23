using System;
using System.Collections.Generic;
using System.Linq;

namespace EmailSenderProgram
{
    public static class Extensions
    {
        public static object[] Replace(this IEnumerable<object> collection, IEnumerable<Tuple<string, object>> swapPairs)
        {
            List<object> result = new List<object>();
            foreach (object item in collection)
            {
                foreach (Tuple<string, object> pair in swapPairs)
                {
                    if (item.GetType() != typeof(string))
                    {
                        result.Add(item);
                        break;
                    }
                    if (item.ToString() == pair.Item1)
                        result.Add(pair.Item2);
                }
            }
            return result.ToArray();
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || collection.Count() == 0;
        }

        public static int ExpectedArguments(this string haystack)
        {
            int param = -1;
            string[] indexes = haystack.Replace("{{", "").Replace("}}", "").Split("{}".ToCharArray());
            for (int x = 1; x < indexes.Length - 1; x += 2)
            {
                int thisparam;
                if (Int32.TryParse(indexes[x], out thisparam) && param < thisparam)
                    param = thisparam;
            }
            return param == -1 ? 0 : param + 1;
        }

        public static void Echo(string text, ConsoleColor foregroundColor = ConsoleColor.Gray)
        {
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
