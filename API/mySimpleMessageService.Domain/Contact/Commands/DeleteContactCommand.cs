using MediatR;
using System.ComponentModel.DataAnnotations;

namespace mySimpleMessageService.Domain.Contact.Commands
{
    public class DeleteContactCommand : IRequest<Unit>
    {
        [Required]
        public int Id { get; set; }
    }
}
