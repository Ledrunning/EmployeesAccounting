using Microsoft.AspNetCore.Mvc;

namespace EA.ServerGateway.Controllers
{
    public class AdministratorController : Controller
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
