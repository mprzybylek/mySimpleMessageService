using MediatR;
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

        public DeleteContactCommandHandler(ContactsRepository repository, IEnumerable<IValidator<DeleteContactCommand>> validators)
        {
            _repository = repository;
            _validators = validators;
        }
        public async Task<Unit> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            foreach (var validator in _validators)
            {
                if (!await validator.IsValid(request))
                {
                    return Unit.Value;
                }
            }

            await _repository.Delete(request.Id);
            return Unit.Value;   
        }
    }
}
