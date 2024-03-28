using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiredBrainCoffee.DataProcessor.Data;
using WiredBrainCoffee.DataProcessor.Model;
using WiredBrainCoffee.DataProcessor.Processing;

namespace TestWireBrainCoffee.Processing
{
    public class MachineDataProcessorTests : IDisposable
    {
        private readonly FakeCoffeeCountStore coffeeCountStoreFake;
        private readonly MachineDataProcessor machineDataProcessor;

        public MachineDataProcessorTests()
        {
             coffeeCountStoreFake = new FakeCoffeeCountStore();
             machineDataProcessor = new MachineDataProcessor(coffeeCountStoreFake);
        }

        [Fact]
        public void ShouldSaveCountPerCoffeeType()
        {
            //Arrange
            var items = new[]
            {
                new MachineDataItem("Cappuchino", new DateTime(2022, 10,27,8,0,0)),
                new MachineDataItem("Cappuchino", new DateTime(2022, 10,27,9,0,0)),
                new MachineDataItem("Espreso", new DateTime(2022, 10,27,10,0,0)),
            };

            //Act
            machineDataProcessor.ProcessItems(items);

            #region Assert
            Assert.Equal(2, coffeeCountStoreFake.SaveItems.Count);

            var item = coffeeCountStoreFake.SaveItems[0];
            Assert.Equal("Cappuchino", item.CoffeeType);
            Assert.Equal(2, item.Count);

            item = coffeeCountStoreFake.SaveItems[1];
            Assert.Equal("Espreso", item.CoffeeType);
            Assert.Equal(1, item.Count);
            #endregion
        }

        [Fact]
        public void ShouldTakeNewerItems()
        {
            //Arrange
            var items = new[]
            {
                new MachineDataItem("Cappuchino", new DateTime(2022, 10,27,8,0,0)),
                new MachineDataItem("Cappuchino", new DateTime(2022, 10,26,8,0,0)),
                new MachineDataItem("Cappuchino", new DateTime(2022, 10,27,7,0,0)),
                new MachineDataItem("Cappuchino", new DateTime(2022, 10,27,9,0,0)),
                new MachineDataItem("Espreso", new DateTime(2022, 10,27,10,0,0)),
                new MachineDataItem("Espreso", new DateTime(2022, 10,26,10,0,0)),
                new MachineDataItem("Espreso", new DateTime(2022, 10,25,10,0,0)),
            };

            //Act
            machineDataProcessor.ProcessItems(items);

            #region Assert
            Assert.Equal(2, coffeeCountStoreFake.SaveItems.Count);

            var item = coffeeCountStoreFake.SaveItems[0];
            Assert.Equal("Cappuchino", item.CoffeeType);
            Assert.Equal(2, item.Count);

            item = coffeeCountStoreFake.SaveItems[1];
            Assert.Equal("Espreso", item.CoffeeType);
            Assert.Equal(1, item.Count);
            #endregion
        }

        [Fact]
        public void ShouldClearPreviousCoffeeCount()
        {
            //Arrange
            var items = new[]
            {
                new MachineDataItem("Cappuchino", new DateTime(2022, 10,27,8,0,0)),
            };

            //Act
            machineDataProcessor.ProcessItems(items);
            machineDataProcessor.ProcessItems(items);

            //Assert
            Assert.Equal(2, coffeeCountStoreFake.SaveItems.Count);
            foreach (var item in coffeeCountStoreFake.SaveItems)
            {
                Assert.Equal("Cappuchino"
                    , item.CoffeeType);
                Assert.Equal(1, item.Count);
            }
        }

        public void Dispose()
        {
            // this runs after every test

        }
    }

    public class FakeCoffeeCountStore() : ICoffeeCountStore
    {
        public List<CoffeeCountItem> SaveItems { get; } = new();
        public void Save(CoffeeCountItem item)
        {
            SaveItems.Add(item);
        }
    }
}
