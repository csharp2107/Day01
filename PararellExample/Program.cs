using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PararellExample
{
    class Program
    {

        static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        static void Main(string[] args)
        {
            //LoopNoPararell();
            //LoopWithPararell();
            //LoopWithPararellForEach();
            //LoopWithPararellBreakStop();
            PararellInvoke();

           // Thread t1 = new Thread(() =>
           //{
           //    Thread.Sleep(2500);
           //    cancellationTokenSource.Cancel();
           //});
           // t1.Start();
           // LoopWithPararellCancel();
        }

        public static void LoopNoPararell()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 10; i++)
            {
                long total = LongOperation();
                Console.WriteLine("{0} - {1}", i, total);
            }
            sw.Stop();
            Console.WriteLine("LoopNoPararell - {0}", sw.ElapsedMilliseconds);
            Console.ReadKey();
        }

        public static void LoopWithPararellCancel()
        {
            ParallelOptions options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 4, 
                CancellationToken = cancellationTokenSource.Token
            };

            try
            {
                Parallel.For(0, 15, options, i =>
                {
                    long total = LongOperation();
                    Console.WriteLine("{0} - {1}", i, total);
                    Thread.Sleep(1000);
                });
            } catch (OperationCanceledException exc)
            {
                Console.WriteLine(exc.Message);
            }
            Console.ReadKey();
        }

        static void PararellInvoke()
        {
            ParallelOptions parallelOptions = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 4
            };
            Parallel.Invoke(parallelOptions,
                () => TestTask(1),
                () => TestTask(2),
                () => TestTask(3),
                () => TestTask(4),
                () => TestTask(5)
               );
        }

        static Random rnd = new Random();
        static void TestTask(int nr)
        {
            Console.WriteLine($"[{nr}] Task start");
            Thread.Sleep(rnd.Next(1500, 3000));
            Console.WriteLine($"[{nr}] Task end");
        }

        public static void LoopWithPararellBreakStop()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ParallelOptions options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 2
            };

            Parallel.For(0, 10, options, (i , loopState) => {
                if (i >= 5)
                    loopState.Stop();
                long total = LongOperation();
                Console.WriteLine("{0} - {1}", i, total);
                
            });

            sw.Stop();
            Console.WriteLine("LoopWithPararell - {0}", sw.ElapsedMilliseconds);
            Console.ReadKey();
        }

        public static void LoopWithPararell()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ParallelOptions options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 8
            };
            //Console.WriteLine($"{options.MaxDegreeOfParallelism}");
            //options.MaxDegreeOfParallelism = 2;

            Parallel.For(0, 10, options, i => {
                long total = LongOperation();
                Console.WriteLine("{0} - {1}", i, total);
            });

            //for (int i = 0; i < 10; i++)
            //{
            //    long total = LongOperation();
            //    Console.WriteLine("{0} - {1}", i, total);
            //}
            sw.Stop();
            Console.WriteLine("LoopWithPararell - {0}", sw.ElapsedMilliseconds);
            Console.ReadKey();
        }


        public static void LoopWithPararellForEach()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ParallelOptions options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 4
            };

            List<int> integerList = Enumerable.Range(0, 20).ToList();
            Parallel.ForEach(integerList, options, i => {
                long total = LongOperation();
                Console.WriteLine("{0} - {1}", i, total);
            });
            sw.Stop();
            Console.WriteLine("LoopWithPararellForEach - {0}", sw.ElapsedMilliseconds);

            sw.Restart();
            foreach (var i in integerList)
            {
                long total = LongOperation();
                Console.WriteLine("{0} - {1}", i, total);
            }
            sw.Stop();
            Console.WriteLine("LoopWithoutPararellForEach - {0}", sw.ElapsedMilliseconds);

            Console.ReadKey();
        }

        public static long LongOperation()
        {
            long total = 0;
            for (int i = 0; i < 100_000_000; i++)
            {
                total += i;
            }
            return total;
        }
    }
}
