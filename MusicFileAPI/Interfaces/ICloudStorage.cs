﻿using Microsoft.AspNetCore.Http;
using MusicFileAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicFileAPI.Interfaces
{
    public interface ICloudStorage
    {
        Task<List<FileDetails>> Index();
        Task UploadAsync(PayloadDetails payLoadDetails);
        Task DeleteFile(string name);
        Task DeleteAll();
    }
}
