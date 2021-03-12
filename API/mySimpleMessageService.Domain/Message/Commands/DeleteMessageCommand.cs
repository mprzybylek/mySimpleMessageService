using MediatR;
using System.ComponentModel.DataAnnotations;

namespace mySimpleMessageService.Domain.Message.Commands
{
    public class DeleteMessageCommand : IRequest<Unit>
    {
        [Required]
        public int Id { get; set; }
    }
}
