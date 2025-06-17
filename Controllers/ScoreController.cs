using GamifyBackEnd.DB;
using GamifyBackEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GamifyBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScoreController : ControllerBase
    {
        private readonly AuthService _authService;
        private GameDbContext _db;

        public ScoreController(AuthService authService, GameDbContext db)
        {
            _authService = authService;
            _db = db;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Name is required.");
            }

            try
            {
                using (_db)
                {
                    // Check if the user already exists
                    var user = _db.Users.FirstOrDefault(u => u.Name == request.Name);

                    if (user == null)
                    {
                        // Create new user
                        user = new User
                        {
                            Name = request.Name,
                            OverworldCoords = "15,5,5",
                            accesslevel = 0
                        };


                        _db.Users.Add(user);
                        _db.SaveChanges();
                    }

                    var roles = new[] { "Admin" }; // from database typically
                    var token = _authService.GenerateJwtToken(user.Name, roles);

                    return Ok(new { authToken = token, success = true, name = user.Name });
                }
            }
            catch (Exception e)
            {
                return BadRequest("Database failure: " + e.Message);
            }
        }

        [HttpPost("submit")]
        public IActionResult SubmitScore([FromBody] ScoreData score)
        {
            if (score == null || score.playerName == null)
            {
                return BadRequest("Invalid data.");
            }

            
            try
            {
                using (_db)
                {
                    var playerId = _db.Users.Where(p => p.Name == score.playerName).Select(p => p.Id).FirstOrDefault();
                    var gameId = _db.Games.Where(g => g.LevelName == score.gameName).Select(g => g.Id).FirstOrDefault();

                    var scoreExisting = _db.Scores.FirstOrDefault(s => s.User_id == playerId && s.Level_id == gameId);

                    if (scoreExisting == null)
                    {
                        var newScore = new Score
                        {
                            scoreAmount = score.Score,
                            Level_id = gameId,
                            User_id = playerId

                        };

                        _db.Scores.Add(newScore);
                        _db.SaveChanges();
                    }
                    else if (scoreExisting.scoreAmount < score.Score)
                    {
                        scoreExisting.scoreAmount = score.Score;
                        _db.SaveChanges();
                    }

                }
            }
            catch (Exception e)
            {
                return BadRequest("Database failure: " + e.Message);
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
                using (_db)
                {
                    var playerId = _db.Users
                                     .Where(p => p.Name == playerName)
                                     .Select(p => p.Id)
                                     .FirstOrDefault();

                    if (playerId == 0)
                    {
                        return NotFound("User not found.");
                    }

                    var totalScore = _db.Scores
                                       .Where(s => s.User_id == playerId)
                                       .Sum(s => s.scoreAmount);

                    return Ok(new { totalScore = totalScore });
                }
            }
            catch (Exception e)
            {
                return BadRequest("Database failure: " + e.Message);
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
                return BadRequest("Database failure: " + e.Message);
            }      
        }

    }



    public class ScoreData
    {
        public string gameName { get; set; }
        public string playerName { get; set; }
        public int Score { get; set; }
    }

    public class LoginRequest
    {
        public string Name { get; set; }
    }
}
