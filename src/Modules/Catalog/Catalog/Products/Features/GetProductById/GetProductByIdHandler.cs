namespace Catalog.Products.Features.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

public record GetProductByIdResult(ProductDto Product);

internal class GetProductByIdHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(
        GetProductByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        // First: Get the product by Id from the dbContext
        var product =
            await dbContext
                .Products.AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == query.Id, cancellationToken: cancellationToken)
            ?? throw new ProductNotFoundException(query.Id);

        // Second: Map the product to ProductDto using Mapster
        var productDto = product.Adapt<ProductDto>();

        // Third: Return the result
        return new GetProductByIdResult(productDto);
    }
}
