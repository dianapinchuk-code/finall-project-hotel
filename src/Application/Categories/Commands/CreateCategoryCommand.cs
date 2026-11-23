using Application.Categories.Exceptions;
using Application.Common.Interfaces.Repositories;
using Domain.Categories;
using LanguageExt;
using MediatR;

namespace Application.Categories.Commands;

public record CreateCategoryCommand : IRequest<Either<CategoryException, Category>>
{
    public required string Name { get; init; }
}

public class CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<CreateCategoryCommand, Either<CategoryException, Category>>
{
    public async Task<Either<CategoryException, Category>> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var existing = await categoryRepository.GetByNameAsync(request.Name, cancellationToken);

        return await existing.MatchAsync(
            c => (Either<CategoryException, Category>)new CategoryAlreadyExistsException(c.Id),
            () => CreateEntityAsync(request, cancellationToken));
    }

    private async Task<Either<CategoryException, Category>> CreateEntityAsync(
        CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var category = Category.New(CategoryId.New(), request.Name);
            return await categoryRepository.AddAsync(category, cancellationToken);
        }
        catch (Exception exception)
        {
            return new CategoryUnhandledException(CategoryId.Empty(), exception);
        }
    }
}