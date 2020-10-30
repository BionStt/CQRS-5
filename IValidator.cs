using CQRS.Models;
using System.Threading.Tasks;

namespace CQRS.Interfaces
{
    public interface IValidator<TRequest>
    {
        Task<CQRSResponse> Validate(TRequest request);
    }
}
