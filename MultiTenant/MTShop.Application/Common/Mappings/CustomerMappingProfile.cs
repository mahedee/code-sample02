using AutoMapper;
using MTShop.Application.Commands.Customer;
using MTShop.Application.DTOs;
using MTShop.Core.Entities;

namespace MTShop.Application.Common.Mappings
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<CustomerCreatedCommand, Customer>();
        }
    }
}
