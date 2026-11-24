using Application.Common.Interfaces.Services;

namespace Infrastructure.Services;

public class LocalFileStorage : IFileStorage
{
    private readonly string _basePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

    public async Task SaveAsync(string relativePath, Stream content, CancellationToken cancellationToken)
    {
        var fullPath = Path.Combine(_basePath, relativePath);
        var directory = Path.GetDirectoryName(fullPath);

        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await using var fileStream = File.Create(fullPath);
        await content.CopyToAsync(fileStream, cancellationToken);
    }
}