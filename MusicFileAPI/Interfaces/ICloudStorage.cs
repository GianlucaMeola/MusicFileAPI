using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicFileAPI.Interfaces
{
    public interface ICloudStorage
    {
        Task<List<Uri>> Index();
        Task UploadAsync(IFormFile file);
        Task DeleteFile(string name);
        Task DeleteAll();
    }
}
