using System.Diagnostics;
using AsynchonousParallelProgramming.Asynchronous;
using AsynchonousParallelProgramming.AsynchronousAndParallel;
using Microsoft.VisualBasic;

namespace AsynchonousParallelProgramming
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //Thread.Sleep(5000);

            #region TestAttachAndDetachTask
            //Console.WriteLine("Starting");

            //await Task.Factory.StartNew(() =>
            //{
            //    Task.Factory.StartNew(() =>
            //    {
            //        Task.Delay(1000);
            //        Console.WriteLine("Complete 1");
            //    }, TaskCreationOptions.AttachedToParent); // attach to parent

            //    Task.Factory.StartNew(() =>
            //    {
            //        Task.Delay(1000);
            //        Console.WriteLine("Complete 2");
            //    });

            //    Task.Factory.StartNew(() =>
            //    {
            //        Task.Delay(1000);
            //        Console.WriteLine("Complete 3");
            //    });
            //});

            //Console.WriteLine("Complete Final");
            #endregion EndTestAttachAndDetachTask


            //Console.WriteLine("Please input stockIdentifier:");
            //var stockIdentifier = Console.ReadLine();

            #region Asynchronous: using Task<Result> async await --------------------------
            //var async = new AsynchonousParallelProgramming.Asynchronous.Asynchronous();
            //Console.WriteLine("1. Start Running async await");

            ////Console.WriteLine("Do you want cancel in 5 seconds?");
            ////var isCancel = Console.ReadLine();

            //var getAllStock = await async.Search_Click(stockIdentifier, true);

            //Console.WriteLine("2. End Running async await");
            #endregion ------------------------------------------------------


            #region Parrallel Programming --------------------------
            //var async = new AsynchonousParallelProgramming.AsynchronousAndParallel.ParallelProgram();
            //Console.WriteLine("1. Start Running parallel");

            //try
            //{
            //    //var getAllStock = async.Search_Click(); using 1,2,3
            //    var getAllStock = await async.Search_Click(); // using scenerio 4.

            //    Console.WriteLine("2. End Running parallel");


            //    Console.WriteLine($"Total Number of AllStock: {getAllStock.Count()}");
            //    foreach (var item in getAllStock)
            //    {
            //        Console.WriteLine(item.ToString());
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            #endregion ------------------------------------------------------


            #region advanced Parallel Programming: Locking and Shared Variables.
            var advanced = new AdvancedParallelProgramming();

            //advanced.Calculate();
            //advanced.CalculateParallelFor();
            //advanced.CalculateWithThreadLocal();
            //advanced.CalculateWithAsyncLocal();
            advanced.CalculatePLinQ();

            #endregion

            //Console.WriteLine($"Loaded stocks for {stockIdentifier} in {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"Loaded stocks  in {stopwatch.ElapsedMilliseconds}ms");

        }


    }
}