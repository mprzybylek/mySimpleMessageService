using AutoMapper;
using MediatR;
using mySimpleMessageService.Persistance.Repositories;
using Persistance.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace mySimpleMessageService.Domain.Contact.Commands
{
    public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, Unit>
    {
        private readonly ContactsRepository _repository;
        private readonly IMapper _mapper;
        private readonly IEnumerable<IValidator<UpdateContactCommand>> _validators;

        public UpdateContactCommandHandler(ContactsRepository repository,IMapper mapper, IEnumerable<IValidator<UpdateContactCommand>> validators)
        {
            _repository = repository;
            _mapper = mapper;
            _validators = validators;
        }
        public async Task<Unit> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {
            foreach (var validator in _validators)
            {
                if (!await validator.IsValid(request))
                {
                    return Unit.Value;
                }
            }

            await _repository.Update(request.Id, _mapper.Map<ContactEntity>(request));
            return Unit.Value;
        }
    }
}
