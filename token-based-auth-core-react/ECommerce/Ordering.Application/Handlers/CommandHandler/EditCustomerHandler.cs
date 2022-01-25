using MediatR;
using Ordering.Application.Commands;
using Ordering.Application.Commands.Customers;
using Ordering.Application.DTOs;
using Ordering.Application.Mapper;
using Ordering.Core.Entities;
using Ordering.Core.Repositories.Command;
using Ordering.Core.Repositories.Query;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers.CommandHandler
{
    // Customer edit command handler with customer response as output
    public class EditCustomerHandler : IRequestHandler<EditCustomerCommand, CustomerResponse>
    {
        private readonly ICustomerCommandRepository _customerCommandRepository;
        private readonly ICustomerQueryRepository _customerQueryRepository;
        public EditCustomerHandler(ICustomerCommandRepository customerRepository, ICustomerQueryRepository customerQueryRepository)
        {
            _customerCommandRepository = customerRepository;
            _customerQueryRepository = customerQueryRepository;
        }
        public async Task<CustomerResponse> Handle(EditCustomerCommand request, CancellationToken cancellationToken)
        {
            var customerEntity = CustomerMapper.Mapper.Map<Customer>(request);

            if (customerEntity is null)
            {
                throw new ApplicationException("There is a problem in mapper");
            }

            try
            {
                await _customerCommandRepository.UpdateAsync(customerEntity);
            }
            catch (Exception exp)
            {
                throw new ApplicationException(exp.Message);
            }

            var modifiedCustomer = await _customerQueryRepository.GetByIdAsync(request.Id);
            var customerResponse = CustomerMapper.Mapper.Map<CustomerResponse>(modifiedCustomer);

            return customerResponse;
        }
    }
}
