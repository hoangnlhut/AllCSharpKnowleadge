using WiredBrainCoffee.DataProcessor.Data;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Processing
{
    public class MachineDataProcessor
    {
        private readonly ICoffeeCountStore _coffeeCountStore;

        public MachineDataProcessor(ICoffeeCountStore coffeeCountStore)
        {
            _coffeeCountStore = coffeeCountStore;
        }
        private readonly Dictionary<string, int> _countPerCoffeeType = new();
        private MachineDataItem? _previousItem;

        public void ProcessItems(MachineDataItem[] dataItems)
        {
            _countPerCoffeeType.Clear();
            _previousItem = null;

            foreach (var dataItem in dataItems)
            {
                ProcessItem(dataItem);
            }

            SaveCountPerCoffeeType();
        }

        private void ProcessItem(MachineDataItem dataItem)
        {
            // guards statements
            if (!CHeckItemIsNewer(dataItem)) return;

            if (!_countPerCoffeeType.ContainsKey(dataItem.CoffeeType))
            {
                _countPerCoffeeType.Add(dataItem.CoffeeType, 1);
            }
            else
            {
                _countPerCoffeeType[dataItem.CoffeeType]++;
            }
            _previousItem = dataItem;

        }

        private bool CHeckItemIsNewer(MachineDataItem dataItem)
        {
            return _previousItem == null || _previousItem.CreatedAt < dataItem.CreatedAt;
        }

        private void SaveCountPerCoffeeType()
        {
            foreach (var entry in _countPerCoffeeType)
            {
                _coffeeCountStore.Save(new CoffeeCountItem(entry.Key, entry.Value));
            }
        }
    }
}
