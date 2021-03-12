using MediatR;
using mySimpleMessageService.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace mySimpleMessageService.Domain.Contact.Queries
{
    public class ReadContactsQuery : IRequest<IQueryable<ContactReadModel>>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
    }
}
