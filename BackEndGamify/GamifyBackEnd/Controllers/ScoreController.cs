using GamifyBackEnd.DB;
using Microsoft.AspNetCore.Mvc;

namespace GamifyBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScoreController : ControllerBase
    {
        [HttpPost("submit")]
        public IActionResult SubmitScore([FromBody] ScoreData score)
        {
            if (score == null || score.PlayerName == null)
            {
                return BadRequest("Invalid data.");
            }
                
            using (var db = new GameDbContext())
            {
                var newScore = new Score
                {
                    scoreAmount = score.Score,
                    Level_id = 0,
                    User_id = 0

                };

                db.Scores.Add(newScore);
                db.SaveChanges();
            }

            return Ok(new { message = "Score submitted successfully!" });
        }
    }

    public class ScoreData
    {
        public string PlayerName { get; set; }
        public int Score { get; set; }
    }
}
