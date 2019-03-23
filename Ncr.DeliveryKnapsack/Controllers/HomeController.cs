using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ncr.DeliveryKnapsack.Models;
using Ncr.DeliveryKnapsack.Repositories;
using Ncr.DeliveryKnapsack.Services;

namespace Ncr.DeliveryKnapsack.Controllers
{
    public class HomeController : Controller
    {
        private readonly Mailer _mailer;
        private readonly ISolutionEvaluator _solutionEvaluator;
        private readonly IProblemGenerator _problemGenerator;
        private readonly SolutionRepository _solutionRepository;
        private readonly RegistrationRepository _registrationRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(Mailer mailer, ISolutionEvaluator solutionEvaluator,
            IProblemGenerator problemGenerator, SolutionRepository solutionRepository,
            RegistrationRepository registrationRepository, ILogger<HomeController> logger)
        {
            _mailer = mailer ?? throw new ArgumentNullException(nameof(mailer));
            _solutionEvaluator = solutionEvaluator ?? throw new ArgumentNullException(nameof(solutionEvaluator));
            _problemGenerator = problemGenerator ?? throw new ArgumentNullException(nameof(problemGenerator));
            _solutionRepository = solutionRepository ?? throw new ArgumentNullException(nameof(solutionRepository));
            _registrationRepository = registrationRepository ?? throw new ArgumentNullException(nameof(registrationRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IActionResult> Index(IndexViewModel submittedSolution, [FromQuery]bool kiosk)
        {
            if (!string.IsNullOrEmpty(submittedSolution.EMail))
            {
                _logger.LogInformation($"Seeing e-mail: {submittedSolution.EMail}");
            }

            string submittedText = null;
            if (submittedSolution.Solution != null)
            {
                try
                {
                    using (var sr = new StreamReader(submittedSolution.Solution.OpenReadStream()))
                    {
                        submittedText = await sr.ReadToEndAsync();
                        var solution = submittedText.Split('\n').Select(l => l.Trim()).Where(l => l != "").Select(l => int.Parse(l)).Distinct().ToList();

                        _logger.LogInformation($"Solution from {submittedSolution.EMail} to be evaluated");

                        submittedSolution.TotalCost = _solutionEvaluator.Evaluate(solution);

                        _logger.LogInformation($"Solution evaluated to {submittedSolution.TotalCost}");

                        _mailer.SolutionAccepted(submittedSolution.EMail, submittedText);
                    }
                }
                catch (Exception ex)
                {
                    submittedSolution.ProblemWithSolution = true;
                }
            }

            if (kiosk && !string.IsNullOrEmpty(submittedSolution.Name) && !string.IsNullOrEmpty(submittedSolution.EMail))
            {
                bool atLeastOneSucces = false;

                try
                {
                    var receiptResult = await _mailer.SendRegistrationReceiptToUs(submittedSolution);
                    var welcomeResult = await _mailer.SendWelcomeMailToThem(submittedSolution.EMail);
                    atLeastOneSucces = atLeastOneSucces || receiptResult || welcomeResult;
                }
                catch (Exception ex)
                {

                }

                if (atLeastOneSucces && kiosk)
                {
                    return Redirect("/Home/RegistrationSuccess");
                }
                else if (kiosk)
                {
                    submittedSolution.ProblemWithRegistration = true;
                }
            }

            submittedSolution.Kiosk = kiosk;

            return View(submittedSolution);
        }

        public string List()
        {
            var problem = _problemGenerator.GetProblem();

            var list = new StringBuilder();
            list.Append(problem.total + "\r\n");
            foreach (var order in problem.items)
            {
                list.Append($"{order.Id};{order.Weight};{order.Price}\r\n");
            }

            return list.ToString();
        }

        public IActionResult RegistrationSuccess()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
