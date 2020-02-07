﻿using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using MusicFileAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MusicFileAPI.Services
{
    public class AzureStorage : ICloudStorage
    {
        static CloudBlobClient blobClient;
        const string blobContainerName = "musicfiles";
        static CloudBlobContainer blobContainer;

        public async Task DeleteAll()
        {
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

            var blob = blobContainer.GetBlockBlobReference(filename);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<List<Uri>> Index()
        {
            // Retrieve storage account information from connection string
            // How to create a storage connection string - http://msdn.microsoft.com/en-us/library/azure/ee758697.aspx
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=mediaplayerstorage;AccountKey=s9oaodZkrULJ6MrooBlb0lAWGEOyWrzmtIIwT4xuVvMs4/AG+KRAUEPlBk34pxwP20QGIwUXmy4uN+ZHbYreVg==;EndpointSuffix=core.windows.net");

            // Create a blob client for interacting with the blob service.
            blobClient = storageAccount.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference(blobContainerName);
            await blobContainer.CreateIfNotExistsAsync();

            // To view the uploaded blob in a browser, you have two options. The first option is to use a Shared Access Signature (SAS) token to delegate  
            // access to the resource. See the documentation links at the top for more information on SAS. The second approach is to set permissions  
            // to allow public access to blobs in this container. Comment the line below to not use this approach and to use SAS. Then you can view the image  
            // using: https://[InsertYourStorageAccountNameHere].blob.core.windows.net/webappstoragedotnet-imagecontainer/FileName 
            await blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            // Gets all Cloud Block Blobs in the blobContainerName and passes them to teh view
            List<Uri> allBlobs = new List<Uri>();
            foreach (IListBlobItem blob in blobContainer.ListBlobs())
            {
                if (blob.GetType() == typeof(CloudBlockBlob))
                    allBlobs.Add(blob.Uri);
                //https://jj09.net/add-custom-metadata-to-azure-blob-storage-files-and-search-them-with-azure-search/
            }

            return allBlobs;
        }

        public async Task UploadAsync(IFormFile file)
        {
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(GetRandomBlobName(file.FileName));
            using (var stream = file.OpenReadStream())
            {
                blob.Metadata.Add("Title", "title");
                blob.Metadata.Add("Artist", "artist");
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