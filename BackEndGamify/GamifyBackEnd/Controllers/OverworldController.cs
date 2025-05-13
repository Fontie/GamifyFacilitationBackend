using GamifyBackEnd.DB;
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
            var data = new { message = "I have failed thee!!!" };

            using (var db = new GameDbContext())
            {
                var userName = "LeeroyJankins";
                var userFromDB = db.Users.FirstOrDefault(u => u.Name == userName);

                if (userFromDB != null)
                {
                    return Ok(userFromDB);
                }           
            }

            return Ok(data);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("savePlayerProgress")]
        public IActionResult savePlayerProgress([FromBody] progressData data)
        {
            if (data.PlayerName == null)
            {
                return BadRequest("Invalid data.");
            }

            using (var db = new GameDbContext())
            {
                var myUser = db.Users.FirstOrDefault(u => u.Name == data.PlayerName);
                
                if (myUser != null)
                {
                    var xxstring = Convert.ToString(data.xx);
                    var yystring = Convert.ToString(data.yy);
                    var zzstring = Convert.ToString(data.zz);

                    myUser.OverworldCoords = xxstring+","+ yystring + "," + zzstring + ",";
                }
                db.SaveChanges();
            }

            return Ok(new { message = "Updated user progress!" });
        }

        public class progressData
        {
            public string PlayerName { get; set; }
            public float xx { get; set; }
            public float yy { get; set; }
            public float zz { get; set; }

        }


        //TODO: MAKE THIS SOMETHING THAT IS ACTUALLY USED LATER FFS!!!
        [HttpPost("increaseAccess/{userName}")]
        public IActionResult increaseAccess(string userName)
        {

            using (var db = new GameDbContext())
            {
                var myUser = db.Users.FirstOrDefault(u => u.Name == userName);

                if (myUser != null)
                {
                   myUser.accesslevel++;
                }
                db.SaveChanges();
            }

            return Ok(new { message = "Access level updated!" });
        }
    }
}
