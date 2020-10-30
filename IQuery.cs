using CQRS.Models;
using MediatR;

namespace CQRS.Interfaces
{
    public interface IQuery : IRequest<CQRSResponse>
    {
    }
}
