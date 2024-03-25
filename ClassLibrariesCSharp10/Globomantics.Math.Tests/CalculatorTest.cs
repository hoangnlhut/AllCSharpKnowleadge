using Globalmantics.Math;
using NUnit.Framework;

namespace Globomantics.Math.Tests
{
    public class CalculatorTest
    {

        [Test]
        public void TestForSum2IntNumber()
        {
            var result = Calculator.Add(11, 2);
            Assert.That(result, Is.EqualTo(13));
        }

        [Test]
        public void AsHexString()
        {
            string hex = Calculator.AsHex(255);
#if NET6_0
Assert.That(hex, Is.EqualTo("FF from .NET 6"));
#elif NET7_0
Assert.That(hex, Is.EqualTo("FF from .NET 7"));
#elif NET461
            Assert.That(hex, Is.EqualTo("FF from .NET Framework 4.6.1"));
#elif NETSTANDARD2_0
 Assert.That(hex, Is.EqualTo("FF from .NET Starndard 2.0"));
#else
     throw new System.PlatformNotSupportedException();
#endif

        }

        [Test]
        public void Internal() {
            var resule = Calculator.Internal();
            Assert.That(resule, Is.EqualTo("Internal Method"));

        }
    }
}