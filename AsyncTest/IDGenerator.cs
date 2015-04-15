using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTest
{
    class IdGenerator
    {
        private static bool PromptAfter = true;

        static void Main(string[] args)
        {
            Console.WriteLine("Main start");

            try
            {
                Random r = new Random(Guid.NewGuid().GetHashCode());
                List<ItemWithID> items = new List<ItemWithID>();
                ChildIdParser parser = new ChildIdParser();

                int maxItems = 6;
                for (int i = 0; i < maxItems; i++)
                {
                    ItemWithID item = new ItemWithID() { id = parser.GenerateID(r.Next(0, 9900)) };
                    if (items.Find(ii => ii.id == item.id) == null)
                        items.Add(item);
                }

                //this generates an exeption, because we're out of id's!
                //items.Add(new ItemWithID() { id = parser.GenerateID(9999) });

                ShowItems(items);

                ItemWithID newitem = new ItemWithID() { id = parser.NewID(items) };

                Console.WriteLine("new item is " + newitem.id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Prompt();
        }

        private static void ShowItems(List<ItemWithID> items)
        {
            foreach (var i in items)
                Console.WriteLine(i.id);
        }

        private static void Prompt()
        {
            if (PromptAfter)
            {
                Console.WriteLine("press any key");
                Console.ReadKey(true);
            }
        }

        public class ItemWithID
        {
            public string id;
        }


        public abstract class AIdParser<T>
        {
            public abstract int ID_MAX { get; }
            public abstract string ID_FORMAT { get; }
            public abstract int PREFIX_LENGTH { get; }

            public string GenerateID(int num)
            {
                if (num > ID_MAX)
                    throw new Exception(string.Format("Cannot generate ID for type {1} : Number exceeds maximum ({0})", ID_MAX, typeof(T).Name));
                return string.Format(ID_FORMAT, num);
            }

            /// <summary>
            /// This method assumes that the numeric part of the ID is in the last part of the string.
            /// if it were embedded, we'd have to use a regex for this.
            /// </summary>
            public int ParseID(string sId)
            {
                string numeric = sId.Substring(PREFIX_LENGTH);  //lop off the first few characters, as defined by PrefixLength
                int id;
                if (!int.TryParse(numeric, out id))
                    throw new Exception(string.Format("Invalid ID found: {0}", sId));
                return id;
            }

            /// <summary>
            /// This needs to be completely overridden, because it's dependent on which field the ID comes from.
            /// </summary>
            public abstract string NewID(List<T> items);
        }

        public class ChildIdParser : AIdParser<ItemWithID>
        {
            public override string ID_FORMAT
            {
                get { return "ABCD{0:D4}"; }
            }

            public override int ID_MAX
            {
                get { return 9999; }
            }

            public override int PREFIX_LENGTH
            {
                get { return 4; }
            }

            public override string NewID(List<ItemWithID> items)
            {
                int max = items.Max(i => ParseID(i.id));
                return GenerateID(max + 1);
            }
        }

        /// <summary>
        /// non-generic class, initially to get logic correct.
        /// </summary>
        public class IdParser
        {
            const int MAX_ID = 9999;
            const string format = "ABCD{0:D4}";
            const int prefixLength = 4;

            public string GenerateID(int num)
            {
                if (MAX_ID > 9999)
                    throw new Exception(string.Format("Cannot generate ID: Number exceeds maximum ({0})", MAX_ID));
                return string.Format(format, num);
            }

            public int ParseID(string sId)
            {
                string numeric = sId.Substring(prefixLength);  //lop off the first few characters
                int id;
                if (!int.TryParse(numeric, out id))
                    throw new Exception(string.Format("Invalid ID found: {0}", sId));
                return id;
            }

            public string NewID(List<ItemWithID> items)
            {
                int max = items.Max(i => ParseID(i.id));
                return GenerateID(max + 1);
            }
        }
    }


}
