using MediatR;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Shared.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[Start] handle request={TRequest} - Response={Response} - {Value}",
                typeof(TRequest).Name, typeof(TResponse).Name, request);

            var timer = Stopwatch.StartNew();
            var response = await next();
            timer.Stop();
            var timeTaken = timer.Elapsed;

            if (timeTaken.Seconds > 3)
                _logger.LogWarning("[PERFORMANCE] The request {TRequest} took {TimeTaken} seconds", typeof(TRequest).Name, timeTaken);

            _logger.LogInformation("[END] handle request={TRequest} - Response={Response} - {Value}",
                typeof(TRequest).Name, typeof(TResponse).Name, request);
            return response;
        }
    }
}
