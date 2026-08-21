using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Catalog.Products.Features.CreateProduct
{
    public record CreateProductCommand(
        string Name,
        List<string> Categories,
        string Description,
        string ImageFile,
        decimal Price) : IRequest<CreateProductResult>;

    public record CreateProductResult(Guid id);

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand requestCommand, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
