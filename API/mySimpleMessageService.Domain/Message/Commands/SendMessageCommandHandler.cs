using AutoMapper;
using MediatR;
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
        public IMapper _mapper { get; }
        private IEnumerable<IValidator<SendMessageCommand>> _validators;

        public SendMessageCommandHandler(MessagesRepository repository,
            IMapper mapper,
            IEnumerable<IValidator<SendMessageCommand>> validators)
        {
            _repository = repository;
            _mapper = mapper;
            _validators = validators;
        }

        public async Task<Unit> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            foreach (var validator in _validators)
            {
                if(!await validator.IsValid(request))
                {
                    return Unit.Value;
                }
            }

            await _repository.Create(_mapper.Map<MessageEntity>(request));
            return Unit.Value;
        }
    }
}
