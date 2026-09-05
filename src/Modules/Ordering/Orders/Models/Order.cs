namespace EShop.Ordering.Orders.Models;

public class Order : Aggregate<Guid>
{
    private readonly List<OrderItem> _items = [];

    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();

    public Guid CustomerId { get; private set; } = default!;

    public string OrderName { get; private set; } = default!;

    public Address ShippingAddress { get; private set; } = default!;

    public Address BillingAdddress { get; private set; } = default!;

    public Payment Payment { get; private set; } = default!;

    public decimal TotalPrice => _items.Sum(item => item.Price * item.Quantity);

    public static Order Create(Guid id,
        Guid customerId,
        string orderName,
        Address shippingAddress,
        Address billingAdddress,
        Payment payment)
    {
        var order = new Order
        {
            Id = id,
            CustomerId = customerId,
            OrderName = orderName,
            ShippingAddress = shippingAddress,
            BillingAdddress = billingAdddress,
            Payment = payment
        };

        order.AddDomainEvent(new OrderCreatedEvent(order));

        return order;
    }

    public void Add(Guid productId, int quantity, decimal price)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var existingItem = _items.FirstOrDefault(item => item.ProductId == productId);

        if (existingItem is not null)
        {
            existingItem.Quantity += quantity;
            return;
        }

        var orderItem = new OrderItem(productId, quantity, price);
        _items.Add(orderItem);
    }

    public void Remove(Guid productId)
    {
        var orderItem = _items.FirstOrDefault(item => item.ProductId == productId);
        if (orderItem is null)
            return;

        _items.Remove(orderItem);
    }
}
