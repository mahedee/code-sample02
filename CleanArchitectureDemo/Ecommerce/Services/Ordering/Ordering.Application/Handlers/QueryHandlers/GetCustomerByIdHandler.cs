using MediatR;
using Ordering.Application.Queries;
using Ordering.Core.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers.QueryHandlers
{
    // Get specific query handler with Customer response as output
    public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, Customer>
    {
        private readonly IMediator _mediator;

        public GetCustomerByIdHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<Customer> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customers = await _mediator.Send(new GetAllCustomerQuery());
            var selectedCustomer = customers.FirstOrDefault(x => x.Id == request.Id);
            return selectedCustomer;
        }
    }
}
