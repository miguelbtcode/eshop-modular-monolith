namespace Catalog.Products.Features.DeleteProduct;

public record DeleteProductCommand(Guid ProductId) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product ID must not be empty.");
    }
}

internal class DeleteProductHandler(CatalogDbContext dbContext)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(
        DeleteProductCommand command,
        CancellationToken cancellationToken
    )
    {
        // Delete product entity by ID
        var product =
            await dbContext.Products.FindAsync(
                [command.ProductId],
                cancellationToken: cancellationToken
            ) ?? throw new ProductNotFoundException(command.ProductId);

        // Save to database
        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        // Return result
        return new DeleteProductResult(true);
    }
}
