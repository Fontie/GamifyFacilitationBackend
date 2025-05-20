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
            _storageAccountName = config["AzureBlob:AccountName"]!; //Add the name from Geert-Jan
            _storageAccountKey = config["AzureBlob:AccountKey"]!; //Add the Key from Geert-Jan
            _containerName = config["AzureBlob:ContainerName"]!; //Add the container from Geert-Jan
            var connectionString = config["AzureBlob:ConnectionString"]!; //Add the constring from Geert-Jan
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public string GetSasUrl(string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _containerName,
                BlobName = fileName,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var credential = new StorageSharedKeyCredential(_storageAccountName, _storageAccountKey);
            var sasToken = sasBuilder.ToSasQueryParameters(credential).ToString();

            return $"{blobClient.Uri}?{sasToken}";
        }
    }
}