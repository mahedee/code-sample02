using AutoMapper;
using MTShop.Application.Commands.Products;
using MTShop.Application.DTOs;
using MTShop.Core.Entities;

namespace MTShop.Application.Common.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<ProductCreatedCommand, Product>();
        }
    }
}
