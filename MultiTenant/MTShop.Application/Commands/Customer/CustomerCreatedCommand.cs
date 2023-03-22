using AutoMapper;
using MediatR;
using MTShop.Application.DTOs;
using MTShop.Application.Interfaces;
using MTShop.Application.Interfaces.Repositories;

namespace MTShop.Application.Commands.Customer
{
    public class CustomerCreatedCommand : IRequest<CustomerDTO>
    {
        public string FullName { get; set; }
        public string FathersName { get; set; }
        public string MothersName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class CustomerCreatedCommandHandler : IRequestHandler<CustomerCreatedCommand, CustomerDTO>
    {
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerCreatedCommandHandler(IMapper mapper, ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerDTO> Handle(CustomerCreatedCommand request, CancellationToken cancellationToken)
        {
            var newCustomer = _mapper.Map<MTShop.Core.Entities.Customer>(request);
            var customer =  await _customerRepository.CreateAsync(newCustomer);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<CustomerDTO>(customer);
        }
    }

}
