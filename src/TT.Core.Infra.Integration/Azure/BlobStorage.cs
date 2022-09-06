using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

namespace TT.Core.Infra.Integration.Azure;

public static class BlobStorage
{
    public static async Task<string> Upload(string connection, string container, IFormFile file, string fileName)
    {
        var blob = new BlobClient(connection, container, fileName);

        await Delete(connection, container, fileName);

        await blob.UploadAsync(file.OpenReadStream());

        return blob.Uri.AbsoluteUri;
    }

    public static async Task Delete(string connection, string container, string fileName)
    {
        var containerClient = new BlobContainerClient(connection, container);

        await containerClient.DeleteBlobIfExistsAsync(fileName);
    }
}
