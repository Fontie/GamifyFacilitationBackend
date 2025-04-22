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
            if (score == null || score.playerName == null)
            {
                return BadRequest("Invalid data.");
            }

            
                
            using (var db = new GameDbContext())
            {
                var playerId = db.Users.Where(p => p.Name == score.playerName).Select(p => p.Id).FirstOrDefault();
                var gameId = db.Games.Where(g => g.LevelName == score.gameName).Select(g => g.Id).FirstOrDefault();

                var scoreExisting = db.Scores.FirstOrDefault(s => s.User_id == playerId && s.Level_id == gameId);

                if (scoreExisting == null)
                {
                    var newScore = new Score
                    {
                        scoreAmount = score.Score,
                        Level_id = gameId,
                        User_id = playerId

                    };

                    db.Scores.Add(newScore);
                    db.SaveChanges();
                }
                else if (scoreExisting.scoreAmount < score.Score)
                {
                    scoreExisting.scoreAmount = score.Score;
                    db.SaveChanges();
                }
                
            }

            return Ok(new { message = "Score submitted successfully!" });
        }
    }

    public class ScoreData
    {
        public string gameName { get; set; }
        public string playerName { get; set; }
        public int Score { get; set; }
    }
}
