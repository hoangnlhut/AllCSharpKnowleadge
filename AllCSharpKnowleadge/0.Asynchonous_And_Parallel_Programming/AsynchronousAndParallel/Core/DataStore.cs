using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsynchonousParallelProgramming.Asynchronous.Core
{
    public class DataStore
    {
        private string basePath { get; }

        public DataStore(string basePath = "")
        {
            this.basePath = basePath;
        }

        public async Task<IEnumerable<StockPrice>> GetStockPricesFromPath(string stockIdentifier)
        {
            var prices = new List<StockPrice>();
            var path = Path.Combine(basePath, @"StockPrices_Small.csv");
            using var stream =
                new StreamReader(File.OpenRead(path));

            await stream.ReadLineAsync(); // Skip the header how in the CSV

            while (await stream.ReadLineAsync() is string line)
            {
                #region Find & Parse Stock Price from CSV
                var segments = line.Split(',');

                for (var i = 0; i < segments.Length; i++) segments[i] = segments[i].Trim('\'', '"');

                if (segments[0].ToUpperInvariant()
                    != stockIdentifier.ToUpperInvariant())
                {
                    continue;
                }
                #endregion

                var price = new StockPrice
                {
                    Identifier = segments[0],
                    TradeDate = DateTime.ParseExact(segments[1], "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                    Volume = Convert.ToInt32(segments[6], CultureInfo.InvariantCulture),
                    Change = Convert.ToDecimal(segments[7], CultureInfo.InvariantCulture),
                    ChangePercent = Convert.ToDecimal(segments[8], CultureInfo.InvariantCulture),
                };

                prices.Add(price);
            }

            if (!prices.Any())
            {
                throw new KeyNotFoundException($"Could not find any stocks for {stockIdentifier}");
            }

            //await Task.Delay(5000);

            return prices;
        }

        public Task<IEnumerable<StockPrice>> GetStockPricesFromFile(string stockIdentifierNew, CancellationTokenSource cancellationTokenSource)
        {
            var token = cancellationTokenSource.Token;
            IEnumerable<StockPrice> result = new List<StockPrice>();
            //var loadLinesTask = Task.Run(() =>
            //{
            //    // if threre is an error occur here we can place try catch
            //    var lines = File.ReadAllLines("StockPrices_Small.csv");
            //    return lines;
            //})

            // or you can file read file async await from streamReader
            var loadLinesTask = Task.Run(async () =>
            {
                var lines = new List<string>();

                // if you use try catch here to catch exception -> after print error it still complete and run continuely below Continue With
                // if you don't use try catch here --> the status is cancel -> it will ignore below ContinueWith.OnlyOnRanToCompletion and run below ContinueWith()
                //try
                //{
                // asynchronous opetation
                using var stream = new StreamReader(File.OpenRead("StockPrices_Small.csv"));
                while (await stream.ReadLineAsync() is string line)
                {
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                    lines.Add(line);
                }
                //}
                //catch (Exception ex )
                //{
                //    Console.WriteLine("Error read file " + ex.ToString() );
                //}

                return lines;
            }, token);

            loadLinesTask.ContinueWith((completedTask) =>
          {
              Console.WriteLine("Failed in read file csv");
              Console.WriteLine(completedTask.Exception?.Message);
          }, TaskContinuationOptions.OnlyOnFaulted);


            var processStockTask = loadLinesTask.ContinueWith((completedTask) =>
             {
                 var data = new List<StockPrice>();
                 //if (completedTask.IsCompletedSuccessfully)
                 //{
                     var result = completedTask.Result;

                     foreach (var line in result.Skip(1)) // remove header
                     {
                         var price = StockPrice.FromCSV(line.ToString());
                         data.Add(price);
                     }
                 //}
                

                 return data.Where(x => x.Identifier == stockIdentifierNew);
             }, TaskContinuationOptions.OnlyOnRanToCompletion); // only run when antecedent completion , it has not exeptions and not cancelled

            processStockTask
                .ContinueWith((completedTask) =>
           {
               Console.WriteLine("Last Continuation");
               if (completedTask.IsCompletedSuccessfully)
               {
                   result = completedTask.Result;
               }
               cancellationTokenSource?.Dispose();
               cancellationTokenSource = null;

               return result;
           });

            ///// you can chain many task like this and it will separate thread
            //continuationTask.ContinueWith(t => { })
            //                .ContinueWith(t => { })
            //                .ContinueWith(t => { });

            return  processStockTask;
        }

        public Task<IEnumerable<StockPrice>> GetStockPricesFromMock(string stockIdentifierNew, CancellationTokenSource cancellationTokenSource)
        {
            var stocks = new List<StockPrice>()
            {
                new(){ Identifier = "MSFT", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "MSFT", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "MSFT", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "MSFT", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "MSFT", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "MSFT", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "MSFT", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "MSFT", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "MSFT", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "MSFT", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "GOOGL", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "GOOGL", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "GOOGL", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "GOOGL", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "GOOGL", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "GOOGL", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "GOOGL", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "GOOGL", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "GOOGL", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "GOOGL", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "CAT", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "CAT", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "CAT", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "CAT", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "CAT", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "CAT", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "CAT", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "CAT", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "CAT", Change = 0.5m, ChangePercent = 0.75m},
                new(){ Identifier = "CAT", Change = 0.5m, ChangePercent = 0.75m},
            };

            return Task.FromResult(stocks.Where(x => x.Identifier == stockIdentifierNew));
        }

        public async IAsyncEnumerable<StockPrice> GetStockPricesIAsyncEnumarable(string stockIdentifierNew)
        {
            await Task.Delay(1000);
            yield return new StockPrice { Identifier = "MSFT", Change = 0.5m };
            await Task.Delay(1000);
            yield return new StockPrice { Identifier = "MSFT", Change = 0.5m };
            await Task.Delay(1000);
            yield return new StockPrice { Identifier = "MSFT", Change = 0.5m };
            await Task.Delay(1000);
            yield return new StockPrice { Identifier = "MSFT", Change = 0.5m };
            await Task.Delay(1000);
            yield return new StockPrice { Identifier = "MSFT", Change = 0.5m };
            await Task.Delay(1000);
            yield return new StockPrice { Identifier = "MSFT", Change = 0.5m };
            await Task.Delay(1000);
            yield return new StockPrice { Identifier = "MSFT", Change = 0.5m };
            await Task.Delay(1000);
            yield return new StockPrice { Identifier = "MSFT", Change = 0.5m };
            await Task.Delay(1000);
            yield return new StockPrice { Identifier = "MSFT", Change = 0.5m };
            await Task.Delay(1000);
            yield return new StockPrice { Identifier = "MSFT", Change = 0.5m };
            await Task.Delay(1000);
            yield return new StockPrice { Identifier = "MSFT", Change = 0.5m };
        }

        public async IAsyncEnumerable<StockPrice> GetStockPricesFromFileByIAsyncEnumarable([EnumeratorCancellation]CancellationToken cancellationToken = default)
        {
            using var stream = new StreamReader(File.OpenRead("StockPrices_Small.csv"));

            await stream.ReadLineAsync(); //Skip header row in the file.

            while(await stream.ReadLineAsync() is string line) {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                yield return StockPrice.FromCSV(line);
            }
        }
    }
}
