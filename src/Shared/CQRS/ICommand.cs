using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Shared.CQRS
{
    public interface ICommand : ICommand<MediatR.Unit>
    {
    }

    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
