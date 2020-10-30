using CQRS.Models;
using MediatR;

namespace CQRS.Interfaces
{
    public interface IAuthorisationRequirementHandler<TRequest> : IRequestHandler<TRequest, CQRSResponse>
        where TRequest : IRequest<CQRSResponse> { }
}
