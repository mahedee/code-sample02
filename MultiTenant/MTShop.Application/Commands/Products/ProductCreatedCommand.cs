using AutoMapper;
using MediatR;
using MTShop.Application.DTOs;
using MTShop.Application.Interfaces;
using MTShop.Application.Interfaces.Repositories;
using MTShop.Core.Entities;

namespace MTShop.Application.Commands.Products
{
    public class ProductCreatedCommand : IRequest<ProductDTO>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rate { get; set; }
    }

    public class ProductCreatedCommandHandler : IRequestHandler<ProductCreatedCommand, ProductDTO>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductCreatedCommandHandler(IMapper mapper, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            this._mapper = mapper;
            this._productRepository = productRepository;
            this._unitOfWork = unitOfWork;
        }

         public async Task<ProductDTO> Handle(ProductCreatedCommand request, CancellationToken cancellationToken)
        {
            var newProduct = _mapper.Map<Product>(request);
            var product = await _productRepository.CreateAsync(newProduct);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<ProductDTO>(product);
        }
    }
}
