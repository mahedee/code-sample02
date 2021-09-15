using MediatR;
using Ordering.Application.Commands;
using Ordering.Application.Mapper;
using Ordering.Application.Queries;
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
    public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerCommand, String>
    {
        private readonly ICustomerRepository _customerRepository;
        public DeleteCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<string> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var customerEntity = await _customerRepository.GetByIdAsync(request.Id);

                await _customerRepository.DeleteAsync(customerEntity);
            }
            catch(Exception exp)
            {
                throw (new ApplicationException(exp.Message));
            }

            return "Customer information has been deleted!";
        }
    }
}
