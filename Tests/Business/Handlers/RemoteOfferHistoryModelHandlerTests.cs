
using Business.Handlers.RemoteOfferHistoryModels.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.RemoteOfferHistoryModels.Queries.GetRemoteOfferHistoryModelQuery;
using Entities.Concrete;
using static Business.Handlers.RemoteOfferHistoryModels.Queries.GetRemoteOfferHistoryModelsQuery;
using static Business.Handlers.RemoteOfferHistoryModels.Commands.CreateRemoteOfferHistoryModelCommand;
using Business.Handlers.RemoteOfferHistoryModels.Commands;
using Business.Constants;
using static Business.Handlers.RemoteOfferHistoryModels.Commands.UpdateRemoteOfferHistoryModelCommand;
using static Business.Handlers.RemoteOfferHistoryModels.Commands.DeleteRemoteOfferHistoryModelCommand;
using MediatR;
using System.Linq;
using FluentAssertions;
using MongoDB.Bson;

namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class RemoteOfferHistoryModelHandlerTests
    {
        Mock<IRemoteOfferHistoryModelRepository> _remoteOfferHistoryModelRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _remoteOfferHistoryModelRepository = new Mock<IRemoteOfferHistoryModelRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task RemoteOfferHistoryModel_GetQuery_Success()
        {
            //Arrange
            var query = new GetRemoteOfferHistoryModelQuery();

            _remoteOfferHistoryModelRepository.Setup(x => x.GetByIdAsync(It.IsAny<ObjectId>())).ReturnsAsync(new RemoteOfferHistoryModel()
//propertyler buraya yazılacak
//{																		
//RemoteOfferHistoryModelId = 1,
//RemoteOfferHistoryModelName = "Test"
//}
);

            var handler = new GetRemoteOfferHistoryModelQueryHandler(_remoteOfferHistoryModelRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.RemoteOfferHistoryModelId.Should().Be(1);

        }

        [Test]
        public async Task RemoteOfferHistoryModel_GetQueries_Success()
        {
            //Arrange
            var query = new GetRemoteOfferHistoryModelsQuery();

            _remoteOfferHistoryModelRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<RemoteOfferHistoryModel, bool>>>()))
                        .ReturnsAsync(new List<RemoteOfferHistoryModel> { new RemoteOfferHistoryModel() { /*TODO:propertyler buraya yazılacak RemoteOfferHistoryModelId = 1, RemoteOfferHistoryModelName = "test"*/ } }.AsQueryable());

            var handler = new GetRemoteOfferHistoryModelsQueryHandler(_remoteOfferHistoryModelRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<RemoteOfferHistoryModel>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task RemoteOfferHistoryModel_CreateCommand_Success()
        {
            RemoteOfferHistoryModel rt = null;
            //Arrange
            var command = new CreateRemoteOfferHistoryModelCommand();
            //propertyler buraya yazılacak
            //command.RemoteOfferHistoryModelName = "deneme";

            _remoteOfferHistoryModelRepository.Setup(x => x.GetByIdAsync(It.IsAny<ObjectId>()))
                        .ReturnsAsync(rt);

            _remoteOfferHistoryModelRepository.Setup(x => x.Add(It.IsAny<RemoteOfferHistoryModel>()));

            var handler = new CreateRemoteOfferHistoryModelCommandHandler(_remoteOfferHistoryModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task RemoteOfferHistoryModel_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateRemoteOfferHistoryModelCommand();
            //propertyler buraya yazılacak 
            //command.RemoteOfferHistoryModelName = "test";

            _remoteOfferHistoryModelRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<RemoteOfferHistoryModel, bool>>>()))
                                           .ReturnsAsync(new List<RemoteOfferHistoryModel> { new RemoteOfferHistoryModel() { /*TODO:propertyler buraya yazılacak RemoteOfferHistoryModelId = 1, RemoteOfferHistoryModelName = "test"*/ } }.AsQueryable());

            _remoteOfferHistoryModelRepository.Setup(x => x.Add(It.IsAny<RemoteOfferHistoryModel>()));

            var handler = new CreateRemoteOfferHistoryModelCommandHandler(_remoteOfferHistoryModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task RemoteOfferHistoryModel_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateRemoteOfferHistoryModelCommand();
            //command.RemoteOfferHistoryModelName = "test";

            _remoteOfferHistoryModelRepository.Setup(x => x.GetByIdAsync(It.IsAny<ObjectId>()))
                        .ReturnsAsync(new RemoteOfferHistoryModel() { /*TODO:propertyler buraya yazılacak RemoteOfferHistoryModelId = 1, RemoteOfferHistoryModelName = "deneme"*/ });

            _remoteOfferHistoryModelRepository.Setup(x => x.UpdateAsync(It.IsAny<ObjectId>(), It.IsAny<RemoteOfferHistoryModel>()));

            var handler = new UpdateRemoteOfferHistoryModelCommandHandler(_remoteOfferHistoryModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task RemoteOfferHistoryModel_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteRemoteOfferHistoryModelCommand();

            _remoteOfferHistoryModelRepository.Setup(x => x.GetByIdAsync(It.IsAny<ObjectId>()))
                        .ReturnsAsync(new RemoteOfferHistoryModel() { /*TODO:propertyler buraya yazılacak RemoteOfferHistoryModelId = 1, RemoteOfferHistoryModelName = "deneme"*/});

            _remoteOfferHistoryModelRepository.Setup(x => x.Delete(It.IsAny<RemoteOfferHistoryModel>()));

            var handler = new DeleteRemoteOfferHistoryModelCommandHandler(_remoteOfferHistoryModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

