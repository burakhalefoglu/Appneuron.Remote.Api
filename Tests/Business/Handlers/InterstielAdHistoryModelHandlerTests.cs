
using Business.Handlers.InterstielAdHistoryModels.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.InterstielAdHistoryModels.Queries.GetInterstielAdHistoryModelQuery;
using Entities.Concrete;
using static Business.Handlers.InterstielAdHistoryModels.Queries.GetInterstielAdHistoryModelsQuery;
using static Business.Handlers.InterstielAdHistoryModels.Commands.CreateInterstielAdHistoryModelCommand;
using Business.Handlers.InterstielAdHistoryModels.Commands;
using Business.Constants;
using static Business.Handlers.InterstielAdHistoryModels.Commands.UpdateInterstielAdHistoryModelCommand;
using static Business.Handlers.InterstielAdHistoryModels.Commands.DeleteInterstielAdHistoryModelCommand;
using MediatR;
using System.Linq;
using FluentAssertions;
using MongoDB.Bson;

namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class InterstielAdHistoryModelHandlerTests
    {
        Mock<IInterstielAdHistoryModelRepository> _interstielAdHistoryModelRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _interstielAdHistoryModelRepository = new Mock<IInterstielAdHistoryModelRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task InterstielAdHistoryModel_GetQuery_Success()
        {
            //Arrange
            var query = new GetInterstielAdHistoryModelQuery();

            _interstielAdHistoryModelRepository.Setup(x => x.GetByIdAsync(It.IsAny<ObjectId>())).ReturnsAsync(new InterstielAdHistoryModel()
//propertyler buraya yazılacak
//{																		
//InterstielAdHistoryModelId = 1,
//InterstielAdHistoryModelName = "Test"
//}
);

            var handler = new GetInterstielAdHistoryModelQueryHandler(_interstielAdHistoryModelRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.InterstielAdHistoryModelId.Should().Be(1);

        }

        [Test]
        public async Task InterstielAdHistoryModel_GetQueries_Success()
        {
            //Arrange
            var query = new GetInterstielAdHistoryModelsQuery();

            _interstielAdHistoryModelRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<InterstielAdHistoryModel, bool>>>()))
                        .ReturnsAsync(new List<InterstielAdHistoryModel> { new InterstielAdHistoryModel() { /*TODO:propertyler buraya yazılacak InterstielAdHistoryModelId = 1, InterstielAdHistoryModelName = "test"*/ } }.AsQueryable());

            var handler = new GetInterstielAdHistoryModelsQueryHandler(_interstielAdHistoryModelRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<InterstielAdHistoryModel>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task InterstielAdHistoryModel_CreateCommand_Success()
        {
            InterstielAdHistoryModel rt = null;
            //Arrange
            var command = new CreateInterstielAdHistoryModelCommand();
            //propertyler buraya yazılacak
            //command.InterstielAdHistoryModelName = "deneme";

            _interstielAdHistoryModelRepository.Setup(x => x.GetByIdAsync(It.IsAny<ObjectId>()))
                        .ReturnsAsync(rt);

            _interstielAdHistoryModelRepository.Setup(x => x.Add(It.IsAny<InterstielAdHistoryModel>()));

            var handler = new CreateInterstielAdHistoryModelCommandHandler(_interstielAdHistoryModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task InterstielAdHistoryModel_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateInterstielAdHistoryModelCommand();
            //propertyler buraya yazılacak 
            //command.InterstielAdHistoryModelName = "test";

            _interstielAdHistoryModelRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<InterstielAdHistoryModel, bool>>>()))
                                           .ReturnsAsync(new List<InterstielAdHistoryModel> { new InterstielAdHistoryModel() { /*TODO:propertyler buraya yazılacak InterstielAdHistoryModelId = 1, InterstielAdHistoryModelName = "test"*/ } }.AsQueryable());

            _interstielAdHistoryModelRepository.Setup(x => x.Add(It.IsAny<InterstielAdHistoryModel>()));

            var handler = new CreateInterstielAdHistoryModelCommandHandler(_interstielAdHistoryModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task InterstielAdHistoryModel_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateInterstielAdHistoryModelCommand();
            //command.InterstielAdHistoryModelName = "test";

            _interstielAdHistoryModelRepository.Setup(x => x.GetByIdAsync(It.IsAny<ObjectId>()))
                        .ReturnsAsync(new InterstielAdHistoryModel() { /*TODO:propertyler buraya yazılacak InterstielAdHistoryModelId = 1, InterstielAdHistoryModelName = "deneme"*/ });

            _interstielAdHistoryModelRepository.Setup(x => x.UpdateAsync(It.IsAny<ObjectId>(), It.IsAny<InterstielAdHistoryModel>()));

            var handler = new UpdateInterstielAdHistoryModelCommandHandler(_interstielAdHistoryModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task InterstielAdHistoryModel_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteInterstielAdHistoryModelCommand();

            _interstielAdHistoryModelRepository.Setup(x => x.GetByIdAsync(It.IsAny<ObjectId>()))
                        .ReturnsAsync(new InterstielAdHistoryModel() { /*TODO:propertyler buraya yazılacak InterstielAdHistoryModelId = 1, InterstielAdHistoryModelName = "deneme"*/});

            _interstielAdHistoryModelRepository.Setup(x => x.Delete(It.IsAny<InterstielAdHistoryModel>()));

            var handler = new DeleteInterstielAdHistoryModelCommandHandler(_interstielAdHistoryModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

