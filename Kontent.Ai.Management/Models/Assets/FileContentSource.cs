namespace Kontent.Ai.Management.Models.Assets;

/// <summary>
/// Wraps the information about file content source.
/// </summary>
public sealed class FileContentSource
{
    private readonly byte[]? _data;
    private readonly string? _filePath;
    private readonly Stream? _stream;

    internal bool CreatesNewStream { get; }

    /// <summary>
    /// Gets the media type of the asset, for example: "image/jpeg".
    /// </summary>
    public string ContentType { get; }

    /// <summary>
    /// Gets the name of the file.
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// Gets an open stream for the file data.
    /// </summary>
    /// <returns>The <see cref="Stream"/> instance that represents opened stream.</returns>
    public Stream OpenReadStream()
    {
        if (_stream is not null)
        {
            return _stream;
        }

        if (_data is not null)
        {
            return new MemoryStream(_data);
        }

        if (_filePath is not null)
        {
            return File.OpenRead(_filePath);
        }

        throw new InvalidOperationException("File content source does not have any source set.");
    }

    /// <summary>
    /// Creates content source file.
    /// </summary>
    /// <param name="data">Binary data of the file.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="contentType">The media type of the asset, for example: "image/jpeg".</param>
    public FileContentSource(byte[] data, string fileName, string contentType)
    {
        ArgumentNullException.ThrowIfNull(data);
        ArgumentException.ThrowIfNullOrEmpty(fileName);
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        _data = data;
        FileName = fileName;
        ContentType = contentType;
        CreatesNewStream = true;
    }

    /// <summary>
    /// Creates content source file.
    /// </summary>
    /// <param name="filePath">Path to file.</param>
    /// <param name="contentType">The media type of the asset, for example: "image/jpeg".</param>
    public FileContentSource(string filePath, string contentType)
    {
        ArgumentException.ThrowIfNullOrEmpty(filePath);
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        _filePath = filePath;
        FileName = Path.GetFileName(filePath);
        ContentType = contentType;
        CreatesNewStream = true;
    }

    /// <summary>
    /// Creates content source file.
    /// </summary>
    /// <param name="stream">Stream of the input data</param>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="contentType">The media type of the asset, for example: "image/jpeg".</param>
    public FileContentSource(Stream stream, string fileName, string contentType)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentException.ThrowIfNullOrEmpty(fileName);
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        _stream = stream;
        FileName = fileName;
        ContentType = contentType;
    }
}
