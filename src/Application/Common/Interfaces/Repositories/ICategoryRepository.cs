using Domain.Categories;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task<Option<Category>> GetByIdAsync(CategoryId id, CancellationToken cancellationToken);
    Task<Option<Category>> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<IReadOnlyList<Category>> GetByIdsAsync(IReadOnlyCollection<CategoryId> ids, CancellationToken cancellationToken);
    Task<Category> AddAsync(Category category, CancellationToken cancellationToken);
}