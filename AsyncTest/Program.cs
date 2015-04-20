using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTest
{
    class Program
    {
        private static bool PromptAfter = true;

        /// <summary>
        /// adding comments by Avani
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //adding comment by Avani
            Console.WriteLine("Main start");

            for (int i = 1; i < 10; i++)
            {
                Task t = asyncMethod(i);
                t.ContinueWith((str) =>
                {
                    Console.WriteLine(str.Status.ToString());
                });
                t.Wait();
            }

            Prompt();
        }

        public async static Task<string> asyncMethod(int i)
        {
            Console.WriteLine("public async static  void asyncMethod() start " + i.ToString());
            await Task.Delay(100);
            Thread.Sleep(1000);
            Console.WriteLine("public async static  void asyncMethod() end" + i.ToString());
            return "finished";
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
