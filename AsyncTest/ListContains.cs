using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTest
{
    class ListContains
    {
        private static bool PromptAfter = true;

        static void Main(string[] args)
        {
            Console.WriteLine("Main start");

            List<string> ignoreItems = new List<string>(){
                "michael", "mend"
            };

            string[] testItems = new string[] { "zzz", "michael", "Mendelson", "mendelson" };

            foreach (string item in testItems)
            {
                Console.WriteLine("TESTING " + item);

                if (ignoreItems.Contains(item))
                    Console.WriteLine(string.Format("{0} is on the list", item));

                if (ignoreItems.Find(i => item.Contains(i)) != null)
                    Console.WriteLine(string.Format("{0} is on the list (using compare)", item));

                if (ignoreItems.Find(i => item.IndexOf(i, StringComparison.OrdinalIgnoreCase) > -1) != null)
                    Console.WriteLine(string.Format("{0} is on the list (using StringComparison)", item));


                //if (ignoreItems.Find(i => Regex.IsMatch(item, i, RegexOptions.IgnoreCase)) != null) ;
                //    Console.WriteLine(string.Format("{0} is on the list (using regex)", item));
            }
            // ))

            Prompt();
        }

        private static void Prompt()
        {
            if (PromptAfter)
            {
                Console.WriteLine("press any key");
                Console.ReadKey(true);
            }
        }
    }
}
