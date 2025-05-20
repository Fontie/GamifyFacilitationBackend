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
                return BadRequest("No file uploaded.");

            var fileName = levelName;

            // DO NOT generate unique names. We need to be SURE it is always the same otherwise the front-end falls apart.
            // fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            using var stream = file.OpenReadStream();
            var url = await _blobService.UploadFileAsync(stream, fileName, file.ContentType);

            return Ok(new { message = "Game saved at " + url });
        }


       

    }
}