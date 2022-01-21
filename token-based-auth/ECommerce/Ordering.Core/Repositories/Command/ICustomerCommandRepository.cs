using Ordering.Core.Entities;
using Ordering.Core.Repositories.Command.Base;

namespace Ordering.Core.Repositories.Command
{
    // Interface for customer command repository
    public interface ICustomerCommandRepository : ICommandRepository<Customer>
    {

    }
}
