using AutoMapper;
using MediatR;
using mySimpleMessageService.Domain.Eventbus;
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
        private readonly IMediator _mediator;

        public UpdateContactCommandHandler(ContactsRepository repository,
            IMapper mapper, 
            IEnumerable<IValidator<UpdateContactCommand>> validators,
            IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _validators = validators;
            _mediator = mediator;
        }
        public async Task<Unit> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {
            foreach (var validator in _validators)
            {
                if (!await validator.IsValid(request))
                {
                    await _mediator.Publish(new ServiceEvent
                    {
                        Message = "UpdateContactCommand failed: " + validator.ValidatorName,
                        RequestId = request.Id,
                        Status = false
                    });
                    return Unit.Value;
                }
            }

            await _repository.Update(request.Id, _mapper.Map<ContactEntity>(request))
                .ContinueWith(x =>
                {
                    _mediator.Publish(new ServiceEvent
                    {
                        Message = "Contact updated",
                        RequestId = request.Id,
                        Status = true
                    });
                });
            return Unit.Value;
        }
    }
}
