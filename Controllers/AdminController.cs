using System;
using System.IO;
using System.IO.Compression;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GamifyBackEnd.DB;
using MySqlConnector;
using MongoDB.Driver.Core.Configuration;
using GamifyBackEnd.Database;
using System.IO.Compression;

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

                string blobPath = $"" + levelName + $"/{entry.FullName.Replace("\\", "/")}";
                string contentType = GetContentType(entry.Name);

                string contentEncoding = Path.GetExtension(entry.Name).EndsWith(".br") ? "br" : null;

                await _blobService.UploadFileAsync(memoryStream, blobPath, contentType, contentEncoding);
            }

            return Ok(new { message = "ZIP file extracted and uploaded to testingNameNow/" });

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