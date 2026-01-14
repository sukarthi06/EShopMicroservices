namespace Catalog.Api.Dtos;

#region Create
public record CreateProductCommand(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price
) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public record CreateProductResponse(Guid Id);

public record CreateProductRequest(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price
);
#endregion

#region Update
public record UpdateProductResponse(bool IsSuccess);
public record UpdateProductResult(bool IsSuccess);
public record UpdateProductRequest(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price);
public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : ICommand<UpdateProductResult>;
#endregion

#region Delete
public record DeleteProductResponse(bool IsSuccess);
public record DeleteProductResult(bool IsSuccess);
public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
#endregion

#region Get
public record GetProductsResponse(IEnumerable<Product> Products);
public record GetProductsResult(IEnumerable<Product> Products);
public record GetProductsRequest(int? PageNumber = 1, int? PageSize = 10);
public record GetProductQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;

#endregion

#region GetById
public record GetProductByIdResponse(Product Product);
public record GetProductByIdResult(Product Product);
public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
#endregion

#region GetByCategory
public record GetProductByCategoryResult(IEnumerable<Product> Products);
public record GetProductByCategoryResponse(IEnumerable<Product> Products);
public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
#endregion