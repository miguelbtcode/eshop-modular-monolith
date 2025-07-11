namespace Catalog.Products.Features.GetProductById;

public record GetProductByIdResponse(ProductDto Product);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/products/{id}",
                async (Guid id, ISender sender) =>
                {
                    var query = new GetProductByIdQuery(id);
                    var result = await sender.Send(query);
                    var response = result.Adapt<GetProductByIdResponse>();

                    return Results.Ok(response);
                }
            )
            .WithName("GetProductById")
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get a product by Id")
            .WithDescription("Retrieves a product by its unique identifier.");
    }
}
