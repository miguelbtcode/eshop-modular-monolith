namespace Catalog.Products.Features.GetProductByCategory;

public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

public record GetProductByCategoryResult(IEnumerable<ProductDto> Products);

internal class GetProductByCategoryHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(
        GetProductByCategoryQuery query,
        CancellationToken cancellationToken
    )
    {
        // First: Get products by category from the dbContext
        var products = await dbContext
            .Products.AsNoTracking()
            .Where(p => p.Category.Contains(query.Category))
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        // Second: Map the products to ProductDto using Mapster
        var productsDto = products.Adapt<List<ProductDto>>();

        // Third: Return the result
        return new GetProductByCategoryResult(productsDto);
    }
}
