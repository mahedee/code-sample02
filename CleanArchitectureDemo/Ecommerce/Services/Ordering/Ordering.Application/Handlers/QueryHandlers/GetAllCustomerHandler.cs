using MediatR;
using Ordering.Application.Queries;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers.QueryHandlers
{
    public class GetAllCustomerHandler : IRequestHandler<GetAllCustomerQuery, List<Customer>>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetAllCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<List<Customer>> Handle(GetAllCustomerQuery request, CancellationToken cancellationToken)
        {
            return (List<Customer>)await _customerRepository.GetAllAsync();
        }
    }
}
