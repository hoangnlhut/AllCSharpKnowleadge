using CsvHelper.Configuration;
using System.Globalization;

namespace _2WorkingWithFile_DataProcessor.Model
{
    public class ProcessedOrderMap : ClassMap<ProcessedOrder>
    {
        public ProcessedOrderMap()
        {
            // you don't need to re-define the same column like that
            //Map(po => po.OrderNumber).Name("OrderNumber"); 
            // instead you should use this way to automatic map same column
            AutoMap(CultureInfo.InvariantCulture);
            Map(po => po.Customer).Name("CustomerNumber");
            Map(po => po.Amount).Name("Quantity").TypeConverter<RomanTypeConverter>() ;
        }
    }
}