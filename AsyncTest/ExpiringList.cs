using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;


namespace AsyncTest
{
    class ExpiringListTest
    {
        private static bool PromptAfter = true;

        static void Main(string[] args)
        {
            Console.WriteLine("Main start");
            ExpiringList<string> mylist = new ExpiringList<string>(100, 2000);

            for (int i = 1; i < 10; i++)
            {
                mylist.Add(string.Format("This was added at {0}", DateTime.Now.ToString("mm ss.ff")));
                System.Threading.Thread.Sleep(500);

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("There are {0} items on the list: ", mylist.Count);
                foreach (string s in mylist)
                    sb.AppendFormat("\r\n\t{0}", s);
                Debug.WriteLine(sb);
            }

            if (PromptAfter)
            {
                Console.WriteLine("press any key");
                Console.ReadKey(true);
            }
        }
    }

    public class ExpiringList<T> : IList<T>
    {
        Timer timer;
        List<long> timeAdded;
        List<T> items;
        int msExpireInterval;

        public ExpiringList(int msInterval, int msExpires)
        {
            items = new List<T>();
            timeAdded = new List<long>();

            msExpireInterval = msExpires;
            timer = new Timer(msInterval);
            timer.Elapsed += new ElapsedEventHandler(Elapsed_Event);
            timer.Start();
        }

        private void Elapsed_Event(object sender, ElapsedEventArgs e)
        {
            long expireTime = DateTime.Now.AddMilliseconds(-msExpireInterval).Ticks;
            for (int i = items.Count - 1; i > 0; i--)
            {
                //Debug.WriteLine(timeAdded[i] - expireTime);
                if (timeAdded[i] < expireTime)
                {
                    timeAdded.RemoveAt(i);
                    items.RemoveAt(i);
                }
            }

        }



        public int IndexOf(T item)
        {
            return items.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            items.Insert(index, item);
            timeAdded.Insert(index, DateTime.Now.Ticks);
        }

        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
            timeAdded.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                return items[index];
            }
            set
            {
                items[index] = value;
            }
        }

        public void Add(T item)
        {
            items.Add(item);
            timeAdded.Add(DateTime.Now.Ticks);
        }

        public void Clear()
        {
            items.Clear();
            timeAdded.Clear();
        }

        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        /// <summary>
        /// note: will copy without times
        /// </summary>
        public void CopyTo(T[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            //we'd have to find first, then remove.
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}
