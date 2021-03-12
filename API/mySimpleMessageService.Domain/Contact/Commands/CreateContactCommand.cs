using MediatR;
using System.ComponentModel.DataAnnotations;

namespace mySimpleMessageService.Domain.Contact.Queries
{
    public class CreateContactCommand : IRequest<Unit>
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Surname { get; set; }
    }
}
