namespace Catalog.Api.Products.GetProducts;

public class GetProductsQueryHandler(IDocumentSession session) 
    : IQueryHandler<GetProductQuery, GetProductsResult>
{
    public async Task<GetProductsResult> HandleAsync(GetProductQuery query, CancellationToken cancellationToken)
    {
        var products = await session.Query<Product>().ToPagedListAsync(
            query.PageNumber ?? 1,
            query.PageSize ?? 10,
            cancellationToken);

        return new GetProductsResult(products);
    }
}
