using BddTestingExample.Calculators;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace BddTestingExample.StepDefinitions
{
    [Binding]
    [Scope(Feature = "Calculator Addition Without Context")]
    public sealed class CalculatorWithoutContextDefinition
    {
        private readonly ICalculator calculator = new BasicCalculator();
        private int actualResult;

        [Given("I have entered (.*) into the calculator")]
        public void GivenIHaveEnteredSomethingIntoTheCalculator(int number)
        {
            calculator.MemoryAdd(number);
        }

        [When("I press add")]
        public void WhenIPressAdd()
        {
            actualResult = calculator.Addition();
        }

        [Then("the result should be (.*) on the screen")]
        public void ThenTheResultShouldBe(int expectedResult)
        {
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
