using MediatR;
using mySimpleMessageService.Domain.Eventbus;
using mySimpleMessageService.Persistance.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace mySimpleMessageService.Domain.Contact.Commands
{
    public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, Unit>
    {
        private readonly ContactsRepository _repository;
        private readonly IEnumerable<IValidator<DeleteContactCommand>> _validators;
        private readonly IMediator _mediator;

        public DeleteContactCommandHandler(ContactsRepository repository, 
            IEnumerable<IValidator<DeleteContactCommand>> validators,
            IMediator mediator)
        {
            _repository = repository;
            _validators = validators;
            _mediator = mediator;
        }
        public async Task<Unit> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            foreach (var validator in _validators)
            {
                if (!await validator.IsValid(request))
                {
                    await _mediator.Publish(new ServiceEvent
                    {
                        Message = "DeleteContactCommand failed: " + validator.ValidatorName,
                        RequestId = request.Id,
                        Status = false
                    });
                    return Unit.Value;
                }
            }

            var user = await _repository.GetById(request.Id);
            user.IsDeleted = true;
            await _repository.Update(request.Id, user)
                .ContinueWith(x =>
                {
                    _mediator.Publish(new ServiceEvent
                    {
                        Message = "Contact added",
                        RequestId = request.Id,
                        Status = true
                    });
                });

            return Unit.Value;   
        }
    }
}
