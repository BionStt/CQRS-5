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
        private readonly INotificationService notificationService;

        public UnhandledExceptionBehavior(ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> logger,
                                          INotificationService notificationService)
        {
            this.logger = logger;
            this.notificationService = notificationService;
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
                await SendUnexpectedErrorMessage(ex, request);
                var response = new TResponse();
                response.ServerError();
                return response;
            }
        }

        private async Task SendUnexpectedErrorMessage(Exception ex, TRequest request)
        {
            var builder = new StringBuilder();
            builder.AppendLine("An unexpected error occured.");
            builder.AppendLine($"Request Type: {request.GetType().Name}");
            builder.AppendLine($"Request Body: {request.ToJson()}");
            await notificationService.Send
            (
                subject: "Unhandled Exception Occurred",
                body: builder.ToString(),
                ex: ex
            );
        }
    }
}
