using System.Collections.Generic;
using System.Linq;

namespace BddTestingExample.Calculators
{
    public class AdvancedCalculator : ICalculator
    {
        public List<int> Numbers { get; set; }

        public AdvancedCalculator()
        {
            Numbers = new List<int>();
        }

        public int Addition()
        {
            var total = Numbers.Sum();
            Numbers.Clear();
            return total;
        }

        public void MemoryAdd(int number)
        {
            Numbers.Add(number);
        }
    }
}