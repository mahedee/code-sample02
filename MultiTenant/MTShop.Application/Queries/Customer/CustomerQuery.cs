using AutoMapper;
using MediatR;
using MTShop.Application.DTOs;
using MTShop.Application.Interfaces.Repositories;

namespace MTShop.Application.Queries.Customer
{

    public record CustomerQuery : IRequest<IEnumerable<CustomerDTO>>;

    public class CustomerQueryHandler : IRequestHandler<CustomerQuery, IEnumerable<CustomerDTO>>
    {
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;
        public CustomerQueryHandler(IMapper mapper, ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CustomerDTO>> Handle(CustomerQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<CustomerDTO>>(await _customerRepository.GetAllAsync());
            //throw new NotImplementedException();
        }
    }
}
