using CQRS.Models;
using MediatR;

namespace CQRS.Interfaces
{
    public interface ICommandHandler<T> : IRequestHandler<T, CQRSResponse>
        where T : ICommand
    {
    }
}
