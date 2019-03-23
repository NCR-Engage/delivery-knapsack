using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ncr.DeliveryKnapsack.Services;
using System.Linq;
using static Ncr.DeliveryKnapsack.Services.SolutionEvaluator;

namespace Ncr.DeliveryKnapsack.Test
{
    [TestClass]
    public class SolutionEvaluatorTest
    {
        [TestMethod]
        public void SingleItem()
        {
            var problemGenerator = new ProblemGenerator();
            var solutionEvaluator = new SolutionEvaluator(problemGenerator);

            var totalPrice = solutionEvaluator.Evaluate(new[] { 258434 });

            Assert.AreEqual(564707.973M, totalPrice);
        }

        [TestMethod]
        [ExpectedException(typeof(KnapsackOverflowException))]
        public void AllItems()
        {
            var problemGenerator = new ProblemGenerator();
            var solutionEvaluator = new SolutionEvaluator(problemGenerator);

            var totalPrice = solutionEvaluator.Evaluate(problemGenerator.GetProblem().items.Select(item => item.Id).ToList());
        }
    }
}
