using AsynchonousParallelProgramming.Asynchronous.Core;
using System;
using System.Collections.Concurrent;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AsynchonousParallelProgramming.AsynchronousAndParallel
{
    public class ParallelProgram
    {
        private static string API_URL = "https://ps-async.fekberg.com/api/stocks";
        private Stopwatch stopwatch = new Stopwatch();
        CancellationTokenSource? cancellationTokenSource;
        private Random random = new Random();

        public Task<IEnumerable<StockCalculation>> Search_Click()
        {
            Task<IEnumerable<StockCalculation>> getdata = null;

            var stocks = new Dictionary<string, IEnumerable<StockPrice>>
            {
                { "MSFT", Generate("MSFT")},
                { "GOOGL", Generate("GOOGL")},
                { "AAPL", Generate("AAPL")},
                { "CAT", Generate("CAT")},
                { "BBBB", Generate("BBBB")},
                { "CCC", Generate("CCC")},
                { "DDDDD", Generate("DDDDD")},
                { "EEEEE", Generate("EEEEE")},
                { "FFFF", Generate("FFFF")},
                { "GGGG", Generate("GGGG")},
                { "HHHH", Generate("HHHH")},
                { "GGGGDDD", Generate("GGGGDDD")},
                { "ZZZZZZZ", Generate("ZZZZZZZ")},
            };

            #region 1. Run synchonize 10~
            //getdata = CalculateSynchronize(stocks);
            #endregion

            #region 2.Using Task.Run() not realy faster than 1.
            //getdata = CalculateSynchronize(stocks);
            #endregion

            #region 3.Using Parallel.Invoke
            //getdata = CalculateParallelInvoke(stocks);
            #endregion

            #region 4.Using Parallel.Invoke and wraps in Task.Run() won't be blocking thread
            //getdata = CalculateParallelInvokeAndAsync(stocks);
            #endregion

            #region 5.Using Parallel.ForEach()
            getdata = CalculateParallelForAndForEach(stocks);
            #endregion

            return getdata;
        }

        //1 .Run synchonize 10~
        private StockCalculation[] CalculateSynchronize(Dictionary<string, IEnumerable<StockPrice>> stocks)
        {
            var msft = Calculate(stocks["MSFT"]);
            var googl = Calculate(stocks["GOOGL"]);
            var aapl = Calculate(stocks["AAPL"]);
            var cat = Calculate(stocks["CAT"]);

            return new[] { msft, googl, aapl, cat };
        }


        //2 .Using Task.Run() not realy faster than 1.
        private Task<IEnumerable<StockCalculation>> CalculateTaskRun(Dictionary<string, IEnumerable<StockPrice>> stocks)
        {
            return Task.Run(() =>
            {
                var msft = Calculate(stocks["MSFT"]);
                var googl = Calculate(stocks["GOOGL"]);
                var aapl = Calculate(stocks["AAPL"]);
                var cat = Calculate(stocks["CAT"]);

                return (new[] { msft, googl, aapl, cat }).AsEnumerable();
            });
        }

        //3 .Using Parallel.Invoke(): faster 2 and 1 in 2 seconds
        private IEnumerable<StockCalculation> CalculateParallelInvoke(Dictionary<string, IEnumerable<StockPrice>> stocks)
        {
            var bag = new ConcurrentBag<StockCalculation>();

            Parallel.Invoke(new ParallelOptions
            {
                CancellationToken = CancellationToken.None,
                //MaxDegreeOfParallelism = 2,
            },
                () =>
                {
                    var msft = Calculate(stocks["MSFT"]);
                    bag.Add(msft);
                },
                () =>
                {
                    var msft = Calculate(stocks["GOOGL"]);
                    bag.Add(msft);
                },
                () =>
                {
                    var msft = Calculate(stocks["AAPL"]);
                    bag.Add(msft);
                },
                () =>
                {
                    var msft = Calculate(stocks["CAT"]);
                    bag.Add(msft);
                }
                );

            return bag;
        }

        //4. .Using Parallel.Invoke() and wraps in Task.Run() to run asynchronous
        private async Task<IEnumerable<StockCalculation>> CalculateParallelInvokeAndAsync(Dictionary<string, IEnumerable<StockPrice>> stocks)
        {
            var bag = new ConcurrentBag<StockCalculation>();
            // calling thread won't be block
            await Task.Run(() =>
            {
                try
                {
                    Parallel.Invoke(
                    //new ParallelOptions    --> optional
                    //{
                    //    //CancellationToken = CancellationToken.None,
                    //    //MaxDegreeOfParallelism = 2,
                    //},
                       () =>
                       {
                           var msft = Calculate(stocks["MSFT"]);
                           bag.Add(msft);
                           throw new Exception("MSFT"); // Throw exception to catch in Paralle.Invoke
                       },
                       () =>
                       {
                           var msft = Calculate(stocks["GOOGL"]);
                           bag.Add(msft);
                       },
                       () =>
                       {
                           var msft = Calculate(stocks["AAPL"]);
                           bag.Add(msft);
                       },
                       () =>
                       {
                           var msft = Calculate(stocks["CAT"]);
                           bag.Add(msft);
                       }
                       );
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
            return bag;
        }

        //5 Using Paralle.For or Parallel.ForEach()
        //3 .Using Parallel.Invoke(): faster 2 and 1 in 2 seconds
        private async Task<IEnumerable<StockCalculation>> CalculateParallelForAndForEach(Dictionary<string, IEnumerable<StockPrice>> stocks)
        {
            var bag = new ConcurrentBag<StockCalculation>();

            //await Task.Run(() =>      // only run from 1 -> 50
            //{
            //    Parallel.For(0, 100, (i, state) =>
            //    {
            //        if (i == 50)
            //        {
            //            state.Break();
            //        }
            //    Console.WriteLine($"{i}: print");
            //    });
            //});

            await Task.Run(() =>
            {
            try
            {
                Parallel.ForEach(stocks, (item, state) =>
                {
                    //USING BREAK
                    if (item.Key == "EEEEE" || item.Key == "GGGG" || item.Key == "FFFF" || 
                    state.ShouldExitCurrentIteration) // if the previous iteration requested a break, it will be set to true
                        {
                            state.Break(); // if you want to break for each with condition
                        }
                        else
                        {
                            var value = Calculate(item.Value);
                            bag.Add(value);
                        }

                        //USING STOP or you can use stop to stop operation
                        //if (item.Key == "AAPL")
                        //{
                        //    state.Stop();
                        //}
                        //var value = Calculate(item.Value);
                        //bag.Add(value);
                    });
                }
                catch (Exception ex)
                {
                    throw;
                }
            });

            return bag;
        }

        private StockCalculation Calculate(IEnumerable<StockPrice> prices)
        {
            #region Start stopwatch
            var calculation = new StockCalculation();
            var watch = new Stopwatch();
            watch.Start();
            #endregion

            var end = DateTime.UtcNow.AddSeconds(2);

            // Spin a loop for a few seconds to simulate load
            while (DateTime.UtcNow < end)
            { }

            #region Return a result
            calculation.Identifier = prices.First().Identifier;
            calculation.Result = prices.Average(s => s.Open);

            watch.Stop();

            calculation.TotalSeconds = watch.Elapsed.Seconds;

            return calculation;
            #endregion
        }

        private IEnumerable<StockPrice> Generate(string stockIdentifier)
        {

            return Enumerable.Range(1, random.Next(10, 250))
                .Select(x => new StockPrice
                {
                    Identifier = stockIdentifier,
                    Open = random.Next(10, 1024)
                });
        }
    }
}
