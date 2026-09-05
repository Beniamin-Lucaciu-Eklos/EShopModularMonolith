using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Ordering.Orders.Features.GetOrderById
{
    public record GetOrderByIdQuery(Guid Id) : IQuery<GetOrderByIdResult>;

    public record GetOrderByIdResult(OrderDto Order);

    public class GetOrderByIdHandler(OrderingDbContext dbContext) : IQueryHandler<GetOrderByIdQuery, GetOrderByIdResult>
    {
        public async Task<GetOrderByIdResult> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
             var product = await dbContext.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.Id);

            if (product is null)
                throw new OrderNotFoundException(request.Id);

            var orderDto = product.Adapt<OrderDto>();

            return new GetOrderByIdResult(orderDto);
        }
    }
}
