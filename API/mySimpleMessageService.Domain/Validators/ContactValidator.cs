using mySimpleMessageService.Domain.Contact.Commands;
using mySimpleMessageService.Persistance.Repositories;
using System;
using System.Threading.Tasks;

namespace mySimpleMessageService.Domain.Validators
{
    public class ContactValidator : IValidator<UpdateContactCommand>, IValidator<DeleteContactCommand>
    {
        private readonly ContactsRepository _repository;
        public string ValidatorName => "ContactValidator";


        public ContactValidator(ContactsRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> IsValid(DeleteContactCommand obj)
        {
            return IsUserExist(obj.Id);
        }

        public Task<bool> IsValid(UpdateContactCommand obj)
        {
            return IsUserExist(obj.Id);
        }

        private async Task<bool> IsUserExist(int id)
        {
            var user = await _repository.GetById(id);
            return !(user == null || user.IsDeleted);
        }

    }
}
