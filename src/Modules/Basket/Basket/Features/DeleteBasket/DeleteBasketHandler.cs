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

    public class DeleteBasketHandler(IBasketRepository basketRepository)
       : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
        {
            var result = await basketRepository.DeleteBasket(command.UserName, cancellationToken: cancellationToken);

            return new DeleteBasketResult(result);
        }
    }
}
