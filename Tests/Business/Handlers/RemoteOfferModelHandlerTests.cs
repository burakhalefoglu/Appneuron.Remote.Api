
using Business.Handlers.RemoteOfferModels.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.RemoteOfferModels.Queries.GetRemoteOfferModelQuery;
using Entities.Concrete;
using static Business.Handlers.RemoteOfferModels.Queries.GetRemoteOfferModelsQuery;
using static Business.Handlers.RemoteOfferModels.Commands.CreateRemoteOfferModelCommand;
using Business.Handlers.RemoteOfferModels.Commands;
using Business.Constants;
using static Business.Handlers.RemoteOfferModels.Commands.UpdateRemoteOfferModelCommand;
using static Business.Handlers.RemoteOfferModels.Commands.DeleteRemoteOfferModelCommand;
using MediatR;
using System.Linq;
using FluentAssertions;
using MongoDB.Bson;

namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class RemoteOfferModelHandlerTests
    {
        Mock<IRemoteOfferModelRepository> _remoteOfferModelRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _remoteOfferModelRepository = new Mock<IRemoteOfferModelRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task RemoteOfferModel_GetQuery_Success()
        {
            //Arrange
            var query = new GetRemoteOfferModelQuery();

            _remoteOfferModelRepository.Setup(x => x.GetByIdAsync(It.IsAny<ObjectId>())).ReturnsAsync(new RemoteOfferModel()
//propertyler buraya yazılacak
//{																		
//RemoteOfferModelId = 1,
//RemoteOfferModelName = "Test"
//}
);

            var handler = new GetRemoteOfferModelQueryHandler(_remoteOfferModelRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.RemoteOfferModelId.Should().Be(1);

        }

        [Test]
        public async Task RemoteOfferModel_GetQueries_Success()
        {
            //Arrange
            var query = new GetRemoteOfferModelsQuery();

            _remoteOfferModelRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<RemoteOfferModel, bool>>>()))
                        .ReturnsAsync(new List<RemoteOfferModel> { new RemoteOfferModel() { /*TODO:propertyler buraya yazılacak RemoteOfferModelId = 1, RemoteOfferModelName = "test"*/ } }.AsQueryable());

            var handler = new GetRemoteOfferModelsQueryHandler(_remoteOfferModelRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<RemoteOfferModel>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task RemoteOfferModel_CreateCommand_Success()
        {
            RemoteOfferModel rt = null;
            //Arrange
            var command = new CreateRemoteOfferModelCommand();
            //propertyler buraya yazılacak
            //command.RemoteOfferModelName = "deneme";

            _remoteOfferModelRepository.Setup(x => x.GetByIdAsync(It.IsAny<ObjectId>()))
                        .ReturnsAsync(rt);

            _remoteOfferModelRepository.Setup(x => x.Add(It.IsAny<RemoteOfferModel>()));

            var handler = new CreateRemoteOfferModelCommandHandler(_remoteOfferModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task RemoteOfferModel_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateRemoteOfferModelCommand();
            //propertyler buraya yazılacak 
            //command.RemoteOfferModelName = "test";

            _remoteOfferModelRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<RemoteOfferModel, bool>>>()))
                                           .ReturnsAsync(new List<RemoteOfferModel> { new RemoteOfferModel() { /*TODO:propertyler buraya yazılacak RemoteOfferModelId = 1, RemoteOfferModelName = "test"*/ } }.AsQueryable());

            _remoteOfferModelRepository.Setup(x => x.Add(It.IsAny<RemoteOfferModel>()));

            var handler = new CreateRemoteOfferModelCommandHandler(_remoteOfferModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task RemoteOfferModel_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateRemoteOfferModelCommand();
            //command.RemoteOfferModelName = "test";

            _remoteOfferModelRepository.Setup(x => x.GetByIdAsync(It.IsAny<ObjectId>()))
                        .ReturnsAsync(new RemoteOfferModel() { /*TODO:propertyler buraya yazılacak RemoteOfferModelId = 1, RemoteOfferModelName = "deneme"*/ });

            _remoteOfferModelRepository.Setup(x => x.UpdateAsync(It.IsAny<ObjectId>(), It.IsAny<RemoteOfferModel>()));

            var handler = new UpdateRemoteOfferModelCommandHandler(_remoteOfferModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task RemoteOfferModel_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteRemoteOfferModelCommand();

            _remoteOfferModelRepository.Setup(x => x.GetByIdAsync(It.IsAny<ObjectId>()))
                        .ReturnsAsync(new RemoteOfferModel() { /*TODO:propertyler buraya yazılacak RemoteOfferModelId = 1, RemoteOfferModelName = "deneme"*/});

            _remoteOfferModelRepository.Setup(x => x.Delete(It.IsAny<RemoteOfferModel>()));

            var handler = new DeleteRemoteOfferModelCommandHandler(_remoteOfferModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

