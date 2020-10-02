using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace XMemes.Data.Repositories
{
    public class AzureBlobFileRepository : IFileRepository
    {
        private readonly BlobContainerClient _containerClient;
        private readonly ILogger<AzureBlobFileRepository> _logger;

        public AzureBlobFileRepository(
            IConfiguration config,
            ILogger<AzureBlobFileRepository> logger)
        {
            _logger = logger;
            var connectionString = config.GetConnectionString("AzureBlobStorage");
            _containerClient = new BlobContainerClient(connectionString, "images");
        }

        public async Task<string?> GetUrl(string filename)
        {
            try
            {
                var blobClient = _containerClient.GetBlobClient(filename);
                var exists = await blobClient.ExistsAsync();
                if (exists is null || !exists.Value) return null;
                
                var url = blobClient.Uri.AbsoluteUri;
                return url;
            }
            catch (Exception e)
            {
                _logger.LogError($"Could not find blob: {filename}", e);
                return null;
            }
        }
        
        public async Task<string?> Upload(string filename, Stream fileStream)
        {
            try
            {
                var blobClient = _containerClient.GetBlobClient(filename);

                fileStream.Position = 0;
                var result = await blobClient.UploadAsync(fileStream);
                
                return result is not null ? filename : null;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error uploading file from stream. Filename: {filename}", e);
                return null;
            }
        }

        public Task<string?> Upload(string filename, byte[] bytes) =>
            Upload(filename, new MemoryStream(bytes));

        public async Task<string?> Upload(string filename, string uploadFilePath)
        {
            try
            {
                await using var uploadFileStream = File.OpenRead(uploadFilePath);
                var result = await Upload(filename, uploadFileStream);
                uploadFileStream.Close();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading file {uploadFilePath} as {filename}", ex);
                return null;
            }
        }

        public async Task<FileInfo?> Download(string filename)
        {
            try
            {
                var tempFile = Path.GetTempFileName();
                await using var downloadFileStream = File.OpenWrite(tempFile);
                var blobClient = _containerClient.GetBlobClient(filename);
                await blobClient.DownloadToAsync(downloadFileStream);
                downloadFileStream.Close();
                return new FileInfo(tempFile);
            }
            catch (IOException ex)
            {
                _logger.LogError($"Error creating temporary file for storing download of {filename}", ex);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error downloading file: {filename}", ex);
                return null;
            }
        }

        public async Task<bool> Exists(string filename)
        {
            var blobClient = _containerClient.GetBlobClient(filename);
            var response = await blobClient.ExistsAsync();
            return response?.Value ?? false;
        }

        public async Task<IList<string>> GetAllFilenames()
        {
            var files = new List<string>();
            await foreach (var blobItem in _containerClient.GetBlobsAsync())
                files.Add(blobItem.Name);
            return files;
        }

        public async Task<bool> Delete(string filename)
        {
            var response = await _containerClient.DeleteBlobIfExistsAsync(filename);
            return response?.Value ?? false;
        }
    }
}