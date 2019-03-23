using System;
using System.Collections.Generic;

namespace Ncr.DeliveryKnapsack.Services
{
    public interface IProblemGenerator
    {
        (decimal total, IEnumerable<ProblemItem> items) GetProblem();
    }

    public class ProblemGenerator : IProblemGenerator
    {
        public static readonly int StepCount = 15000;

        public (decimal total, IEnumerable<ProblemItem> items) GetProblem()
        {
            var r = new Random(5);

            return (r.Next(), GetItems(r));
        }

        private IEnumerable<ProblemItem> GetItems(Random r)
        {
            var currentItemId = 258431;
            for (var i = 0; i < StepCount; i++)
            {
                currentItemId += r.Next(10) + 1;
                var price = (decimal)r.Next() / 1000;
                var weight = price + Math.Round((decimal)r.Next() / 1000000) / 100;
                yield return new ProblemItem { Id = currentItemId, Price = price, Weight = weight };
            }
        }
    }
}
