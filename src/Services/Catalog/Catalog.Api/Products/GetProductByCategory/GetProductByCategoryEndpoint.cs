namespace Catalog.Api.Products.GetProductByCategory;

public class GetProductByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}",
            async (string category, IQueryExecutor executor) =>
            {
                var query = new GetProductByCategoryQuery(category);

                var result = await executor.ExecuteAsync<GetProductByCategoryQuery, GetProductByCategoryResult>(query);

                var response = result.Adapt<GetProductByCategoryResponse>();

                return Results.Ok(response);
            })
        .WithName("GetProductByCategory")
        .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Product By Category")
        .WithDescription("Get Product By Category");
    }
}
