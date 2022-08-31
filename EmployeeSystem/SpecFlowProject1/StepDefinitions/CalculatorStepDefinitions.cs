using NUnit.Framework;

namespace SpecFlowProject1.StepDefinitions
{
    [Binding]
    public sealed class CalculatorStepDefinitions
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef
        int fnum;
        int snum;
        int result;
        [Given("the first number is (.*)")]
        public void GivenTheFirstNumberIs(int number)
        {
            //TODO: implement arrange (precondition) logic
            // For storing and retrieving scenario-specific data see https://go.specflow.org/doc-sharingdata
            // To use the multiline text or the table argument of the scenario,
            // additional string/Table parameters can be defined on the step definition
            // method. 
            fnum = number;
            Console.WriteLine("First Number : "+fnum);
        }

        [Given("the second number is (.*)")]
        public void GivenTheSecondNumberIs(int number)
        {
            //TODO: implement arrange (precondition) logic

            //throw new PendingStepException();
            snum= number;
            Console.WriteLine("Second Number: " + snum);
        }

        [When("the two numbers are added")]
        public void WhenTheTwoNumbersAreAdded()
        {
            //TODO: implement act (action) logic
            result = fnum + snum;
            Console.WriteLine("Addition ");
        }

        [Then("the result should be (.*)")]
        public void ThenTheResultShouldBe(int _result)
        {
            //TODO: implement assert (verification) logic
            Assert.IsFalse(result == _result);
            Console.WriteLine(result);
        }
    }
}