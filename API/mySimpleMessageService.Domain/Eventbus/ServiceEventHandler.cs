using MediatR;
using Microsoft.Extensions.DependencyInjection;
using mySimpleMessageService.Persistance.Repositories;
using Newtonsoft.Json;
using Persistance;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace mySimpleMessageService.Domain.Eventbus
{
    public class ServiceEventHandler : INotificationHandler<ServiceEvent>
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceEventHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task Handle(ServiceEvent notification, CancellationToken cancellationToken)
        {
            using (IServiceScope scope =  _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<EventsRepository>();

                await context.Create(new Persistance.Entities.EventsEntity
                {
                    Date = DateTime.Now,
                    Event = JsonConvert.SerializeObject(notification),
                });
            }

        }
    }
}
