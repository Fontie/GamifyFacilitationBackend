using GamifyBackEnd.DB;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace GamifyBackEnd.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GameController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public GameController(IWebHostEnvironment env)
        {
            _env = env;
        }

        // Endpoint to fetch, extract, and serve the game, agree what the game id should even be at the end of the day pls.
        [HttpGet("load/{levelName}")]
        public async Task<IActionResult> LoadGame(string levelName)
        {
            try
            {
                // Simulating fetching ZIP from database as a byte array
                byte[] zipData = await GetGameZipFromDatabase(levelName);
                if (zipData == null) return NotFound("Game not found.");

                // Define extraction path (wwwroot/games/{levelName}/)
                string gameFolderPath = Path.Combine(_env.WebRootPath, "games", levelName);

                //Upload bij upload niet call

                // DELETE the existing folder and all contents (if it exists)
                if (Directory.Exists(gameFolderPath))
                {
                    Directory.Delete(gameFolderPath, recursive: true);
                }

                // Recreate the directory
                Directory.CreateDirectory(gameFolderPath);

                // Extract the ZIP file
                using (var zipStream = new MemoryStream(zipData))
                using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
                {
                    archive.ExtractToDirectory(gameFolderPath);
                }

                // Return the game URL
                string gameUrl = $"{Request.Scheme}://{Request.Host}/games/{levelName}/index.html";
                return Ok(new { url = gameUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error loading game: {ex.Message}");
            }
        }

        // Simulated method to get ZIP file from database
        private async Task<byte[]> GetGameZipFromDatabase(string levelName)
        {
            try
            {
                byte[] fileData = null;
                var game = new Game {};

                using (var db = new GameDbContext())
                {
                    game = db.Games.FirstOrDefault(game => game.LevelName == levelName);
                }

                if (game != null)
                {
                    fileData = game.ZipData;
                }

                return fileData;
            }
            catch(Exception)
            {
                return null;
            }
            
        }
    }
}