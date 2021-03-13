using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace mySimpleMessageService.Domain.Contact.Commands
{
    public class UpdateContactCommand : IRequest<Unit>
    {
        [Required]
        [JsonIgnore]
        public int Id { get; set; }
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
