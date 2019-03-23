using Microsoft.AspNetCore.Http;
using Ncr.DeliveryKnapsack.Repositories;
using System.Collections.Generic;

namespace Ncr.DeliveryKnapsack.Models
{
    public class IndexViewModel
    {
        public bool Kiosk { get; set; }

        public string Name { get; set; }

        public string EMail { get; set; }

        public string Phone { get; set; }

        public bool OpenDoor { get; set; }
        public bool Knapsack { get; set; }
        public bool Hackathon { get; set; }

        public bool Intern { get; set; }
        public bool Part { get; set; }
        public bool Full { get; set; }

        public IFormFile Solution { get; set; }

        public decimal? TotalCost { get; set; }
        
        public bool ProblemWithSolution { get; set; }

        public bool ProblemWithRegistration { get; set; }

        public IList<SolutionlessSolution> BestSolutions { get; set; }
    }
}
