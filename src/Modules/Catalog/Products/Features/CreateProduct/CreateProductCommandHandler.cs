using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EShop.Catalog.Products.Features.CreateProduct
{
    public record CreateProductCommand(ProductDto Product) : ICommand<CreateProductResult>;

    public record CreateProductResult(Guid id);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(p => p.Product.Name)
                .NotEmpty().WithMessage("Name is Required");
            RuleFor(p => p.Product.Categories)
                .NotEmpty().WithMessage("Categories is Required");
            RuleFor(p => p.Product.ImageFile)
                .NotEmpty().WithMessage("ImageFile is Required");
            RuleFor(p => p.Product.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }

    public class CreateProductCommandHandler
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        private readonly CatalogDbContext _dbContext;
        private readonly ILogger<CreateProductCommandHandler> _logger;

        public CreateProductCommandHandler(
            CatalogDbContext dbContext,
            ILogger<CreateProductCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("start creating");

            var product = CreateNewProduct(command.Product);

            await _dbContext.Products.AddAsync(product, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CreateProductResult(product.Id);
        }

        private Product CreateNewProduct(ProductDto productDto)
        {
            var product = Product.Create(
                Guid.NewGuid(),
                productDto.Name,
                productDto.Categories,
                productDto.Description,
                productDto.ImageFile,
                productDto.Price);

            return product;
        }
    }
}
