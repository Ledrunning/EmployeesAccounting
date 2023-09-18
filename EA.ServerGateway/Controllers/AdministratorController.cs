using EA.ServerGateway.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace EA.ServerGateway.Controllers
{
    [BasicAuthorization]
    public class AdministratorController : Controller
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
