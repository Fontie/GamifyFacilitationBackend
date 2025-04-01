using System;
using System.IO;
using System.IO.Compression;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GamifyBackEnd.DB;
using MySqlConnector;

namespace GamifyBackEnd.Controllers
{
        
    [ApiController]    
    [Route("api/admin")]
    public class FileUploadController : ControllerBase
    {
        [HttpPost("upload")]
        [RequestSizeLimit(100_000_000_000_000)]
        public async Task<IActionResult> UploadGame([FromForm] string gameName, [FromForm] IFormFile file, [FromForm] string levelName)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            byte[] fileData = memoryStream.ToArray();

            using (var db = new GameDbContext())
            {
                var newGame = new Game
                {
                    Name = gameName,
                    ZipData = fileData,
                    LevelName = levelName
                };

                db.Games.Add(newGame);
                db.SaveChanges();
            }
            return Ok(new { message = "Game saved on Database!" });
        }


        
        [HttpGet("download/{gameName}")]
        public async Task<IActionResult> DownloadGame(string gameName)
        {
            var game = new Game{};

            using (var db = new GameDbContext())
            {
                game = db.Games.FirstOrDefault(game => game.LevelName == gameName);
            }
         
            if (game.ZipData == null)
            {
                return NotFound("Game not found.");
            }

            byte[] fileData = game.ZipData;

            return File(fileData, "application/zip", $"{gameName}.zip");
        }
        
    }
}