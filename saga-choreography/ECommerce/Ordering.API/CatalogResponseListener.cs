using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Ordering.API.Db;
using Plain.RabbitMQ;
using Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API
{
    public class CatalogResponseListener : IHostedService
    {
        private ISubscriber _subscriber;
        private readonly IServiceScopeFactory _scopeFactory;
        public CatalogResponseListener(ISubscriber subscripber, IServiceScopeFactory scopeFactory)
        {
            this._subscriber = subscripber;
            this._scopeFactory = scopeFactory;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _subscriber.Subscribe(Subscribe);
            return Task.CompletedTask;
        }

        private bool Subscribe(string message, IDictionary<string, object> header)
        {
            var response = JsonConvert.DeserializeObject<CatalogResponse>(message);

            if(!response.IsSuccess)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var _orderingContext = scope.ServiceProvider.GetRequiredService<OrderingContext>();
                    
                    // If transaction is not successful, Remove ordering item
                    var orderItem = _orderingContext.OrderItems.Where(o => o.ProductId == response.CatalogId && o.OrderId == response.OrderId).FirstOrDefault();
                    _orderingContext.OrderItems.Remove(orderItem);
                    _orderingContext.SaveChanges();
                }
            }
            return true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
