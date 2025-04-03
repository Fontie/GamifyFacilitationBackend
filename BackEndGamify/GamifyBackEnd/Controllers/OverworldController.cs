using Microsoft.AspNetCore.Mvc;

namespace GamifyBackEnd.Controllers
{
    [ApiController]
    [Route("api/overworld")]
    public class OverworldController : Controller
    {
        [HttpGet("getPlayerProgress")]
        public IActionResult GetData()
        {
            var data = new { message = "Hello from .NET Backend!" };
            return Ok(data);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
