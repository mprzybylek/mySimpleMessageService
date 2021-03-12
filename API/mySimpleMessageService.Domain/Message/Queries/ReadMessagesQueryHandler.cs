using AutoMapper;
using mySimpleMessageService.Persistance.Repositories;
using mySimpleMessageService.Common.Models;
using Persistance.Entities;
using System.Linq;
using MediatR;
using System.Threading;
using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;

namespace mySimpleMessageService.Domain.Message.Queries
{
    public class ReadMessagesQueryHandler : IRequestHandler<ReadMessagesQuery, IQueryable<MessageReadModel>>
    {
        private readonly MessagesRepository _repository;

        private MapperConfiguration configuration = new MapperConfiguration(cfg =>
             cfg.CreateMap<MessageEntity, MessageReadModel>()
                .ForMember(d => d.From, opt => opt.MapFrom(s => s.ContactSent.Name))
                .ForMember(d => d.To, opt => opt.MapFrom(s => s.ContactReceived.Name))
                .ForMember(d => d.Text, opt => opt.MapFrom(s => s.Text)));

        public ReadMessagesQueryHandler(MessagesRepository repository)
        {
            _repository = repository;
        }
        public async Task<IQueryable<MessageReadModel>> Handle(ReadMessagesQuery request, CancellationToken cancellationToken)
        {
            return _repository.GetAll().ProjectTo<MessageReadModel>(configuration).AsQueryable();
        }
    }
}
