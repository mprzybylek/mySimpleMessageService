using mySimpleMessageService.Domain.Message.Commands;
using mySimpleMessageService.Persistance.Repositories;
using System.Threading.Tasks;

namespace mySimpleMessageService.Domain.Message.Validators
{
    public class MessageValidator : IValidator<SendMessageCommand>, IValidator<DeleteMessageCommand>
    {
        private readonly ContactsRepository _repository;
        public string ValidatorName => "SenderReceiverMessageValidator";

        public MessageValidator(ContactsRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> IsValid(SendMessageCommand obj)
        {

            if (obj.SenderId == obj.ReceiverId) return false;
            if (!await IsUserExist(obj.SenderId)) return false;
            if (!await IsUserExist(obj.ReceiverId)) return false;

            return true;
        }

        private async Task<bool> IsUserExist(int id)
        {
            return await _repository.GetById(id) != null;
        }

        public Task<bool> IsValid(DeleteMessageCommand obj)
        {
            return IsUserExist(obj.Id);
        }
    }
}
