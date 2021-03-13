using mySimpleMessageService.Domain.Message.Commands;
using mySimpleMessageService.Persistance.Repositories;
using System.Threading.Tasks;

namespace mySimpleMessageService.Domain.Message.Validators
{
    public class MessageValidator : IValidator<SendMessageCommand>, IValidator<DeleteMessageCommand>
    {
        private readonly ContactsRepository _contactRepository;
        private readonly MessagesRepository _messageRepository;

        public string ValidatorName => "SenderReceiverMessageValidator";

        public MessageValidator(ContactsRepository contactRepository, MessagesRepository messagesRepository)
        {
            _contactRepository = contactRepository;
            _messageRepository = messagesRepository;
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
            return await _contactRepository.GetById(id) != null;
        }

        public async Task<bool> IsValid(DeleteMessageCommand obj)
        {
            return await _messageRepository.GetById(obj.Id) != null;
        }
    }
}
