namespace Globalmantics.Math
{
    public static class Calculator
    {
        public static int Add(int a, int b) => a + b;
        public static string AsHex(int a)
        {
            var hex = a.ToString("X");

#if NET6_0
return $"{hex} from .NET 6";
#elif NET7_0
return $"{hex} from .NET 7";
#elif NET461
            return $"{hex} from .NET Framework 4.6.1";
#elif NETSTANDARD2_0
return $"{hex} from .NET Starndard 2.0";
#else
     throw new System.PlatformNotSupportedException();
#endif
        }

        public static bool IsWriteAsHexSupported
        {
            get
            {
#if NET6_0 ||  NET7_0 || NET461 || NETSTANDARD2_0 
                return true;
#else
                return false;
#endif
            }
        }
    }

   
}