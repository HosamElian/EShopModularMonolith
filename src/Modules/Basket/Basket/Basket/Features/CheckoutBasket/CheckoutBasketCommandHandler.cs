using Shared.Messaging.Events;
using System.Text.Json;

namespace Basket.Basket.Features.CheckoutBasket;

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckout)
    : ICommand<CheckoutBasketCommandResult>;
public record CheckoutBasketCommandResult(bool IsSuccess);
public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckout).NotNull().WithMessage("BasketCheckoutDto can't be null");
        RuleFor(x => x.BasketCheckout.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

internal class CheckoutBasketCommandHandler(BasketDbContext dbContext)
    : ICommandHandler<CheckoutBasketCommand, CheckoutBasketCommandResult>
{
    public async Task<CheckoutBasketCommandResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var basket = await dbContext.ShoppingCarts
                .Include(s => s.Items)
                .SingleOrDefaultAsync(s => s.UserName == command.BasketCheckout.UserName, cancellationToken);
            if (basket == null)
            {
                throw new BasketNotFoundException(command.BasketCheckout.UserName);
            }

            var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>();

            eventMessage.TotalPrice = basket.TotalPrice;

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = typeof(BasketCheckoutIntegrationEvent).AssemblyQualifiedName!,
                Content = JsonSerializer.Serialize(eventMessage),
                OccuredOn = DateTime.UtcNow
            };
            dbContext.OutboxMessages.Add(outboxMessage);
            
            dbContext.ShoppingCarts.Remove(basket);

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new CheckoutBasketCommandResult(true);

        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return new CheckoutBasketCommandResult(false);
        }

        //var basket = await repository.GetBasket(command.BasketCheckout.UserName, true, cancellationToken);

        //var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>();
        //eventMessage.TotalPrice = basket.TotalPrice;

        //await bus.Send(eventMessage, cancellationToken);

        //await repository.DeleteBasket(basket.UserName, cancellationToken);
        //return new CheckoutBasketCommandResult(true);
    }
}