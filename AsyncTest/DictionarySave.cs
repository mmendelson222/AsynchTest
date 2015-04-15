using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace AsyncTest
{
    class DictionarySave
    {
        private static bool PromptAfter = true;

        static void Main(string[] args)
        {
            Console.WriteLine("Main start");
            LoggedInUsers user = LoggedInUsers.Instance;

            if (false)
            {
                Guid g = Guid.NewGuid();
                user.Add(g, "adf");
                user.Add(Guid.NewGuid(), "fdsa");
                user.Serialize();
                user.Deserialize();
                Console.WriteLine(user[g]);
            }

            {
                user.Add(Guid.NewGuid(), "Michael");
                Guid g = new Guid("f3a28092-9e42-4fdb-a7c9-ab528d135112");
                if (user.ContainsKey(g))
                    Console.WriteLine(user[g]);
                else
                    Console.WriteLine("nope");
            }

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


    public sealed class LoggedInUsers : Dictionary<Guid, string>
    {
        private static volatile LoggedInUsers instance;
        private static object syncRoot = new Object();
        private LoggedInUsers() { }
        static string filename = "LoggedInUsers.xml";

        public static LoggedInUsers Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new LoggedInUsers();
                            if (File.Exists(filename))
                                instance.Deserialize();
                        }
                    }
                }
                return instance;
            }
        }

        public new void Add(Guid key, string value){
            base.Add(key, value);
            Serialize();
        }

        public class item
        {
            public Guid id;
            public string value;
        }

        public void Serialize()
        {
            XElement xElem = new XElement("items", this.Select(x => new XElement("item", new XAttribute("id", x.Key), new XAttribute("value", x.Value))));
            File.WriteAllText(filename, xElem.ToString());
        }

        public void Deserialize()
        {
            this.Clear();
            string xml = File.ReadAllText(filename);
            XElement xElem2 = XElement.Parse(xml);
            foreach (KeyValuePair<Guid, string> kv in xElem2.Descendants("item").ToDictionary(x => (Guid)x.Attribute("id"), x => (string)x.Attribute("value")))
            {
                this.Add(kv.Key, kv.Value);
            }
        }

    }
}
