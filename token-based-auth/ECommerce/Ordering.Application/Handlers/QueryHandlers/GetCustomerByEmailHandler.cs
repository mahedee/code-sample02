using MediatR;
using Ordering.Application.Queries;
using Ordering.Core.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers.QueryHandlers
{
    // Get specific customer query handler with Customer response as output
    public class GetCustomerByEmailHandler : IRequestHandler<GetCustomerByEmailQuery, Customer>
    {
        private readonly IMediator _mediator;

        public GetCustomerByEmailHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<Customer> Handle(GetCustomerByEmailQuery request, CancellationToken cancellationToken)
        {
            var customers = await _mediator.Send(new GetAllCustomerQuery());
            var selectedCustomer = customers.FirstOrDefault(x => x.Email.ToLower().Contains(request.Email.ToLower()));
            return selectedCustomer;
        }
    }
}
