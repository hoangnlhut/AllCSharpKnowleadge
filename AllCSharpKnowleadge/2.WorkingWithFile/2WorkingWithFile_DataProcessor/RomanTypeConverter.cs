using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2WorkingWithFile_DataProcessor
{
    public class RomanTypeConverter : ITypeConverter
    {
        // This method is called when reading CSV data
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (text == "I") return 1;
            if (text == "II") return 2;
            if (text == "V") return 5;

            throw new ArgumentOutOfRangeException(nameof(text));
        }

        // This method is called when writing CSV data
        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            throw new NotImplementedException();
        }
    }
}
