using AutoMapper;
using MediatR;
using MTShop.Application.DTOs;
using MTShop.Application.Interfaces.Repositories;

namespace MTShop.Application.Queries.Products
{
    public record ProductQuery : IRequest<IEnumerable<ProductDTO>>;

    public class ProductQueryHandler : IRequestHandler<ProductQuery, IEnumerable<ProductDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public ProductQueryHandler(IMapper mapper, IProductRepository productRepository)
        {
            this._mapper = mapper;
            this._productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductDTO>> Handle(ProductQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<ProductDTO>>(await _productRepository.GetAllAsync());

        }
    }
}
