using System;

namespace BddTestingExample.Calculators
{
    public class BasicCalculator: ICalculator
    {
        public int? FirstNumber { get; set; }

        public int? SecondNumber { get; set; }

        public int Addition()
        {
            var total = FirstNumber.Value + SecondNumber.Value;
            FirstNumber = SecondNumber = null;
            return total;
        }

        public void MemoryAdd(int number)
        {
            if (!FirstNumber.HasValue)
                FirstNumber = number;
            else if (!SecondNumber.HasValue)
                SecondNumber = number;
            else
                throw new InvalidOperationException("Cannot add more than two number");
        }
    }
}