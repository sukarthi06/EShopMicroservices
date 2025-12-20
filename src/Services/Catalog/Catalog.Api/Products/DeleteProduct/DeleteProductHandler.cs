
using Catalog.Api.Products.UpdateProduct;

namespace Catalog.Api.Products.DeleteProduct
{

    public record DeleteProductResult(bool IsSuccess);
    public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required.");
        }
    }
    public class DeleteProducCommandtHandler(IDocumentSession session)
        : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            session.Delete<Product>(command.Id);
            await session.SaveChangesAsync(cancellationToken);

            return new DeleteProductResult(true);
        }
    }
}
