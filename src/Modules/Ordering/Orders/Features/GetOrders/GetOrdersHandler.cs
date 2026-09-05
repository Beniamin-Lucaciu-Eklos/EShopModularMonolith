using EShop.Shared.Pagination;

namespace EShop.Ordering.Orders.Features.GetOrders;

public record GetOrdersQuery(PaginationRequest PaginationRequest)
       : IQuery<GetOrdersResult>;

public record GetOrdersResult(PaginationResult<OrderDto> Orders);

public class GetOrdersHandler(OrderingDbContext dbContext)
    : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        (int pageIndex, int pageSize) = query.PaginationRequest;

        var ordersQuery = dbContext.Orders
            .Include(o => o.Items)
            .AsNoTracking()
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .OrderBy(p => p.OrderName)
            .AsQueryable();

        var totalCount = await ordersQuery.LongCountAsync(cancellationToken);
        var orders = await ordersQuery.ToListAsync(cancellationToken);
        var orderDtos = orders.Adapt<List<OrderDto>>();

        return new GetOrdersResult(
            new PaginationResult<OrderDto>(pageIndex,
                pageSize,
                totalCount,
                orderDtos));
    }
}
