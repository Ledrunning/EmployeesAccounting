using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EA.ServerGateway.Controllers
{
    [Authorize]
    public class AdministratorController : Controller
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
