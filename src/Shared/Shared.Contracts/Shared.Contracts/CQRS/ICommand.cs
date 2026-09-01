using MediatR;

namespace Shared.Contracts.CQRS;

public interface ICommand : ICommand<MediatR.Unit>
{
}

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
