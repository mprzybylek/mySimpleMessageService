using MediatR;
using mySimpleMessageService.Domain.Eventbus;
using mySimpleMessageService.Persistance.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace mySimpleMessageService.Domain.Message.Commands
{
    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, Unit>
    {
        private readonly MessagesRepository _reposiotry;
        private readonly IEnumerable<IValidator<DeleteMessageCommand>> _validators;
        private readonly IMediator _mediator;

        public DeleteMessageCommandHandler(MessagesRepository repository, 
                                            IEnumerable<IValidator<DeleteMessageCommand>> validators,
                                            IMediator mediator)
        {
            _reposiotry = repository;
            _validators = validators;
            _mediator = mediator;
        }
        public async Task<Unit> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            foreach (var validator in _validators)
            {
                if (!await validator.IsValid(request))
                {
                    await _mediator.Publish(new ServiceEvent
                    {
                        Message = "DeleteMessageCommand failed: " + validator.ValidatorName,
                        RequestId = request.Id,
                        Status = false
                    });
                    return Unit.Value;
                }
            }

            await _reposiotry.Delete(request.Id)
                .ContinueWith(x =>
                {
                    _mediator.Publish(new ServiceEvent
                    {
                        Message = "Message added",
                        RequestId = request.Id,
                        Status = true
                    });
                });

            return Unit.Value;
        }
    }
}
