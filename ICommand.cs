using CQRS.Models;
using MediatR;

namespace CQRS.Interfaces
{
    public interface ICommand : IRequest<CQRSResponse>
    {
    }
}
