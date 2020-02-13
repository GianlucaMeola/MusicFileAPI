using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicFileAPI.Extention;
using MusicFileAPI.Interfaces;
using MusicFileAPI.Model;

namespace MusicFileAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/MusicFile")]
    public class MusicFileController : Controller
    {
        private readonly ILogger<MusicFileController> _logger;
        private readonly ICloudStorage _cloudStorage;
        private readonly IValidator<UploadMusicFileRequest> _uploadMusicFileValidator;

        public MusicFileController(ILogger<MusicFileController> logger, ICloudStorage cloudStorage, IValidator<UploadMusicFileRequest> uploadMusicFileValidator)
        {
            _logger = logger;
            _cloudStorage = cloudStorage;
            _uploadMusicFileValidator = uploadMusicFileValidator;
        }
        
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(List<ApiErrorResponse>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Get()
        {
            var files = await _cloudStorage.Index();
            return Ok(files);
        }

        [HttpPost()]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(List<ApiErrorResponse>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Upload([FromForm] UploadMusicFileRequest request)
        {
            try
            {
                var validation = _uploadMusicFileValidator.Validate(request);

                if (!validation.IsValid)
                    return this.ErrorResult(HttpStatusCode.BadRequest, validation.Errors);

                await _cloudStorage.UploadAsync(request);
                return Ok();
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Failed to upload the music");
                return this.ErrorResult(HttpStatusCode.InternalServerError, "InternalServerError", "An error occured while processing this request.");
            }
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
