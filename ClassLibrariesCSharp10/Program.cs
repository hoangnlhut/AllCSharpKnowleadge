using Globalmantics.Math;

namespace ClassLibrariesCSharp10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Globomantics calculator!");
            Console.WriteLine();

            // Input validation omitted to keep demo code simple
            Console.WriteLine("Please enter your first integer number");
            int number1 = int.Parse(Console.ReadLine()!);

            Console.WriteLine("Please enter an operation");
            string? operation = Console.ReadLine();

            Console.WriteLine("Please enter your second integer number");
            int number2 = int.Parse(Console.ReadLine()!);

            if (operation == "+")
            {
                var result = Calculator.Add(number1, number2);
                Console.WriteLine($"The result is: {result}");
            }
            else
            {
                Console.WriteLine($"The operation '{operation}' is not currently supported.");
            }

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}



