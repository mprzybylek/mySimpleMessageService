using MediatR;
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

        public DeleteMessageCommandHandler(MessagesRepository repository, IEnumerable<IValidator<DeleteMessageCommand>> validators)
        {
            _reposiotry = repository;
            _validators = validators;
        }
        public async Task<Unit> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            foreach (var validator in _validators)
            {
                if (!await validator.IsValid(request))
                {
                    return Unit.Value;
                }
            }

            await _reposiotry.Delete(request.Id);
            return Unit.Value;
        }
    }
}
