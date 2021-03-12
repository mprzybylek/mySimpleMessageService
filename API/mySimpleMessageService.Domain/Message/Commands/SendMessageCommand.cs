using MediatR;
using System.ComponentModel.DataAnnotations;

namespace mySimpleMessageService.Domain.Message.Commands
{
    public class SendMessageCommand : IRequest<Unit>
    {
        [Required]
        public int SenderId { get; set; }
        [Required]
        public int ReceiverId { get; set; }
        [MinLength(1)]
        [MaxLength(255)]
        public string Message { get; set; }
    }
}
