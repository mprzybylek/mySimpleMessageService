using AutoMapper;
using MediatR;
using Moq;
using mySimpleMessageService.API.Controllers;
using mySimpleMessageService.API.Infrastructure;
using mySimpleMessageService.Domain;
using mySimpleMessageService.Domain.Message.Commands;
using mySimpleMessageService.Domain.Message.Queries;
using mySimpleMessageService.Domain.Message.Validators;
using mySimpleMessageService.Persistance.Repositories;
using NUnit.Framework;
using Persistance.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mySimpleMessageService.Tests
{
    public class MessageTests
    {
        private Mock<IMediator> _mediator;
        private Mock<MessagesRepository> _messageRepository;
        private IMapper _mapper;
        private Mock<ContactsRepository> _contactRepository;

        [SetUp]
        public void Setup()
        {
            _mediator = new Mock<IMediator>();
            _messageRepository = new Mock<MessagesRepository>(null);
            IQueryable<MessageEntity> _dataset = (new List<MessageEntity>
            {
                new MessageEntity
                { 
                    ContactReceivedId = 1,
                    ContactSentId = 2,
                    Text="Message",
                    ContactReceived = new ContactEntity
                    {
                        Id = 1,
                        Name = "Test",
                        Surname = "Test"
                    },
                    ContactSent = new ContactEntity
                    {
                        Id = 2,
                        Name = "Test1",
                        Surname = "Test2"
                    }
                }
            }).AsQueryable();
            _messageRepository.Setup(r => r.GetAll()).Returns(_dataset);
            _messageRepository.Setup(r => r.GetById(1)).Returns(Task.FromResult(new MessageEntity()));

            _contactRepository = new Mock<ContactsRepository>(null);
            _contactRepository.Setup(r => r.GetById(1)).Returns(Task.FromResult(new ContactEntity()));
            _contactRepository.Setup(r => r.GetById(2)).Returns(Task.FromResult(new ContactEntity()));



            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiles>());
            _mapper = config.CreateMapper();
        }

        [Test]
        public async Task MessageController_SendsCorrectMessageQuery()
        {
            var messagesController = new MessagesController(_mediator.Object);
            
            await messagesController.Get();

            _mediator.Verify(x => x.Send(It.IsAny<ReadMessagesQuery>(), It.IsAny<System.Threading.CancellationToken>()), Times.Once);

        }

        [Test]
        public async Task ReadMessagesQueryHandler_ReturnsCorrectCollection()
        {
            var queryHandler = new ReadMessagesQueryHandler(_messageRepository.Object);
            
            var dbCollection = _messageRepository.Object.GetAll();
            var output  = await queryHandler.Handle(new ReadMessagesQuery(), new System.Threading.CancellationToken());

            Assert.AreEqual(output.Count(), dbCollection.Count());
        }

        [Test]
        public async Task MessageController_SendMMessageCommand()
        {
            var messagesController = new MessagesController(_mediator.Object);
            var messageWithoutSender = new SendMessageCommand();

            await messagesController.Post(messageWithoutSender);

            _mediator.Verify(x => x.Send(It.IsAny<SendMessageCommand>(), It.IsAny<System.Threading.CancellationToken>()), Times.Once);

        }

        [Test]
        public async Task SendMessageCommandHandler_SendMessageWithoutReceiver()
        {
            var messageWithoutReceiver = new SendMessageCommand
            {
                Message = "Test",
                SenderId = 1
            };
            var validators = new List<IValidator<SendMessageCommand>> { new MessageValidator(_contactRepository.Object)};
            var messageCommandHandler = new SendMessageCommandHandler(_messageRepository.Object, _mapper, validators);

            await messageCommandHandler.Handle(messageWithoutReceiver, new System.Threading.CancellationToken());


            _messageRepository.Verify(x => x.Create(It.IsAny<MessageEntity>()), Times.Never);
        }
        [Test]
        public async Task SendMessageCommandHandler_SendMessageWihSameSenderReceiver()
        {
            var messageWithoutReceiver = new SendMessageCommand
            {
                Message = "Test",
                SenderId = 1,
                ReceiverId = 1
            };
            var validators = new List<IValidator<SendMessageCommand>> { new MessageValidator(_contactRepository.Object) };
            var messageCommandHandler = new SendMessageCommandHandler(_messageRepository.Object, _mapper, validators);
            
            await messageCommandHandler.Handle(messageWithoutReceiver, new System.Threading.CancellationToken());

            _messageRepository.Verify(x => x.Create(It.IsAny<MessageEntity>()), Times.Never);
        }

        [Test]
        public async Task SendMessageCommandHandler_SendCorrectMessage()
        {
            var messageWithoutReceiver = new SendMessageCommand
            {
                Message = "Test",
                SenderId = 1,
                ReceiverId = 2
            };
            var validators = new List<IValidator<SendMessageCommand>> { new MessageValidator(_contactRepository.Object) };
            var messageCommandHandler = new SendMessageCommandHandler(_messageRepository.Object, _mapper, validators);
            
            await messageCommandHandler.Handle(messageWithoutReceiver, new System.Threading.CancellationToken());

            _messageRepository.Verify(x => x.Create(It.IsAny<MessageEntity>()), Times.Once);
        }

        [Test]
        public async Task MessageController_DeleteMessage()
        {
            var messagesController = new MessagesController(_mediator.Object);
            var deleteMessageCommand = new DeleteMessageCommand();

            await messagesController.Delete(deleteMessageCommand.Id);

            _mediator.Verify(x => x.Send(It.IsAny<DeleteMessageCommand>(), It.IsAny<System.Threading.CancellationToken>()), Times.Once);

        }

        [Test]
        public async Task DeleteMessageCommandHandler_DeleteExistMessage()
        {
            var messageToDelete = new DeleteMessageCommand
            {
                Id = 1
            };
            var validators = new List<IValidator<DeleteMessageCommand>> { new MessageValidator(_contactRepository.Object) };
            var messageCommandHandler = new DeleteMessageCommandHandler(_messageRepository.Object, validators);

            await messageCommandHandler.Handle(messageToDelete, new System.Threading.CancellationToken());

            _messageRepository.Verify(x => x.Delete(It.IsAny<int>()), Times.Once);
        }


        [Test]
        public async Task DeleteMessageCommandHandler_TryDeleteNotExistMessage()
        {
            var messageToDelete = new DeleteMessageCommand
            {
                Id = 100
            };
            var validators = new List<IValidator<DeleteMessageCommand>> { new MessageValidator(_contactRepository.Object) };
            var messageCommandHandler = new DeleteMessageCommandHandler(_messageRepository.Object, validators);

            await messageCommandHandler.Handle(messageToDelete, new System.Threading.CancellationToken());

            _messageRepository.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
        }
    }
}