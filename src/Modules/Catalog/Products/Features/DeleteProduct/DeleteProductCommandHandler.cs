using EShop.Catalog.Products.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Catalog.Products.Features.DeleteProduct
{
    public record DeleteProductCommand(Guid Id)
        : ICommand<DeleteProductResult>;

   public record DeleteProductResult(bool IsSuccess);

    public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductValidator()
        {
            RuleFor(p => p.Id)
                 .Must(id => id != Guid.Empty)
                 .WithMessage("product id must not be empty");
        }
    }

    public class DeleteProductCommandHandler(CatalogDbContext dbContext)
        : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var product = await dbContext.Products.FindAsync([command.Id], cancellationToken);
            if (product is null)
                throw new ProductNotFoundException(command.Id);
        
            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new DeleteProductResult(true);
        }
    }
}
