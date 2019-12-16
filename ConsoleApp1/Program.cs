using Infrastructure.Lock;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{

    public class StartUp
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseMiddleware<responseMiddleWares>();
            app.Use(async (context, next) =>
            {
                Console.WriteLine("A (in)");
                await next();
                Console.WriteLine("A (out)");
            });

            app.UseWhen(
                context => context.Request.Path.StartsWithSegments(new PathString("/foo")),
                builder => builder.Use(async (context, next) =>
                   {
                       Console.WriteLine("B (in)");
                       await next.Invoke();
                       Console.WriteLine("B (out)");
                   })
                )
            .Run(async context =>
            {
                Console.WriteLine("C");
                await context.Response.WriteAsync("hello world from the terminal middleware");
            });
        }
    }
    public class responseMiddleWares
    {
        private RequestDelegate _nextDeleage { get; set; }

        public responseMiddleWares(RequestDelegate requestDelegate)
        {
            _nextDeleage = requestDelegate;
        }
        //requestMiddleWares，先注册=>先request，最后response
        //并且这个Middleware必须放在第一位，这样才能确保这Middleware处理的是最后一个Repsonse，因为Repsonse的处理顺序跟Request的处理顺序是相反的。
        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (_nextDeleage == null)
            {
                throw new ArgumentNullException(nameof(_nextDeleage));
            }
            await _nextDeleage.Invoke(httpContext);//先进行其他的中间件

            if (!httpContext.Request.IsHttps)
            {
                await httpContext.Response.WriteAsync("http is not safe", System.Text.Encoding.UTF8);
            }
            else
            {
                await httpContext.Response.WriteAsync(" https is safe", System.Text.Encoding.UTF8);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //new Program().TestReadWriteLock();
            CreateHostBuilder(args).Build().Run(); ;
        }
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webbuilder =>
            {
                webbuilder.UseStartup<StartUp>();
            });

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
