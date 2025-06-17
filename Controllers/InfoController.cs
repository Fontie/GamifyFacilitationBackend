using Microsoft.AspNetCore.Mvc;

namespace GamifyBackEnd.Controllers
{

    [Route("api/info")]
    public class InfoController : Controller
    {
        public IActionResult Index()
        {
            return Ok("Endpoints can be reached. Also ci/cd works. Niet handmatig");
        }
    }
}
