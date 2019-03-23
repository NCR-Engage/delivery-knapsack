using Microsoft.AspNetCore.Mvc;

namespace Ncr.DeliveryKnapsack.Controllers
{
    public class HealthController : Controller
    {
        public IActionResult Index()
        {
            return new EmptyResult();
        }
    }
}
