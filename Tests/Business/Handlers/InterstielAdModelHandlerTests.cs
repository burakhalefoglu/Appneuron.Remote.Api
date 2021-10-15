
using Business.Handlers.InterstielAdModels.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.InterstielAdModels.Queries.GetInterstielAdModelQuery;
using Entities.Concrete;
using static Business.Handlers.InterstielAdModels.Queries.GetInterstielAdModelsQuery;
using static Business.Handlers.InterstielAdModels.Commands.CreateInterstielAdModelCommand;
using Business.Handlers.InterstielAdModels.Commands;
using Business.Constants;
using static Business.Handlers.InterstielAdModels.Commands.UpdateInterstielAdModelCommand;
using static Business.Handlers.InterstielAdModels.Commands.DeleteInterstielAdModelCommand;
using MediatR;
using System.Linq;
using FluentAssertions;
using MongoDB.Bson;

namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class InterstielAdModelHandlerTests
    {
        Mock<IInterstielAdModelRepository> _interstielAdModelRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _interstielAdModelRepository = new Mock<IInterstielAdModelRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task InterstielAdModel_GetQuery_Success()
        {
            //Arrange
            var query = new GetInterstielAdModelQuery();

            _interstielAdModelRepository.Setup(x => x.GetByIdAsync(It.IsAny<ObjectId>())).ReturnsAsync(new InterstielAdModel()
//propertyler buraya yazılacak
//{																		
//InterstielAdModelId = 1,
//InterstielAdModelName = "Test"
//}
);

            var handler = new GetInterstielAdModelQueryHandler(_interstielAdModelRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.InterstielAdModelId.Should().Be(1);

        }

        [Test]
        public async Task InterstielAdModel_GetQueries_Success()
        {
            //Arrange
            var query = new GetInterstielAdModelsQuery();

            _interstielAdModelRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<InterstielAdModel, bool>>>()))
                        .ReturnsAsync(new List<InterstielAdModel> { new InterstielAdModel() { /*TODO:propertyler buraya yazılacak InterstielAdModelId = 1, InterstielAdModelName = "test"*/ } }.AsQueryable());

            var handler = new GetInterstielAdModelsQueryHandler(_interstielAdModelRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<InterstielAdModel>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task InterstielAdModel_CreateCommand_Success()
        {
            InterstielAdModel rt = null;
            //Arrange
            var command = new CreateInterstielAdModelCommand();
            //propertyler buraya yazılacak
            //command.InterstielAdModelName = "deneme";

            _interstielAdModelRepository.Setup(x => x.GetByIdAsync(It.IsAny<ObjectId>()))
                        .ReturnsAsync(rt);

            _interstielAdModelRepository.Setup(x => x.Add(It.IsAny<InterstielAdModel>()));

            var handler = new CreateInterstielAdModelCommandHandler(_interstielAdModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task InterstielAdModel_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateInterstielAdModelCommand();
            //propertyler buraya yazılacak 
            //command.InterstielAdModelName = "test";

            _interstielAdModelRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<InterstielAdModel, bool>>>()))
                                           .ReturnsAsync(new List<InterstielAdModel> { new InterstielAdModel() { /*TODO:propertyler buraya yazılacak InterstielAdModelId = 1, InterstielAdModelName = "test"*/ } }.AsQueryable());

            _interstielAdModelRepository.Setup(x => x.Add(It.IsAny<InterstielAdModel>()));

            var handler = new CreateInterstielAdModelCommandHandler(_interstielAdModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task InterstielAdModel_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateInterstielAdModelCommand();
            //command.InterstielAdModelName = "test";

            _interstielAdModelRepository.Setup(x => x.GetByIdAsync(It.IsAny<ObjectId>()))
                        .ReturnsAsync(new InterstielAdModel() { /*TODO:propertyler buraya yazılacak InterstielAdModelId = 1, InterstielAdModelName = "deneme"*/ });

            _interstielAdModelRepository.Setup(x => x.UpdateAsync(It.IsAny<ObjectId>(), It.IsAny<InterstielAdModel>()));

            var handler = new UpdateInterstielAdModelCommandHandler(_interstielAdModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task InterstielAdModel_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteInterstielAdModelCommand();

            _interstielAdModelRepository.Setup(x => x.GetByIdAsync(It.IsAny<ObjectId>()))
                        .ReturnsAsync(new InterstielAdModel() { /*TODO:propertyler buraya yazılacak InterstielAdModelId = 1, InterstielAdModelName = "deneme"*/});

            _interstielAdModelRepository.Setup(x => x.Delete(It.IsAny<InterstielAdModel>()));

            var handler = new DeleteInterstielAdModelCommandHandler(_interstielAdModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

