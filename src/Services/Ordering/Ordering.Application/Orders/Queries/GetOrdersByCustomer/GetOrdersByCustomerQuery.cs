namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer;

public sealed record GetOrdersByCustomerResult(IEnumerable<OrderDto> Orders);
public sealed record GetOrdersByCustomerQuery(Guid CustomerId)
    : IRequest<GetOrdersByCustomerResult>;
