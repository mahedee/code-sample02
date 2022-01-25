using MediatR;
using Ordering.Application.DTOs;
using Ordering.Application.Mapper;
using Ordering.Core.Entities;
using Ordering.Core.Repositories.Command;
using Ordering.Core.Repositories.Query;

namespace Ordering.Application.Commands.Customers
{
    // Customer create command with CustomerResponse
    public class EditCustomerCommand : IRequest<CustomerResponse>
    {

        public Int64 Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
    }

    public class EditCustomerCommandHandler : IRequestHandler<EditCustomerCommand, CustomerResponse>
    {
        private readonly ICustomerCommandRepository _customerCommandRepository;
        private readonly ICustomerQueryRepository _customerQueryRepository;
        public EditCustomerCommandHandler(ICustomerCommandRepository customerRepository, ICustomerQueryRepository customerQueryRepository)
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
