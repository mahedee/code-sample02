using MediatR;
using Ordering.Core.Entities;
using System.Collections.Generic;

namespace Ordering.Application.Queries
{
    // Customer query with List<Customer> response
    public record GetAllCustomerQuery : IRequest<List<Customer>>
    {

    }
}
