using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Ncr.DeliveryKnapsack.Services
{
    public interface ISolutionEvaluator
    {
        decimal Evaluate(IEnumerable<int> solution);
    }

    public class SolutionEvaluator : ISolutionEvaluator
    {
        private readonly IProblemGenerator _problemGenerator;
        private readonly ILogger<SolutionEvaluator> _log;

        public SolutionEvaluator(IProblemGenerator problemGenerator, ILogger<SolutionEvaluator> log)
        {
            _problemGenerator = problemGenerator ?? throw new ArgumentNullException(nameof(problemGenerator));
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public class KnapsackOverflowException : Exception { }

        public decimal Evaluate(IEnumerable<int> solution)
        {
            var problem = _problemGenerator.GetProblem();
            var items = problem.items.ToList();

            var weights = items.ToDictionary(item => item.Id, item => item.Weight);
            var prices = items.ToDictionary(item => item.Id, item => item.Price);

            var totalWeight = solution.Select(id => weights[id]).Sum();
            var totalPrice = solution.Select(id => prices[id]).Sum();

            _log.LogInformation($"totalWeight: {totalWeight}");
            _log.LogInformation($"totalPrice: {totalPrice}");

            if (totalWeight > problem.total)
            {
                throw new KnapsackOverflowException();
            }

            return totalPrice;
        }
    }
}
