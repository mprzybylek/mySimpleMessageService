using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using mySimpleMessageService.Common.Models;
using mySimpleMessageService.Persistance.Repositories;
using Persistance.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace mySimpleMessageService.Domain.Contact.Queries
{
    public class ReadContactsQueryHandler : IRequestHandler<ReadContactsQuery, IQueryable<ContactReadModel>>
    {
        private readonly ContactsRepository _repository;

        private MapperConfiguration configuration = new MapperConfiguration(cfg =>
             cfg.CreateMap<ContactReadModel, ContactEntity>().ReverseMap());
        public ReadContactsQueryHandler(ContactsRepository repository)
        {
            _repository = repository;
        }

        public async Task<IQueryable<ContactReadModel>> Handle(ReadContactsQuery request, CancellationToken cancellationToken)
        {
            return _repository.GetAll().ProjectTo<ContactReadModel>(configuration).AsQueryable();
        }
    }
}
