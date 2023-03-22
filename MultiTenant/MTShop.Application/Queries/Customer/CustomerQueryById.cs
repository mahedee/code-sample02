using AutoMapper;
using MediatR;
using MTShop.Application.DTOs;
using MTShop.Application.Interfaces.Repositories;

namespace MTShop.Application.Queries.Customer
{
    public record CustomerQueryById(Guid Id) : IRequest<CustomerDTO>;

    public class CustomerQueryByIdHandler : IRequestHandler<CustomerQueryById, CustomerDTO>
    {
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;
        public CustomerQueryByIdHandler(IMapper mapper, ICustomerRepository customerRepository)
        {
            this._mapper = mapper;
            this._customerRepository = customerRepository;
        }

        public async Task<CustomerDTO> Handle(CustomerQueryById request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id)
                ?? throw new Exception("No customer found with given id");
            return _mapper.Map<CustomerDTO>(await _customerRepository.GetByIdAsync(request.Id));
        }
    }
}
