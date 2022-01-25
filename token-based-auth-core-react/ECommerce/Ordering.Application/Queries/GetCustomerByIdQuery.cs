using MediatR;
using Ordering.Core.Entities;
using System;

namespace Ordering.Application.Queries
{
    // Customer GetCustomerByIdQuery with Customer response
    public class GetCustomerByIdQuery: IRequest<Customer>
    {
        public Int64 Id { get; private set; }
        
        public GetCustomerByIdQuery(Int64 Id)
        {
            this.Id = Id;
        }

    }
}
