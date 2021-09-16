using MediatR;
using Ordering.Core.Entities;

namespace Ordering.Application.Queries
{

    public class GetCustomerByEmailQuery: IRequest<Customer>
    {
        public string Email { get; private set; }
        
        public GetCustomerByEmailQuery(string email)
        {
            this.Email = email;
        }

    }
}
