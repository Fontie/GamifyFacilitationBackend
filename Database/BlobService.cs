using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace GamifyBackEnd.Database
{

    public class BlobService
    {
        private readonly string _storageAccountName;
        private readonly string _storageAccountKey;
        private readonly string _containerName;
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(IConfiguration config)
        {
            _storageAccountName = Environment.GetEnvironmentVariable("AZUREBLOB_ACCOUNTNAME");//config["AzureBlob:AccountName"]!; //Found in appsettings.json
            _storageAccountKey = Environment.GetEnvironmentVariable("AZUREBLOB_ACCOUNTKEY");//config["AzureBlob:AccountKey"]!; //Found in appsettings.json
            _containerName = Environment.GetEnvironmentVariable("AZUREBLOB_CONTAINERNAME"); //config["AzureBlob:ContainerName"]!; //Found in appsettings.json
            var connectionString = Environment.GetEnvironmentVariable("AZUREBLOB_CONNECTIONSTRING"); //config["AzureBlob:ConnectionString"]!; //Found in appsettings.json
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, string contentEncoding = null)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync();
            await containerClient.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.None);

            var blobClient = containerClient.GetBlobClient(fileName);

            var blobHttpHeaders = new Azure.Storage.Blobs.Models.BlobHttpHeaders
            {
                ContentType = contentType
            };

            if (!string.IsNullOrEmpty(contentEncoding))
            {
                blobHttpHeaders.ContentEncoding = contentEncoding;
            }

            await blobClient.UploadAsync(fileStream, new Azure.Storage.Blobs.Models.BlobUploadOptions
            {
                HttpHeaders = blobHttpHeaders,
                TransferOptions = new Azure.Storage.StorageTransferOptions
                {
                    // Optional: adjust based on file size but this should be good enough for unity webgl... I hope
                    MaximumTransferSize = 4 * 1024 * 1024,
                    InitialTransferSize = 4 * 1024 * 1024
                }
            });

            return blobClient.Uri.ToString();
        }

    }
}