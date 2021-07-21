using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadConsoleExample
{
    class LockPerformance
    {
        private static readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        private static int testValue = 0;
        const int maxValue = 10_000_000;
        static object _lock = new object();
        private static List<TestClass> dataList = new List<TestClass>(1_000);

        public static void Run()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            dataList.Clear();
            for (int i = 0; i < maxValue; i++)
            {
                lock (_lock)
                {
                    //testValue++;
                    TestClass tc = new TestClass();
                    tc.x = 123.45;
                    dataList.Add(tc);
                }
            }
            sw.Stop();
            double resultLock = sw.Elapsed.TotalMilliseconds;

            //sw.Restart();
            //dataList.Clear();
            //for (int i = 0; i < maxValue; i++)
            //{
            //    Interlocked.Increment(ref testValue);
            //}
            //sw.Stop();
            //double resultIncrement = sw.Elapsed.TotalMilliseconds;

            sw.Restart();
            dataList.Clear();
            for (int i = 0; i < maxValue; i++)
            {
                semaphoreSlim.Wait();
                //testValue++;
                TestClass tc = new TestClass();
                tc.x = 123.45;
                dataList.Add(tc);
                semaphoreSlim.Release();
            }
            sw.Stop();
            double resultSemaphore = sw.Elapsed.TotalMilliseconds;

            sw.Restart();
            dataList.Clear();
            for (int i = 0; i < maxValue; i++)
            {
                Monitor.Enter("");
                //testValue++;
                TestClass tc = new TestClass();
                tc.x = 123.45;
                dataList.Add(tc);
                Monitor.Exit("");
            }
            sw.Stop();
            double resultMonitor = sw.Elapsed.TotalMilliseconds;

            Console.WriteLine($"Lock = {resultLock}");
            //Console.WriteLine($"Interlocked = {resultIncrement}");
            Console.WriteLine($"Semaphore = {resultSemaphore}");
            Console.WriteLine($"Monitor = {resultMonitor}");
        }
    }
}
