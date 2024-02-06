using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsynchonousParallelProgramming.AsynchronousAndParallel
{
    public class AdvancedParallelProgramming
    {
        static object syncRoot = new();
        static Random random = new Random();

        //1. Synchronous Calculte
        public void Calculate()
        {
            decimal total = 0;
            for (int i = 0; i < 100; i++)
            {
                total += Compute(i);
            }
            Console.WriteLine($"Total Synchronous: {total}");
        }

        //2. Parallel.For With Lock or Interlock and apply Cancel Parallel Operational
        public void CalculateParallelFor()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(2000);

            var parallelOptions = new ParallelOptions()
            {
                CancellationToken = cancellationTokenSource.Token,
                MaxDegreeOfParallelism = 1
            };

            //decimal total = 0; //using lock object
            int total = 0; //using interlock

            try
            {
                Parallel.For(0, 100, parallelOptions, (i) =>
                {
                    var compute = Compute(i);
                    Interlocked.Add(ref total, (int)compute);
                });
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine("Cancellation");
            }
            Console.WriteLine($"Total ParallelFor: {total}");
        }

        static ThreadLocal<decimal?> threadLocal = new();

        //3. using ThreadLocal<T> with Parallel.For
        public void CalculateWithThreadLocal()
        {
            var options = new ParallelOptions() { MaxDegreeOfParallelism = 2 };

            Parallel.For(0,100, options, (i) =>
            {
                var currentValue = threadLocal.Value;
                threadLocal.Value = Compute(i);
            });

            var lastValue = threadLocal.Value;
        }

        static AsyncLocal<decimal?> asyncLocal = new();
        //4. using AsyncLocal<T> with Parallel.For
        public void CalculateWithAsyncLocal()
        {
            var options = new ParallelOptions() { MaxDegreeOfParallelism = 1 };

            asyncLocal.Value = 200;

            Parallel.For(0, 100, options, async (i) =>
            {
                var currentValue = asyncLocal.Value;
                asyncLocal.Value = Compute(i);
            });

            var lastValue = asyncLocal.Value;
        }


        //5. Using PLINQ
        public void CalculatePLinQ()
        {
            var result = Enumerable.Range(0, 100)
                .AsParallel() // ~ 0.5 seconds - without it -> >3 seconds
                .AsOrdered()
                .Select(Compute)
            //.Sum();
            .Take(10);

            //using foreach
            foreach (var item in result)
                {
                    Console.WriteLine(item);
                }

            //uing result For ALL: to run parallel
            result.ForAll(Console.WriteLine);

            Console.WriteLine($"Result : {result}");
        }

        private decimal Compute(int value)
        {
            var randomMiliseconds = random.Next(10, 50);
            var end = DateTime.Now + TimeSpan.FromMilliseconds(randomMiliseconds);

            while (end > DateTime.Now) { }

            return value + 0.5m;
        }
    }
}
