using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadConsoleExample
{
    class Program
    {
        static int totalCounter = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Start execution...");

            LockPerformance.Run();            

            //LongOperation();

            //ThreadStart obj = new ThreadStart(LongOperation);
            //Thread thread1 = new Thread(obj);

            //Thread thread1 = new Thread(new ThreadStart(LongOperation));
            Thread thread1 = new Thread(() => LongOperation());
            Thread thread2 = new Thread(new ThreadStart(LongOperation));

            thread1.Priority = ThreadPriority.Lowest;
            thread1.Name = "thread1";
            thread1.IsBackground = true;
            
            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            Console.WriteLine($"Total counter = {totalCounter}");

            //Thread thread3 = new Thread(new ParameterizedThreadStart(LongOperationParams));
            //thread3.Start(10);

            //Thread thread4 = new Thread(() => LongOperation2Params(15, 750));
            //thread4.Start();

            // Get values from method running in thread
            //ResultCallbackDelegate resultCallbackDelegate = new ResultCallbackDelegate(ResultCallbackMethod);
            //SumHelper sumHelper = new SumHelper(100, resultCallbackDelegate);
            //Thread thread5 = new Thread(sumHelper.CalculationSum);
            //thread5.Start();

            Console.WriteLine("End execution");
            Console.ReadKey();

            if (thread1.IsAlive)
                thread1.Abort();
            if (thread2.IsAlive)
                thread2.Abort();

        }

        private static void ResultCallbackMethod(int result)
        {
            Console.WriteLine($"The result = {result}");
        }

        private static readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        static void LongOperation()
        {
            int threaId = Thread.CurrentThread.ManagedThreadId;
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"[{threaId}] counter: {i} ");
                Thread.Sleep(500);

                // semaphore based changing variable by the thread
                //semaphoreSlim.Wait();
                //totalCounter++;
                //semaphoreSlim.Release();

                //Monitor.Enter("");
                //totalCounter++;
                //Monitor.Exit("");

                Interlocked.Increment(ref totalCounter);
            }
        }

        static void LongOperationParams(object counter)
        {
            int intCounter = (int)counter;
            Console.WriteLine($"Counter value: {intCounter}");
            int threaId = Thread.CurrentThread.ManagedThreadId;
            for (int i = 0; i < intCounter; i++)
            {
                Console.WriteLine($"[{threaId}] counter: {i} ");
                Thread.Sleep(1000);
            }
        }

        static void LongOperation2Params(int counter, int delay)
        {
            int intCounter = (int)counter;
            Console.WriteLine($"Counter value: {intCounter}");
            int threaId = Thread.CurrentThread.ManagedThreadId;
            for (int i = 0; i < intCounter; i++)
            {
                Console.WriteLine($"[{threaId}] counter: {i} ");
                Thread.Sleep(delay);
            }
        }

    }
}
