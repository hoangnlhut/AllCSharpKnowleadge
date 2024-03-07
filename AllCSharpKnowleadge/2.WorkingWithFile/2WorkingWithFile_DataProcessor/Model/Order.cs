namespace _2WorkingWithFile_DataProcessor.Model
{
    public class Order
    {
        public int OrderNumber { get; set; }
        public int CustomerNumber { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
    }
}