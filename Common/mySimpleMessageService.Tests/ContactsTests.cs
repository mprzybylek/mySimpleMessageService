using AutoMapper;
using MediatR;
using Moq;
using mySimpleMessageService.API.Controllers;
using mySimpleMessageService.API.Infrastructure;
using mySimpleMessageService.Domain;
using mySimpleMessageService.Domain.Contact.Commands;
using mySimpleMessageService.Domain.Contact.Queries;
using mySimpleMessageService.Domain.Validators;
using mySimpleMessageService.Persistance.Repositories;
using NUnit.Framework;
using Persistance.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mySimpleMessageService.Tests
{
    public class ContactsTests
    {
        private Mock<IMediator> _mediator;
        private IMapper _mapper;
        private Mock<ContactsRepository> _contactRepository;

        [SetUp]
        public void Setup()
        {
            _mediator = new Mock<IMediator>();
            IQueryable<ContactEntity> _dataset = (new List<ContactEntity>
            {
                new ContactEntity
                {
                    Name="test",
                    Surname= "test"
                }
            }).AsQueryable();

            _contactRepository = new Mock<ContactsRepository>(null);
            _contactRepository.Setup(r => r.GetById(1)).Returns(Task.FromResult(new ContactEntity()));
            _contactRepository.Setup(r => r.GetById(2)).Returns(Task.FromResult(new ContactEntity()));
            _contactRepository.Setup(r => r.GetAll()).Returns(_dataset);

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiles>());
            _mapper = config.CreateMapper();
        }

        [Test]
        public async Task ContactController_GetContacts()
        {
            var contactController = new ContactsController(_mediator.Object);

            await contactController.Get();

            _mediator.Verify(x => x.Send(It.IsAny<ReadContactsQuery>(), It.IsAny<System.Threading.CancellationToken>()), Times.Once);

        }


        [Test]
        public async Task ContactController_UpdateContacts()
        {
            var contactController = new ContactsController(_mediator.Object);

            await contactController.Put(1,new UpdateContactCommand());

            _mediator.Verify(x => x.Send(It.IsAny<UpdateContactCommand>(), It.IsAny<System.Threading.CancellationToken>()), Times.Once);

        }
        [Test]
        public async Task ContactController_DeleteContacts()
        {
            var contactController = new ContactsController(_mediator.Object);

            await contactController.Delete(1);

            _mediator.Verify(x => x.Send(It.IsAny<DeleteContactCommand>(), It.IsAny<System.Threading.CancellationToken>()), Times.Once);

        }
        [Test]
        public async Task ContactController_CreateContacts()
        {
            var contactController = new ContactsController(_mediator.Object);

            await contactController.Post(new CreateContactCommand());

            _mediator.Verify(x => x.Send(It.IsAny<CreateContactCommand>(), It.IsAny<System.Threading.CancellationToken>()), Times.Once);

        }


        [Test]
        public async Task GetContactQueryHandler_ReturnsCorrectCollection()
        {
            var queryHandler = new ReadContactsQueryHandler(_contactRepository.Object);

            var dbCollection = _contactRepository.Object.GetAll();
            var output = await queryHandler.Handle(new ReadContactsQuery(), new System.Threading.CancellationToken());

            Assert.AreEqual(output.Count(), dbCollection.Count());
        }

        [Test]
        public async Task CreateContactHandler_AddNewContact()
        {
            var command = new CreateContactCommand
            {
                Name = "test",
                Surname = "test"
            };
            var contactHandler = new CreateContactCommandHandler(_contactRepository.Object,_mapper);
            
            await contactHandler.Handle(command, new System.Threading.CancellationToken());
            
            _contactRepository.Verify(x => x.Create(It.IsAny<ContactEntity>()), Times.Once);
        }

        [Test]
        public async Task DeleteContactHandler_DeleteExistContact()
        {
            var command = new DeleteContactCommand
            {
                 Id = 1
            };
            var validators = new List<IValidator<DeleteContactCommand>> { new ContactValidator(_contactRepository.Object) };
            var contactHandler = new DeleteContactCommandHandler(_contactRepository.Object, validators);

            await contactHandler.Handle(command, new System.Threading.CancellationToken());

            _contactRepository.Verify(x => x.Delete(It.IsAny<int>()), Times.Once);
        }
        [Test]
        public async Task DeleteContactHandler_DeleteNonExistContact()
        {
            var command = new DeleteContactCommand
            {
                Id = 100
            };
            var validators = new List<IValidator<DeleteContactCommand>> { new ContactValidator(_contactRepository.Object) };
            var contactHandler = new DeleteContactCommandHandler(_contactRepository.Object, validators);

            await contactHandler.Handle(command, new System.Threading.CancellationToken());

            _contactRepository.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
        }
        [Test]
        public async Task UpdateContactHandler_UpdateExistContact()
        {
            var command = new UpdateContactCommand
            {
                Id = 1
            };
            var validators = new List<IValidator<UpdateContactCommand>> { new ContactValidator(_contactRepository.Object) };
            var contactHandler = new UpdateContactCommandHandler(_contactRepository.Object,_mapper, validators);

            await contactHandler.Handle(command, new System.Threading.CancellationToken());

            _contactRepository.Verify(x => x.Update(It.IsAny<int>(),It.IsAny<ContactEntity>()), Times.Once);
        }
        [Test]
        public async Task UpdateContactHandler_UpdateNonExistContact()
        {
            var command = new UpdateContactCommand
            {
                Id = 100
            };
            var validators = new List<IValidator<UpdateContactCommand>> { new ContactValidator(_contactRepository.Object) };
            var contactHandler = new UpdateContactCommandHandler(_contactRepository.Object, _mapper, validators);

            await contactHandler.Handle(command, new System.Threading.CancellationToken());

            _contactRepository.Verify(x => x.Update(It.IsAny<int>(),It.IsAny<ContactEntity>()), Times.Never);
        }

    }
}
