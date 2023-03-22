using AutoMapper;
using MediatR;
using MTShop.Application.DTOs;
using MTShop.Application.Interfaces.Repositories;

namespace MTShop.Application.Queries.Products
{

    public record ProductQueryById(Guid Id) : IRequest<ProductDTO>;

    public class ProductQueryByIdIdHandler : IRequestHandler<ProductQueryById, ProductDTO>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        public ProductQueryByIdIdHandler(IMapper mapper, IProductRepository productRepository)
        {
            this._mapper = mapper;
            this._productRepository = productRepository;
        }

        public async Task<ProductDTO> Handle(ProductQueryById request, CancellationToken cancellationToken)
        {
            var customer = await _productRepository.GetByIdAsync(request.Id)
                ?? throw new Exception("No customer found with given id");
            return _mapper.Map<ProductDTO>(await _productRepository.GetByIdAsync(request.Id));
        }
    }
}
