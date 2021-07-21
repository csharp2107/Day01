using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PararellExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //LoopNoPararell();
            LoopWithPararell();
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
