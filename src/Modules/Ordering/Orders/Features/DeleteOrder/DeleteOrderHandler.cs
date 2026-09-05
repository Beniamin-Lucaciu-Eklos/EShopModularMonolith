namespace EShop.Ordering.Orders.Features.DeleteOrder;

public record DeleteOrderCommand(Guid Id)
        : ICommand<DeleteOrderResult>;

public record DeleteOrderResult(bool IsSuccess);

public class DeleteOrderValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderValidator()
    {
        RuleFor(p => p.Id)
             .Must(id => id != Guid.Empty)
             .WithMessage("order id must not be empty");
    }
}

public class DeleteProductCommandHandler(OrderingDbContext dbContext)
    : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
{
    public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders.FindAsync([command.Id], cancellationToken);
        if (order is null)
            throw new OrderNotFoundException(command.Id);

        dbContext.Orders.Remove(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteOrderResult(true);
    }
}
