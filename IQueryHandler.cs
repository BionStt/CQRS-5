using CQRS.Models;
using MediatR;

namespace CQRS.Interfaces
{
    public interface IQueryHandler<T> : IRequestHandler<T, CQRSResponse>
        where T : IQuery
    {
    }
}
