using AutoMapper;
using mySimpleMessageService.Common.Models;
using mySimpleMessageService.Domain.Contact.Commands;
using mySimpleMessageService.Domain.Contact.Queries;
using mySimpleMessageService.Domain.Message.Commands;
using Persistance.Entities;

namespace mySimpleMessageService.API.Infrastructure
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ContactReadModel, ContactEntity>()
              .ReverseMap();
            CreateMap<CreateContactCommand, ContactEntity>();
            CreateMap<UpdateContactCommand, ContactEntity>();


            CreateMap<MessageReadModel, MessageEntity>()
                .ReverseMap();
            CreateMap<SendMessageCommand, MessageEntity>()
                .ForMember(d => d.ContactReceivedId, opt => opt.MapFrom(s=> s.ReceiverId))
                .ForMember(d => d.ContactSentId, opt => opt.MapFrom(s=> s.SenderId))
                .ForMember(d => d.Text, opt => opt.MapFrom(s=> s.Message));
        }
    }
}
