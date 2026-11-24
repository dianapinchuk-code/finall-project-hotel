using Application.Common.Interfaces.Queries;
using Domain.Categories;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using static LanguageExt.Prelude;

namespace Infrastructure.Persistence.Queries;

public class CategoryQueries(ApplicationDbContext context) : ICategoryQueries
{
    public async Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Categories
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<Category>> GetByIdAsync(CategoryId id, CancellationToken cancellationToken)
    {
        var entity = await context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        return Optional(entity);
    }
}