using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicFileAPI.Interfaces;

namespace MusicFileAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/MusicFile")]
    public class MusicFileController : Controller
    {
        private readonly ICloudStorage _cloudStorage;
        public MusicFileController(ICloudStorage cloudStorage)
        {
            _cloudStorage = cloudStorage;
        }
        
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var files = await _cloudStorage.Index();
            return Ok(files);
        }

        [HttpPost()]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            await _cloudStorage.UploadAsync(file);
            return Ok();
        }

        [HttpDelete("{fileName}")]
        public async Task<IActionResult> Delete(string fileName)
        {
            await _cloudStorage.DeleteFile(fileName);
            return Ok();
        }

        [HttpDelete()]
        public async Task<IActionResult> DeleteAll()
        {
            await _cloudStorage.DeleteAll();
            return Ok();
        }
    }
}
