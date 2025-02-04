using MassTransit;

namespace Catalog.Products.EventsHandlers;

internal class ProductCreatedEventHandler
    (IBus bus, ILogger<ProductCreatedEventHandler> logger)
    : INotificationHandler<ProductCreatedEvent>
{
    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain event handled {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
        //var integrationEvent = new ProductPriceChangedIntegrationEvent
        //{
        //    ProductId = notification.Product.Id,
        //    Name = notification.Product.Name,
        //    Category = notification.Product.Category,
        //    Description = notification.Product.Description,
        //    ImageFile = notification.Product.ImageFile,
        //    Price = notification.Product.Price 
        //};

        //await bus.Publish(integrationEvent, cancellationToken);
    }
}
