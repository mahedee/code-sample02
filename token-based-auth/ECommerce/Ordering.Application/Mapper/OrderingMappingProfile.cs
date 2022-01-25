using AutoMapper;
using Ordering.Application.Commands;
using Ordering.Application.Commands.Customers;
using Ordering.Application.DTOs;
using Ordering.Core.Entities;

namespace Ordering.Application.Mapper
{
    public class OrderingMappingProfile : Profile
    {
        public OrderingMappingProfile()
        {
            CreateMap<Customer, CustomerResponse>().ReverseMap();
            CreateMap<Customer, CreateCustomerCommand>().ReverseMap();
            CreateMap<Customer, EditCustomerCommand>().ReverseMap();
        }
    }
}
