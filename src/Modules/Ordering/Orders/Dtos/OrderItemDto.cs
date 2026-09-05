namespace EShop.Ordering.Orders.Dtos;

public record OrderItemDto(
    Guid Id,
    Guid ProductId,
    int Quantity,
    decimal Price
);
