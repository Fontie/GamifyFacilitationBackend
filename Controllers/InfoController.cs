using Microsoft.AspNetCore.Mvc;

namespace GamifyBackEnd.Controllers
{

    [Route("api/info")]
    public class InfoController : Controller
    {
        public IActionResult Index()
        {
            return Ok("Version 1.0");
        }
    }
}
