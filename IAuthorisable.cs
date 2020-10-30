using System.Collections.Generic;

namespace CQRS.Interfaces
{
    public interface IAuthorisable
    {
        IEnumerable<IAuthorisationRequirement> Requirements { get; }
    }
}
