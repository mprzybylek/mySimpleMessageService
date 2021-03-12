using AutoMapper;
using MediatR;
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

        public CreateContactCommandHandler(ContactsRepository repository, IMapper mapper)
        {
            _contactRepository = repository;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            await _contactRepository.Create(_mapper.Map<ContactEntity>(request));
            return Unit.Value;
        }
    }
}
