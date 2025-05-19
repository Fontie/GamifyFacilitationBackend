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

            
            try
            {
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
            }
            catch (Exception e)
            {
                return BadRequest("Database failure.");
            }     
            

            return Ok(new { message = "Score submitted successfully!" });
        }

        [HttpGet("total-score/{playerName}")]
        public IActionResult GetTotalScore(string playerName)
        {
            if (string.IsNullOrWhiteSpace(playerName))
            {
                return BadRequest("Player name is required.");
            }

            try
            {
                using (var db = new GameDbContext())
                {
                    var playerId = db.Users
                                     .Where(p => p.Name == playerName)
                                     .Select(p => p.Id)
                                     .FirstOrDefault();

                    if (playerId == 0)
                    {
                        return NotFound("User not found.");
                    }

                    var totalScore = db.Scores
                                       .Where(s => s.User_id == playerId)
                                       .Sum(s => s.scoreAmount);

                    return Ok(new { totalScore = totalScore });
                }
            }
            catch (Exception e)
            {
                return BadRequest("Database failure.");
            }     
        }

        [HttpGet("leaderboard")]
        public IActionResult GetLeaderboard()
        {
            try
            {
                using (var db = new GameDbContext())
                {
                    var leaderboard = db.Users
                        .Select(user => new
                        {
                            Name = user.Name,
                            Score = db.Scores
                                      .Where(s => s.User_id == user.Id)
                                      .Sum(s => (int?)s.scoreAmount) ?? 0
                        })
                        .OrderByDescending(u => u.Score)
                        .ToList();

                    return Ok(leaderboard);
                }

            }
            catch(Exception e)
            {
                return BadRequest("Database failure.");
            }      
        }

    }



    public class ScoreData
    {
        public string gameName { get; set; }
        public string playerName { get; set; }
        public int Score { get; set; }
    }
}
