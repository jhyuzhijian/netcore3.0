using Infrastructure.Lock;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().TestReadWriteLock();
            Console.ReadLine();
        }
        class MyQueue : IEnumerable<string>
        {
            List<string> _List;
            UsingLock<object> _Lock;
            public MyQueue(IEnumerable<string> strings)
            {
                _List = new List<string>(strings);
                _Lock = new UsingLock<object>();
            }
            //获取第一个元素并从集合中移除
            public string LootFirst()
            {
                Console.WriteLine("当前线程:" + Thread.CurrentThread.ManagedThreadId);
                using (_Lock.Write())
                {
                    if (_List.Count() == 0)
                    {
                        _Lock.Enabled = false;
                        return null;
                    }
                    var s = _List[0];
                    _List.RemoveAt(0);
                    Console.WriteLine("进入当前写锁的线程:" + Thread.CurrentThread.ManagedThreadId);
                    return s;
                }
            }
            public int Count { get { return _List.Count; } }

            public IEnumerator<string> GetEnumerator()
            {
                //Console.WriteLine("当前线程:" + Thread.CurrentThread.ManagedThreadId);
                using (_Lock.Read())
                {
                    //Console.WriteLine("进入读锁的线程:" + Thread.CurrentThread.ManagedThreadId);
                    foreach (var item in _List)
                    {
                        yield return item;
                    }
                }
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        public void TestReadWriteLock()
        {

            try
            {
                Stopwatch stop = new Stopwatch();
                stop.Start();
                List<string> list = new List<string>(100);
                for (int i = 0; i < list.Capacity; i++)
                {
                    list.Add("字符串:" + i);
                }
                MyQueue mq = new MyQueue(list);
                string last = list[list.Count - 1];
                for (int i = 0; i < list.Capacity; i++)
                {
                    ThreadPool.QueueUserWorkItem(o =>
                    {
                        mq.LootFirst();
                        //Console.WriteLine(mq.LootFirst());
                    });
                }
                while (mq.Count > 0)
                {
                    ThreadPool.QueueUserWorkItem(o =>
                    {
                        foreach (var item in mq)
                        {
                            if (item == last)
                            {
                                Console.WriteLine("最后的值还在");
                            }
                        }
                    });
                }
                stop.Stop();
                Console.WriteLine("花费的时间:" + stop.ElapsedMilliseconds);
            }
            catch (Exception ex)
            { }
        }
    }
}
