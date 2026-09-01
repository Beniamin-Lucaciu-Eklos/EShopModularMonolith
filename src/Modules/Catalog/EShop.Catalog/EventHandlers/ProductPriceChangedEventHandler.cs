using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Catalog.EventHandlers
{
    public class ProductPriceChangedEventHandler
       : INotificationHandler<ProductPriceChangedEvent>
    {
        private readonly ILogger<ProductPriceChangedEvent> _logger;
        public ProductPriceChangedEventHandler(ILogger<ProductPriceChangedEvent> logger)
        {
            _logger = logger;
        }

        public Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
        {
            //TODO : publish product price changed
            _logger.LogInformation("Domain event handle {DomainEvent}", notification.GetType().Name);
            return Task.CompletedTask;
        }
    }
}
