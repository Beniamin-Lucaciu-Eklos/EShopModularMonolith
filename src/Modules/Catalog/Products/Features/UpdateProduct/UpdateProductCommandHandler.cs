using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Catalog.Products.Features.UpdateProduct
{
    public record UpdateProductCommand(ProductDto Product)
        : ICommand<UpdateProductResult>;

   public record UpdateProductResult(bool IsSuccess);

    public class UpdateProductCommandHandler(CatalogDbContext dbContext)
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == command.Product.Id, cancellationToken);
            if (product is null)
                throw new ArgumentException($"Not found product {command.Product.Id}");

            UpdateProduct(product, command.Product);

            dbContext.Products.Update(product);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }

        private void UpdateProduct(Product product, ProductDto productDto)
        {
            product.Update(productDto.Name,
                productDto.Categories,
                product.Description,
                product.ImageFile,
                product.Price);            
        }
    }
}
