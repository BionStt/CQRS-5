using CQRS.Interfaces;
using CQRS.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Behaviours
{
    public class AuthorisationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : CQRSResponse, new()
    {
        private readonly ILogger<AuthorisationBehavior<TRequest, TResponse>> logger;
        private readonly IMediator mediator;

        public AuthorisationBehavior(ILogger<AuthorisationBehavior<TRequest, TResponse>> logger,
                                     IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if(request is IAuthorisable authorisable)
            {
                var name = request.GetType().Name;
                logger.LogInformation("{Request} - Authorisation configured.", name);

                foreach(var requirement in authorisable.Requirements)
                {
                    var result = await mediator.Send(requirement, cancellationToken);
                    if (result.IsUnauthorised)
                    {
                        logger.LogWarning("{Request} - {Requirement} Authorisation failed.", name, requirement.GetType().Name);
                        var response = new TResponse();
                        response.StatusCode = 401;
                        return response;
                    }
                }
            }

            // Go to the next behaviour in the pipeline
            return await next();
        }
    }
}
