
namespace Eshop.Modules.Catalog.Products.Events;

public record ProductPriceChangedEvent(Product Product)
: IDomainEvent;