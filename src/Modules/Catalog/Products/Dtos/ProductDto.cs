using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Catalog.Products.Dtos
{
    public record ProductDto(Guid Id,
        string Name,
        List<string> Categories,
        string Description,
        string ImageFile,
        decimal Price);
}
