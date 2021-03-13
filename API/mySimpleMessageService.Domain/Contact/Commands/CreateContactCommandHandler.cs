using AutoMapper;
using MediatR;
using mySimpleMessageService.Domain.Eventbus;
using mySimpleMessageService.Persistance.Repositories;
using Persistance.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace mySimpleMessageService.Domain.Contact.Queries
{
    public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, Unit>
    {
        private readonly ContactsRepository _contactRepository;
        public IMapper _mapper { get; }

        private readonly IMediator _mediator;

        public CreateContactCommandHandler(ContactsRepository repository, IMapper mapper, IMediator mediator)
        {
            _contactRepository = repository;
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task<Unit> Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            await _contactRepository.Create(_mapper.Map<ContactEntity>(request))
                .ContinueWith( x =>
                {
                    _mediator.Publish(new ServiceEvent
                    {
                        Message = "Contact updated",
                        Status = true
                    });
                }
            );
            
            return Unit.Value;
        }
    }
}
