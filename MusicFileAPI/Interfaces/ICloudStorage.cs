using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicFileAPI.Interfaces
{
    public interface ICloudStorage
    {
        Task<List<FileDetails>> Index();
        Task UploadAsync(IFormFile file, string title, string artist);
        Task DeleteFile(string name);
        Task DeleteAll();
    }
}
