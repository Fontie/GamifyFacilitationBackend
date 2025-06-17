using GamifyBackEnd.DB;
using GamifyBackEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace GamifyBackEnd.Controllers
{
    [ApiController]
    [Route("api/overworld")]
    public class OverworldController : Controller
    {
        private GameDbContext _db;

        public OverworldController(GameDbContext db)
        {
            _db = db;
        }

        [HttpGet("getPlayerProgress/{playerName}")]
        public IActionResult GetData(string playerName)
        {
            try
            {
                var data = new { message = "No data has been foud" };

                using (_db)
                {
                    var userName = playerName;
                    var userFromDB = _db.Users.FirstOrDefault(u => u.Name == userName);

                    if (userFromDB != null)
                    {
                        return Ok(userFromDB);
                    }
                }

                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest("Database failure: " + e.Message);
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("savePlayerProgress")]
        public IActionResult savePlayerProgress([FromBody] progressData data)
        {
            try
            {
                if (data.PlayerName == null)
                {
                    return BadRequest("Invalid data.");
                }

                using (_db)
                {
                    var myUser = _db.Users.FirstOrDefault(u => u.Name == data.PlayerName);

                    if (myUser != null)
                    {
                        var xxstring = Convert.ToString(data.xx);
                        var yystring = Convert.ToString(data.yy);
                        var zzstring = Convert.ToString(data.zz);

                        myUser.OverworldCoords = xxstring + "," + yystring + "," + zzstring + ",";
                    }
                    _db.SaveChanges();
                }

                return Ok(new { message = "Updated user progress!" });
            }
            catch (Exception e)
            {
                return BadRequest("Database failure: " + e.Message);
            }
        }

        public class progressData
        {
            public string PlayerName { get; set; }
            public float xx { get; set; }
            public float yy { get; set; }
            public float zz { get; set; }

        }


        //TODO: MAKE THIS SOMETHING THAT IS ACTUALLY USED LATER!!!
        [HttpPost("increaseAccess/{userName}")]
        public IActionResult increaseAccess(string userName)
        {
            try
            {
                using (_db)
                {
                    var myUser = _db.Users.FirstOrDefault(u => u.Name == userName);

                    if (myUser != null)
                    {
                        myUser.accesslevel++;
                    }
                    _db.SaveChanges();
                }

                return Ok(new { message = "Access level updated!" });
            }
            catch(Exception e)
            {
                return BadRequest("Database failure: " + e.Message);
            }
            
        }
    }
}
