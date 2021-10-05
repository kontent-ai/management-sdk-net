using System;
using System.IO;

namespace Kentico.Kontent.Management.Models.Assets
{
    /// <summary>
    /// Wraps the information about file content source.
    /// </summary>
    public class FileContentSource
    {
        private byte[] Data { get; set; }

        private string FilePath { get; set; }

        private Stream Stream { get; set; }

        internal bool CreatesNewStream { get; private set; }

        /// <summary>
        /// Gets or sets the media type of the asset, for example: "image/jpeg".
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the name of the file/
        /// </summary>
        public string FileName { get; set; }

        private FileContentSource()
        {
        }

        /// <summary>
        /// Gets an open stream for the file data.
        /// </summary>
        /// <returns>The <see cref="Stream"/> instance that represents opened stream.</returns>
        public Stream OpenReadStream()
        {
            if (Stream != null)
            {
                return Stream;
            }

            if (Data != null)
            {
                return new MemoryStream(Data);
            }

            if (FilePath != null)
            {
                return File.OpenRead(FilePath);
            }

            throw new InvalidOperationException("File content source does not have any source set.");
        }

        /// <summary>
        /// Creates content source file.
        /// </summary>
        /// <param name="data">Binary data of the file.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="contentType">Gets or sets the media type of the asset, for example: "image/jpeg".</param>
        public FileContentSource(byte[] data, string fileName, string contentType)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("File name cannot be empty.", nameof(fileName));
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentException("Content type cannot be empty.", nameof(contentType));
            }

            Data = data ?? throw new ArgumentNullException(nameof(data));
            FileName = fileName;
            ContentType = contentType;
            CreatesNewStream = true;
        }

        /// <summary>
        /// Creates content source file.
        /// </summary>
        /// <param name="filePath">Path to file.</param>
        /// <param name="contentType">Gets or sets the media type of the asset, for example: "image/jpeg".</param>
        public FileContentSource(string filePath, string contentType)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File name cannot be empty.", nameof(filePath));
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentException("Content type cannot be empty.", nameof(contentType));
            }


            FilePath = filePath;
            FileName = Path.GetFileName(FilePath);
            ContentType = contentType;
            CreatesNewStream = true;
        }

        /// <summary>
        /// Creates content source file.
        /// </summary>
        /// <param name="stream">Stream of the input data</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="contentType">Gets or sets the media type</param>
        public FileContentSource(Stream stream, string fileName, string contentType)
        {
            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentException("Content type cannot be empty.", nameof(contentType));
            }

            Stream = stream ?? throw new ArgumentNullException(nameof(stream));
            FileName = fileName;
            ContentType = contentType;
        }
   }
}
