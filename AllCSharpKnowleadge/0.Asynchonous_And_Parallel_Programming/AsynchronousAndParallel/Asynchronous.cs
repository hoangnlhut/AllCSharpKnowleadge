using AsynchonousParallelProgramming.Asynchronous.Core;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace AsynchonousParallelProgramming.Asynchronous
{
    public class Asynchronous
    {
        private static string API_URL = "https://ps-async.fekberg.com/api/stocks";
        private Stopwatch stopwatch = new Stopwatch();
        CancellationTokenSource? cancellationTokenSource;
        private Random random = new Random();

        public  Task<IEnumerable<StockPrice>> Search_Click(string stockIdentifier, bool isCanceled)
        {
            if (cancellationTokenSource is not null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
                //return new Task<IEnumerable<StockPrice>>(() => { return new List<StockPrice>();  });
            }
            try
            {

                cancellationTokenSource = new();

                cancellationTokenSource.Token.Register(() =>
                {
                    Console.WriteLine("Token is Cancelled");
                });

                // multiple identifiers in stockIdentifier string
                var identifiers = stockIdentifier.Split(',', ' ');

                //cancel after 2 seconds
                if (isCanceled)
                {
                    cancellationTokenSource?.CancelAfter(200000);
                }

                return  HandingData(identifiers);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<StockPrice>> HandingData(string[] identifiers)
        {
            //1. Using Task.WhenAll or WhenAny
            //return  usingTaskWhenAllOrAny(identifiers);

            //2. Using ConcurrentBag
            //return  usingConcurrentBag(identifiers);

            //3. Using IAsyncIEnumrable<T>: Asynchronous streams allow us to work with streams of data(from web, db or disk or any other type of asynchronous operation, idea is to process items as they are arriving to the application in a better and more intuitive manner
            //return UsingIAsyncIEnumrable(identifiers);

            //4 using ThreadPool.QueueUserWorkItem
            return UsingThreadPoolQueueUserWorkItem(identifiers);
        }

        private Task<IEnumerable<StockPrice>> UsingThreadPoolQueueUserWorkItem(string[] identifiers)
        {
            //When we queue the work onto the Threadpool, we can't await that
            // need to introduce the TaskCompletionSoure to help us

            var tcs = new TaskCompletionSource<IEnumerable<StockPrice>>();

            ThreadPool.QueueUserWorkItem(_ =>
            {
                var lines = File.ReadAllLines("StockPrices_Small.csv");

                var prices = new List<StockPrice>();

                foreach (var line in lines.Skip(1))
                {
                    var stock = StockPrice.FromCSV(line);
                    if (identifiers.Contains(stock.Identifier))
                    {
                        prices.Add(stock);
                    }
                }

                //TODO: communicate the result of 'prices' ?
                tcs.SetResult(prices);
            });

            //TODO: return a Task<IEnumerable<StockPrice>>
            return tcs.Task;
        }

        private async Task<IEnumerable<StockPrice>>? UsingIAsyncIEnumrable(string[] identifiers)
        {
            var data = new List<StockPrice>();
            var dataStore = new DataStore();

            var enumarator = dataStore.GetStockPricesFromFileByIAsyncEnumarable(CancellationToken.None);

            await foreach (var item in enumarator
                .WithCancellation(CancellationToken.None) // you can implement cancelation on you own!
                )
            {
                if (identifiers.Contains(item.Identifier))
                {
                    Console.WriteLine($"Adding Stock: {item.Identifier} - TradeData: {item.TradeDate} - Change: {item.Change} ");
                    data.Add(item);
                }
            }

            return data;
        }
        
        // using ConcurrentBag:  Process Tasks as They Complete to collect data from many other task run parallel to avoid lost data on the way)
        private async Task<IEnumerable<StockPrice>> usingConcurrentBag(string[] identifiers)
        {
            var collectionAll = new ConcurrentBag<StockPrice>();

            foreach (var item in identifiers)
            {
                var loadTask = GetStocks(item, cancellationTokenSource);
                loadTask.ContinueWith((t) =>
                {

                    var result = t.Result;
                    foreach (var item in result)
                    {
                        collectionAll.Add(item);
                    }

                //TODONEXT: print collectionAll to UI Thread.This way to use to print data to UI whenever they get the data -> user not have to wait to get all data on the screen at one time->improve user experience
                });
            }
            
            return collectionAll;
        }

        private async Task<IEnumerable<StockPrice>> usingTaskWhenAllOrAny(string[] identifiers)
        {
            var loadingTask = new List<Task<IEnumerable<StockPrice>>>(); 
            foreach (var item in identifiers)
            {
                var loadTask = GetStocks(item, cancellationTokenSource);

                // you can use ConfigureAwait for faster but note that this action is not relate to main context, let await keyword now that it does not have to marshal  back to original context
                //await PrintGetIdentifier(item).ConfigureAwait(false);

                loadingTask.Add(loadTask); //use for Task.Whenall or Task.WhenAny
            }

            /////1. Using Task.WhenAll
            //task whenall first: use await to collect all data from multiple thread
            //var result1 = await Task.WhenAll(loadingTask);
            //return result1.SelectMany(x => x);

            /////2.Using Task.WhenAny
            var timeout = Task.Delay(15000);
            var result = Task.WhenAll(loadingTask);


            var taskWhenAny = await Task.WhenAny(timeout, result);
            if (taskWhenAny == timeout)
            {
                Console.WriteLine("Get Data too low");
                throw new OperationCanceledException("Timeout!");
            }

            return result.Result.SelectMany(x => x);
        }

        public Task<IEnumerable<StockPrice>>? GetStocks(string stockIdentifier, CancellationTokenSource cancellationTokenSource)
        {
            var store = new DataStore();
            Task<IEnumerable<StockPrice>> responseTask = null;
            //1. get Data from path
            //responseTask = store.GetStockPricesFromPath(stockIdentifier);

            //2. get Data from file
            responseTask = store.GetStockPricesFromFile(stockIdentifier, cancellationTokenSource);

            //3. get Data stock from mock using Task.FromResult()
            //responseTask = store.GetStockPricesFromMock(stockIdentifier, cancellationTokenSource);

            //4. get Data using IAsyncEnumarable

            return responseTask;
        }

        public Task PrintGetIdentifier(string stockIdentifier)
        {
            return Task.Run(() =>
            {
                Console.WriteLine("Process Other Operation of " + stockIdentifier + ".......");
            });
        }
    }
}
