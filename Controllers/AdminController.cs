using System;
using System.IO;
using System.IO.Compression;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GamifyBackEnd.DB;
using MySqlConnector;
using MongoDB.Driver.Core.Configuration;
using System.IO.Compression;
using GamifyBackEnd.Services;

namespace GamifyBackEnd.Controllers
{

    [ApiController]    
    [Route("api/admin")]
    public class FileUploadController : ControllerBase
    {
        private readonly BlobService _blobService;
        private readonly GameDbContext _db;

        public FileUploadController(BlobService blobService, GameDbContext db)
        {
            _blobService = blobService;
            _db = db;
        }

        [HttpPost("upload")]
        [RequestSizeLimit(100_000_000_000_000)]
        public async Task<IActionResult> UploadGame([FromForm] string gameName, [FromForm] IFormFile file, [FromForm] string levelName)
        {

            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded.");

                using var archiveStream = file.OpenReadStream();
                using var zipArchive = new ZipArchive(archiveStream, ZipArchiveMode.Read);

                foreach (var entry in zipArchive.Entries)
                {
                    if (string.IsNullOrEmpty(entry.Name))
                        continue; // Skip directories

                    using var entryStream = entry.Open();
                    using var memoryStream = new MemoryStream();
                    await entryStream.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    string blobPath = $"yondas-quest\\" + levelName + $"/{entry.FullName.Replace("\\", "/")}";
                    string contentType = GetContentType(entry.Name);

                    string contentEncoding = Path.GetExtension(entry.Name).EndsWith(".br") ? "br" : null;

                    await _blobService.UploadFileAsync(memoryStream, blobPath, contentType, contentEncoding);
                }

                using (_db)
                {
                    var gameId = _db.Games.Where(g => g.LevelName == levelName).Select(g => g.Id).FirstOrDefault();
                    var gameExisting = _db.Games.FirstOrDefault(s => s.Id == gameId);

                    if (gameExisting == null)
                    {
                        var newGame = new Game
                        {
                            Name = gameName,
                            LevelName = levelName

                        };

                        _db.Games.Add(newGame);
                        _db.SaveChanges();
                    }
                    else
                    {
                        gameExisting.Name = gameName;
                        _db.SaveChanges();
                    }

                }

                return Ok(new { message = "ZIP file extracted and uploaded to yondas-quest/" });
            }
            catch(Exception e)
            {
                return BadRequest("PIT Error: " + e.Message);
            }
            

        }

        private string GetContentType(string fileName)
        {
            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out string contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        /*
if (file == null || file.Length == 0)
    return BadRequest("No file uploaded.");

var fileName = levelName;

// DO NOT generate unique names. We need to be SURE it is always the same otherwise the front-end falls apart.
// fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

using var stream = file.OpenReadStream();
var url = await _blobService.UploadFileAsync(stream, fileName, file.ContentType);

return Ok(new { message = "Game saved at " + url });
*/


    }
}