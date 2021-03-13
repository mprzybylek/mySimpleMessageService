using AutoMapper;
using MediatR;
using mySimpleMessageService.Domain.Eventbus;
using mySimpleMessageService.Persistance.Repositories;
using Persistance.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace mySimpleMessageService.Domain.Message.Commands
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Unit>
    {
        private readonly MessagesRepository _repository;
        private readonly IMapper _mapper;
        private readonly IEnumerable<IValidator<SendMessageCommand>> _validators;
        private readonly IMediator _mediator;

        public SendMessageCommandHandler(MessagesRepository repository,
            IMapper mapper,
            IEnumerable<IValidator<SendMessageCommand>> validators,
            IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _validators = validators;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            foreach (var validator in _validators)
            {
                if(!await validator.IsValid(request))
                {
                    await _mediator.Publish(new ServiceEvent
                    {
                        Message = "SendMessageCommand failed: " + validator.ValidatorName,
                        Status = true
                    });
                    return Unit.Value;
                }
            }

            await _repository.Create(_mapper.Map<MessageEntity>(request)).ContinueWith(x =>
            {
                _mediator.Publish(new ServiceEvent
                {
                    Message = "Message sent",
                    Status = true
                });
            });
            return Unit.Value;
        }
    }
}
