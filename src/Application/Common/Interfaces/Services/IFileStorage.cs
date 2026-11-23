namespace Application.Common.Interfaces.Services;

public interface IFileStorage
{
    Task SaveAsync(string relativePath, Stream content, CancellationToken cancellationToken);
}