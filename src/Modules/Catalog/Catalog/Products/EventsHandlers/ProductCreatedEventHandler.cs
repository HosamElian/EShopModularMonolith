namespace Catalog.Products.EventsHandlers;

internal class ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger)
    : INotificationHandler<ProductCreatedEvent>
{
    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain event handled {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}
