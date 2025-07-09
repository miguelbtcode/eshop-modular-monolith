namespace Catalog.Products.Features.GetProducts;

public record GetProductsQuery() : IQuery<GetProductsResult>;

public record GetProductsResult(IEnumerable<ProductDto> Products);

internal class GetProductsHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(
        GetProductsQuery query,
        CancellationToken cancellationToken
    )
    {
        // First: Get products using dbContext
        var products = await dbContext
            .Products.AsNoTracking()
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        // Second: Map products to ProductDto using Mapster
        var productsDto = products.Adapt<List<ProductDto>>();

        // Third: Return the result
        return new GetProductsResult(productsDto);
    }
}
