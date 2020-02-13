using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicFileAPI.Interfaces
{
    public interface IStorageConnectionFactory
    {
        Task<CloudBlobContainer> GetContainer();
    }
}
