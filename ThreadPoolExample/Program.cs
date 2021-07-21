using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPoolExample
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool.SetMaxThreads(1, 1);
            ThreadPool.SetMinThreads(1, 1);
            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(MyCustomMethod));
            }
            Console.ReadKey();

            Stopwatch sw = new Stopwatch();
            sw.Restart();
            MethodWithThread();
            sw.Stop();
            Console.WriteLine($"MethodWithThread - {sw.ElapsedMilliseconds}");

            sw.Restart();
            MethodWithThreadPool();
            sw.Stop();
            Console.WriteLine($"MethodWithThreadPool - {sw.ElapsedMilliseconds}");

            Console.ReadKey();
        }


        public static void MethodWithThread()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(Test);
                thread.Start();
            }
        }

        public static void MethodWithThreadPool()
        {
            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(Test));
            }
        }

        public static void Test(object obj)
        {

        }

        public static void MyCustomMethod(object obj)
        {
            Thread thread = Thread.CurrentThread;
            string msg = $"Thread Pool: {thread.IsThreadPoolThread}, ThreadID: {thread.ManagedThreadId}";
            Console.WriteLine(msg);
            Thread.Sleep(1000);
        }
    }
}
