using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Messaging.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Catalog.Products.EventHandlers
{
    public class ProductPriceChangedEventHandler
       : INotificationHandler<ProductPriceChangedEvent>
    {
        private readonly IBus _bus;
        private readonly ILogger<ProductPriceChangedEventHandler> _logger;
        public ProductPriceChangedEventHandler(IBus bus,
            ILogger<ProductPriceChangedEventHandler> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        public async Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
        {
            //TODO : publish product price changed
            _logger.LogInformation("Domain event handle {DomainEvent}", notification.GetType().Name);

            var integrationEvent = new ProductPriceChangedIntegrationEvent
            {
                ProductId = notification.Product.Id,
                Name = notification.Product.Name,
                Categories = notification.Product.Categories,
                Description = notification.Product.Description,
                ImageFile = notification.Product.ImageFile,
                Price = notification.Product.Price
            };

            await _bus.Publish(integrationEvent, cancellationToken);
        }
    }
}
