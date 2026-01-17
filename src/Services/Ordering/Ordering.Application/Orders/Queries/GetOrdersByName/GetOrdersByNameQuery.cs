namespace Ordering.Application.Orders.Queries.GetOrdersByName;

public sealed record GetOrdersByNameResult(IEnumerable<OrderDto> Orders);
public sealed record GetOrdersByNameQuery(string Name)
    : IRequest<GetOrdersByNameResult>;
