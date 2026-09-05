using EShop.Shared.Exceptions;

namespace EShop.Ordering.Orders.Exceptions;

public class OrderNotFoundException : NotFoundException
{
    public OrderNotFoundException(Guid id)
           : base("Order", id)
    {
    }
}
