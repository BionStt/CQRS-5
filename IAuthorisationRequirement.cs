using CQRS.Models;
using MediatR;

namespace CQRS.Interfaces
{
    public interface IAuthorisationRequirement : IRequest<CQRSResponse>
    {
    }
}
