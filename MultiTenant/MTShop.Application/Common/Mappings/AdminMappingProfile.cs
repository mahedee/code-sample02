using AutoMapper;
using MTShop.Application.Commands.Tenant;
using MTShop.Application.DTOs;
using MTShop.Core.Entities.Admin;

namespace MTShop.Application.Common.Mappings
{
    public class AdminMappingProfile : Profile
    {
        public AdminMappingProfile()
        {
            // CreateMap<Source, Destination>()
            CreateMap<TenantCreateCommand, TenantEntity>();
            CreateMap<TenantEntity, TenantDTO>().ReverseMap();
            CreateMap<TenantUpdateCommand, TenantEntity>();
        }
    }
}
