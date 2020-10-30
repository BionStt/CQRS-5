using CQRS.Models;
using Interfaces;
using GCAA.IEMS.PI.Common.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Behaviours
{
    public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : CQRSResponse, new()
    {
        private readonly ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> logger;

        public UnhandledExceptionBehavior(ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> logger)
        {
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Request} - Unhandled exception caught.", typeof(TRequest).Name);
                var response = new TResponse();
                // TODO: Send notification if required
                response.ServerError();
                return response;
            }
        }
    }
}
