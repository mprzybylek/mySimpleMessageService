using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using mySimpleMessageService.Common.Models;
using mySimpleMessageService.Persistance.Entities;
using mySimpleMessageService.Persistance.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace mySimpleMessageService.Domain.Eventbus
{
    public class ReadEventsQueryHandler : IRequestHandler<ReadEventsQuery, IQueryable<EventReadModel>>
    {
    private readonly EventsRepository _repository;

    private MapperConfiguration configuration = new MapperConfiguration(cfg =>
         cfg.CreateMap<EventReadModel, EventsEntity>().ReverseMap());
    public ReadEventsQueryHandler(EventsRepository repository)
    {
        _repository = repository;
    }

    public async Task<IQueryable<EventReadModel>> Handle(ReadEventsQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetAll().ProjectTo<EventReadModel>(configuration).AsQueryable();
    }
}
}
