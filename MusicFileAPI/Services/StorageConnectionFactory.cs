using Microsoft.Azure.Storage.Blob;
using MusicFileAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicFileAPI.Model;
using Microsoft.Azure.Storage;

namespace MusicFileAPI.Services
{
    public class StorageConnectionFactory : IStorageConnectionFactory
    {
        private CloudBlobClient _blobClient;
        private CloudBlobContainer _blobContainer;
        private readonly CloudStorageOptions _storageOptions;

        public StorageConnectionFactory(CloudStorageOptions storageOptions)
        {
            _storageOptions = storageOptions;
        }

        public async Task<CloudBlobContainer> GetContainer()
        {
            if(_blobContainer != null) 
            {
                return _blobContainer;
            }
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_storageOptions.ConnectionString);

            _blobClient = storageAccount.CreateCloudBlobClient();
            _blobContainer = _blobClient.GetContainerReference(_storageOptions.Container);
            await _blobContainer.CreateIfNotExistsAsync();

            await _blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            return _blobContainer;
        }
    }
}
