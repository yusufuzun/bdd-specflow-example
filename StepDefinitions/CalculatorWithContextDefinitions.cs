using BddTestingExample.Calculators;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace BddTestingExample.StepDefinitions
{
    [Binding]
    [Scope(Feature = "Calculator Addition With Context")]
    public sealed class CalculatorWithContextDefinitions
    {

        [Given("I have a new \"(.*)\"")]
        public void GivenIHaveANewBasicCalculator(string calculatorName)
        {
            if (calculatorName == "Basic Calculator")
            {
                var instance = new BasicCalculator(); //we let this to IoC container
                ScenarioContext.Current.ScenarioContainer.RegisterInstanceAs(instance, typeof(ICalculator), calculatorName);
            }
            if (calculatorName == "Advanced Calculator")
            {
                var instance = new AdvancedCalculator(); //we let this to IoC container
                ScenarioContext.Current.ScenarioContainer.RegisterInstanceAs(instance, typeof(ICalculator), calculatorName);
            }
        }

        [Given("I have entered (.*) into the \"(.*)\"")]
        public void GivenIHaveEnteredSomethingIntoTheCalculator(int number, string calculatorName)
        {
            var calculator = ScenarioContext.Current.ScenarioContainer.Resolve<ICalculator>(calculatorName);
            calculator.MemoryAdd(number);
        }

        [When("I press add in \"(.*)\" and see \"(.*)\"")]
        public void WhenIPressAdd(string calculatorName, string resultName)
        {
            var calculator = ScenarioContext.Current.ScenarioContainer.Resolve<ICalculator>(calculatorName);
            var additionResult = new AdditionTestResult
            {
                Result = calculator.Addition()
            };
            ScenarioContext.Current.ScenarioContainer.RegisterInstanceAs(additionResult, typeof(AdditionTestResult), resultName);
        }

        [Then("the \"(.*)\" should be (.*) on the screen")]
        public void ThenTheResultShouldBe(string resultName, int expectedResult)
        {
            var actualResult = ScenarioContext.Current.ScenarioContainer.Resolve<AdditionTestResult>(resultName);
            Assert.AreEqual(expectedResult, actualResult.Result);
        }

        public class AdditionTestResult
        {
            public int Result { get; set; }
        }
    }

}
