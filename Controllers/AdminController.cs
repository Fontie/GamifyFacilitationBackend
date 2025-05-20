using System;
using System.IO;
using System.IO.Compression;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GamifyBackEnd.DB;
using MySqlConnector;
using MongoDB.Driver.Core.Configuration;
using GamifyBackEnd.Database;

namespace GamifyBackEnd.Controllers
{
        
    [ApiController]    
    [Route("api/admin")]
    public class FileUploadController : ControllerBase
    {
        private readonly BlobService _blobService;

        public FileUploadController(BlobService blobService)
        {
            _blobService = blobService;
        }

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
                var existingGame = db.Games.FirstOrDefault(g => g.LevelName == levelName);

                if (existingGame == null)
                {
                    var newGame = new Game
                    {
                        Name = gameName,
                        ZipData = fileData,
                        LevelName = levelName
                    };

                    db.Games.Add(newGame);
                }
                else
                {
                    existingGame.ZipData = fileData;
                    existingGame.Name = gameName;
                }

                
                db.SaveChanges();
            }
            return Ok(new { message = "Game saved on Database!" });
        }


        
        [HttpGet("download/{gameName}")]
        public async Task<IActionResult> DownloadGame(string gameName)
        {
             
            byte[] fileData = _blobService.GetSasUrl(gameName); ;

            return File(fileData, "application/zip", $"{gameName}.zip");
        }


       

    }
}