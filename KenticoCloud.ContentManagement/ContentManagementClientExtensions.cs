using System.IO;
using System.Threading.Tasks;

using KenticoCloud.ContentManagement.Models.Assets;

namespace KenticoCloud.ContentManagement
{
    public static class ContentManagementClientExtensions
    {
        /// <summary>
        /// Uploads the given file from a file system.
        /// </summary>
        /// <param name="client">Content management client instance.</param>
        /// <param name="filePath">File system path to the file. Name of the file will be used for the asset name when creating an asset. Example: c:\MyFiles\which-brewing-fits-you-1080px.jpg</param>
        /// <param name="contentType">Specifies the media type of the binary data. Example: image/jpeg, application/zip.</param>
        public static async Task<FileReferenceModel> UploadFile(this ContentManagementClient client, string filePath, string contentType)
        {
            var stream = File.OpenRead(filePath);

            return await client.UploadFileAsync(stream, Path.GetFileName(filePath), contentType);
        }

        /// <summary>
        /// Uploads the given file from a byte array.
        /// </summary>
        /// <param name="client">Content management client instance.</param>
        /// <param name="data">Binary data of the file.</param>
        /// <param name="fileName">The name of the uploaded binary file. It will be used for the asset name when creating an asset. Example: which-brewing-fits-you-1080px.jpg.</param>
        /// <param name="contentType">Specifies the media type of the binary data. Example: image/jpeg, application/zip.</param>
        public static async Task<FileReferenceModel> UploadFile(this ContentManagementClient client, byte[] data, string fileName, string contentType)
        {
            var stream = new MemoryStream(data);

            return await client.UploadFileAsync(stream, fileName, contentType);
        }
    }
}
