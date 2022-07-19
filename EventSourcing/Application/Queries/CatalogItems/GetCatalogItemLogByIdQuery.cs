using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.CatalogItems
{
    public class GetCatalogItemLogByIdQuery : IRequest<List<object>>
    {
        public GetCatalogItemLogByIdQuery(Guid catalogItemId)
        {
            CatalogItemId = catalogItemId;
        }

        public Guid CatalogItemId { get; private set; }
    }

    public class GetCatalogItemLogByIdQueryHandler : IRequestHandler<GetCatalogItemLogByIdQuery, List<object>>
    {
        private readonly IAggregateRepository<CatalogItem, Guid> _aggregateRepository;

        public GetCatalogItemLogByIdQueryHandler(IAggregateRepository<CatalogItem, Guid> aggregateRepository)
        {
            _aggregateRepository = aggregateRepository;
        }

        public async Task<List<object>> Handle(GetCatalogItemLogByIdQuery request, CancellationToken cancellationToken)
        {
            var data = await _aggregateRepository.ReadEventsAsync(request.CatalogItemId, cancellationToken);
            return data.Values.ToList();
        }
    }
}
