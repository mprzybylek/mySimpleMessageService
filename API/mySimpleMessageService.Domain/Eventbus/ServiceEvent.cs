using MediatR;

namespace mySimpleMessageService.Domain.Eventbus
{
    public class ServiceEvent : INotification
    {
        /// <summary>
        /// Is ok
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Entity Id
        /// </summary>
        public int RequestId { get; set; }
    }
}
