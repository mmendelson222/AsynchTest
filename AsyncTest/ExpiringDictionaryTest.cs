using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;


namespace AsyncTest
{
    class ExpiringDictionaryTest
    {
        private static bool PromptAfter = true;

        static void Main(string[] args)
        {
            Console.WriteLine("Main start");
            ExpiringDictionary<string, string> mylist = new ExpiringDictionary<string, string>(500, 2000);

            for (int i = 1; i < 10; i++)
            {
                mylist.Add(i.ToString(), string.Format("This was added at {0}", DateTime.Now.ToString("mm ss.ff")));
                System.Threading.Thread.Sleep(500);

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("There are {0} items on the list: ", mylist.Count);
                foreach (KeyValuePair<string, string> kv in mylist)
                    sb.AppendFormat("\r\n\tkey: {0} value: {1}", kv.Key, kv.Value);
                Debug.WriteLine(sb);
            }

            if (PromptAfter)
            {
                Console.WriteLine("press any key");
                Console.ReadKey(true);
            }
        }
    }

    public class ExpiringDictionary<K, T> : IDictionary<K, T>
    {
        Timer timer;
        Dictionary<K, long> timeAdded;
        Dictionary<K, T> items;
        int msExpireInterval;

        public ExpiringDictionary(int msInterval, int msExpires)
        {
            items = new Dictionary<K, T>();
            timeAdded = new Dictionary<K, long>();

            msExpireInterval = msExpires;
            timer = new Timer(msInterval);
            timer.Elapsed += new ElapsedEventHandler(Elapsed_Event);
            timer.Start();
        }

        private void Elapsed_Event(object sender, ElapsedEventArgs e)
        {
            long expireTime = DateTime.Now.AddMilliseconds(-msExpireInterval).Ticks;
            List<K> removeUs = new List<K>();
            foreach (K key in items.Keys)
            {
                if (timeAdded[key] < expireTime)
                {
                    removeUs.Add(key);
                }
            }

            foreach (K key in removeUs)
            {
                timeAdded.Remove(key);
                items.Remove(key);
            }
        }

        public void Add(K key, T value)
        {
            items.Add(key, value);
            timeAdded.Add(key, DateTime.Now.Ticks);
        }

        public bool ContainsKey(K key)
        {
            return ContainsKey(key);
        }

        public ICollection<K> Keys
        {
            get { return items.Keys; }
        }

        public bool Remove(K key)
        {
            if (timeAdded.Remove(key))
            {
                items.Remove(key);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryGetValue(K key, out T value)
        {
            return items.TryGetValue(key, out value);
        }

        public ICollection<T> Values
        {
            get { return items.Values; }
        }

        public T this[K key]
        {
            get
            {
                return items[key];
            }
            set
            {
                if (items.ContainsKey(key))
                {
                    items.Remove(key);
                    timeAdded.Remove(key);
                }
                this.Add(key, value);
            }
        }

        public void Add(KeyValuePair<K, T> item)
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<K, T> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<K, T>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<K, T> item)
        {
            throw new NotImplementedException();
        }

        IEnumerator<KeyValuePair<K, T>> IEnumerable<KeyValuePair<K, T>>.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public void Clear()
        {
            items.Clear();
            timeAdded.Clear();
        }

        public int Count
        {
            get { return items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}
