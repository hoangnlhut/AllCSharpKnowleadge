using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiredBrainCoffee.DataProcessor.Data;
using WiredBrainCoffee.DataProcessor.Model;

namespace TestWireBrainCoffee.Data
{
    public class ConsoleCoffeeCountStoreTest
    {
        [Fact]
        public void ShouldWriteOutputToConsole()
        {
            //Arrage
            CoffeeCountItem item = new CoffeeCountItem("Cappuccino", 5);
            var stringWriter = new StringWriter();
            ConsoleCoffeeCountStore console = new ConsoleCoffeeCountStore(stringWriter);

            //Act
            console.Save(item);

            //Assert
            var result = stringWriter.ToString();
            Assert.Equal($"{item.CoffeeType}:{item.Count}{Environment.NewLine}", result);
        }
    }
}
