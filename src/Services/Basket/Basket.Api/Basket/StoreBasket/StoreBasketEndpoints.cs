namespace Basket.Api.Basket.StoreBasket;

public sealed record StoreBasketRequest(ShoppingCart Cart);
public sealed record StoreBasketResponse(string UserName);

public class StoreBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (StoreBasketRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<StoreBasketCommand>();

            // Explicitly cast to ICommand<StoreBasketResult> to resolve ambiguity
            var result = await sender.Send(command, cancellationToken);

            var response = result.Adapt<StoreBasketResponse>();

            return Results.Created($"/basket/{response.UserName}", response);
        })
       .WithName("CreateProduct")
       .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
       .ProducesProblem(StatusCodes.Status400BadRequest)
       .WithSummary("Create Product")
       .WithDescription("Create Product");
    }
}
