using eShop.BuildingBlocks.Pagination;

namespace Ordering.Application.Orders.Queries.GetOrders;

public sealed record GetOrdersResult(PaginatedResult<OrderDto> Orders);
public sealed record GetOrdersQuery(PaginationRequest PaginationRequest)
    : IRequest<GetOrdersResult>;
