using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Basket.Basket.Features.DeleteBasket
{
    public record DeleteBasketCommand(string UserName)
          : ICommand<DeleteBasketResult>;

    public record DeleteBasketResult(bool IsSuccess);

    public class DeleteBasketValidator : AbstractValidator<DeleteBasketCommand>
    {
        public DeleteBasketValidator()
        {
            RuleFor(p => p.UserName)
                  .NotEmpty()
                  .WithMessage("basket UserName must not be empty");
        }
    }

    public class DeleteBasketCommandHandler(BasketDbContext dbContext)
       : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
        {
            var shoppingCart = await dbContext.ShoppingCarts.FindAsync([command.UserName], cancellationToken);
            if (shoppingCart is null)
                throw new ShoppingCartNotFoundException(command.UserName);

            dbContext.ShoppingCarts.Remove(shoppingCart);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new DeleteBasketResult(true);
        }
    }
}
