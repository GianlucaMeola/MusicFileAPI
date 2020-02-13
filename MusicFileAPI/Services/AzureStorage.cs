using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using MusicFileAPI.Interfaces;
using MusicFileAPI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MusicFileAPI.Services
{
    public class AzureStorage : ICloudStorage
    {
        private readonly IStorageConnectionFactory _storageConnectionFactory;

        public AzureStorage(IStorageConnectionFactory storageConnectionFactory)
        {
            _storageConnectionFactory = storageConnectionFactory;
        }
        public async Task DeleteAll()
        {
            var blobContainer = await _storageConnectionFactory.GetContainer();
            foreach (var blob in blobContainer.ListBlobs())
            {
                if (blob.GetType() == typeof(CloudBlockBlob))
                {
                    await ((CloudBlockBlob)blob).DeleteIfExistsAsync();
                }
            }
        }

        public async Task DeleteFile(string fileName)
        {
            Uri uri = new Uri(fileName);
            string filename = Path.GetFileName(uri.LocalPath);

            var blobContainer = await _storageConnectionFactory.GetContainer();
            var blob = blobContainer.GetBlockBlobReference(filename);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<List<FileDetails>> GetAll()
        {
            List<FileDetails> allBlobs = new List<FileDetails>();
            var blobContainer = await _storageConnectionFactory.GetContainer();
            foreach (CloudBlockBlob item in blobContainer.ListBlobs(null, true, BlobListingDetails.Metadata))
            {
                allBlobs.Add(new FileDetails
                {
                    uri = item.Uri,
                    artist = item.Metadata.FirstOrDefault(x => x.Key == "artist").Value,
                    title = item.Metadata.FirstOrDefault(x => x.Key == "title").Value
                });
            }
            return allBlobs;
        }

        public async Task UploadAsync(UploadMusicFileRequest payLoadDetails)
        {
            var blobContainer = await _storageConnectionFactory.GetContainer();
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(GetRandomBlobName(payLoadDetails.musicFile.FileName));
            using (var stream = payLoadDetails.musicFile.OpenReadStream())
            {
                blob.Metadata.Add("title", payLoadDetails.title);
                blob.Metadata.Add("artist", payLoadDetails.artist);
                await blob.UploadFromStreamAsync(stream);
            }
        }

        /// <summary> 
        /// string GetRandomBlobName(string filename): Generates a unique random file name to be uploaded  
        /// </summary> 
        private string GetRandomBlobName(string filename)
        {
            string ext = Path.GetExtension(filename);
            return string.Format("{0:10}_{1}{2}", DateTime.Now.Ticks, Guid.NewGuid(), ext);
        }
    }

}