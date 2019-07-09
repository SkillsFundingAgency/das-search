using Microsoft.AspNetCore.Mvc;

namespace Sfa.Das.Sas.Shared.Components.Controllers
{
    public class BasketController : Controller
    {
        [HttpPost]
        public IActionResult Add(string apprenticeshipId, int? ukprn)
        {
            // Validate arg formats
                //Fail: throw exception

            // Merge item into basket
            return View();
        }
    }
}
