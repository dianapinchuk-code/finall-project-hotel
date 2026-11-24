using Application.Common.Interfaces.Repositories;
using Domain.Categories;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using static LanguageExt.Prelude;

namespace Infrastructure.Persistence.Repositories;

public class CategoryRepository(ApplicationDbContext context) : ICategoryRepository
{
    public async Task<Option<Category>> GetByIdAsync(CategoryId id, CancellationToken cancellationToken)
    {
        var entity = await context.Categories
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        return Optional(entity);
    }

    public async Task<Option<Category>> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        var entity = await context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == name, cancellationToken);

        return Optional(entity);
    }

    public async Task<IReadOnlyList<Category>> GetByIdsAsync(
        IReadOnlyCollection<CategoryId> ids,
        CancellationToken cancellationToken)
    {
        return await context.Categories
            .Where(c => ids.Contains(c.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<Category> AddAsync(Category category, CancellationToken cancellationToken)
    {
        await context.Categories.AddAsync(category, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return category;
    }
}