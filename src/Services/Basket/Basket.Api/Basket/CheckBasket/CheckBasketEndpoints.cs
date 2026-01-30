namespace Basket.Api.Basket.CheckBasket;

public record CheckBasketResponse(bool IsSuccess);
public record CheckBasketRequest(string UserName);
public class CheckBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/checkbasket/{userName}", async (string userName, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new CheckBasketQuery(userName), cancellationToken);
            var response = result.Adapt<CheckBasketResponse>();
            return Results.Ok(response);
        })
        .WithName("CheckBasketExists")
        .Produces<CheckBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Check Basket Exists")
        .WithDescription("Checks if a basket exists for a given user.");
    }
}
