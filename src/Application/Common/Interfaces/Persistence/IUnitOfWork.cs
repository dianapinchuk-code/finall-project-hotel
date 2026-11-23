namespace Application.Common.Interfaces.Persistence;

public interface IUnitOfWork
{
    Task<IApplicationTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
}