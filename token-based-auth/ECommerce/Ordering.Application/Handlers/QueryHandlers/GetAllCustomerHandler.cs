using MediatR;
using Ordering.Application.Queries;
using Ordering.Core.Entities;
using Ordering.Core.Repositories.Query;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers.QueryHandlers
{
    // Get all customer query handler with List<Customer> response as output
    public class GetAllCustomerHandler : IRequestHandler<GetAllCustomerQuery, List<Customer>>
    {
        private readonly ICustomerQueryRepository _customerQueryRepository;

        public GetAllCustomerHandler(ICustomerQueryRepository customerQueryRepository)
        {
            _customerQueryRepository = customerQueryRepository;
        }
        public async Task<List<Customer>> Handle(GetAllCustomerQuery request, CancellationToken cancellationToken)
        {
            return (List<Customer>)await _customerQueryRepository.GetAllAsync();
        }
    }
}
