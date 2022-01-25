using AutoMapper;
using System;

namespace Ordering.Application.Mapper
{
    public class CustomerMapper
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(()=>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<OrderingMappingProfile>();
            });

            var mapper = config.CreateMapper();
            return mapper;
        });

        public static IMapper Mapper => Lazy.Value;
    }
}
