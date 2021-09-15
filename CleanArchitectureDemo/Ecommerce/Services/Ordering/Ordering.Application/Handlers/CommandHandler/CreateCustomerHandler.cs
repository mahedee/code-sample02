using MediatR;
using Ordering.Application.Commands;
using Ordering.Application.Mapper;
using Ordering.Application.Response;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers.CommandHandler
{
    public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CustomerResponse>
    {
        private readonly ICustomerRepository _customerRepository;
        public CreateCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<CustomerResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customerEntity = CustomerMapper.Mapper.Map<Customer>(request);

            if(customerEntity is null)
            {
                throw new ApplicationException("There is a problem in mapper");
            }

            var newCustomer = await _customerRepository.AddAsync(customerEntity);
            var customerResponse = CustomerMapper.Mapper.Map<CustomerResponse>(newCustomer);
            return customerResponse;
        }
    }
}
