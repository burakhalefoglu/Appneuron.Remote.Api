using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Business.Constants;
using Business.Handlers.InterstielAdModels.Commands;
using Business.Handlers.InterstielAdModels.Queries;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using FluentAssertions;
using MediatR;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using static Business.Handlers.InterstielAdModels.Commands.CreateInterstielAdModelCommand;
using static Business.Handlers.InterstielAdModels.Queries.GetInterstielAdModelsByProjectIdQuery;
using static Business.Handlers.InterstielAdModels.Commands.UpdateInterstielAdModelCommand;
using static Business.Handlers.InterstielAdModels.Commands.DeleteInterstielAdModelCommand;

namespace Tests.Business.Handlers
{
    [TestFixture]
    public class InterstielAdModelHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            _interstielAdModelRepository = new Mock<IInterstielAdModelRepository>();
            _mediator = new Mock<IMediator>();
        }

        private Mock<IInterstielAdModelRepository> _interstielAdModelRepository;
        private Mock<IMediator> _mediator;

        [Test]
        public async Task InterstielAdModel_GetByIdQueries_Success()
        {
            //Arrange
            var query = new GetInterstielAdModelsByProjectIdQuery();
            query.ProjectId = "121212";

            _interstielAdModelRepository
                .Setup(x => x.GetListAsync(It.IsAny<Expression<Func<InterstielAdModel, bool>>>()))
                .ReturnsAsync(new List<InterstielAdModel>
                {
                    new()
                    {
                        ProjectId = "121212",
                        Version = 1,
                        AdvStrategies = new AdvStrategy[] { },
                        Id = new ObjectId(),
                        IsAdvSettingsActive = true,
                        Name = "Test"
                    },
                    new()
                    {
                        ProjectId = "121212",
                        Version = 3,
                        AdvStrategies = new AdvStrategy[] { },
                        Id = new ObjectId(),
                        IsAdvSettingsActive = true,
                        Name = "Test"
                    },
                    new()
                    {
                        ProjectId = "121212",
                        Version = 2,
                        AdvStrategies = new AdvStrategy[] { },
                        Id = new ObjectId(),
                        IsAdvSettingsActive = true,
                        Name = "Test"
                    }
                }.AsQueryable());

            var handler =
                new GetInterstielAdModelsByProjectIdQueryHandler(_interstielAdModelRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            x.Data.ToList().Count.Should().BeGreaterThan(1);
        }

        [Test]
        public async Task InterstielAdModel_CreateCommand_Success()
        {
            //Arrange
            var command = new CreateInterstielAdModelCommand();
            command.AdvStrategies = new AdvStrategy[] { };
            command.IsAdvSettingsActive = true;
            command.Name = "Test";
            command.ProjectId = "121212";
            command.Version = 1;


            _interstielAdModelRepository.Setup(x => x.Any(It.IsAny<Expression<Func<InterstielAdModel, bool>>>()))
                .Returns(false);

            _interstielAdModelRepository.Setup(x => x.Add(It.IsAny<InterstielAdModel>()));

            var handler =
                new CreateInterstielAdModelCommandHandler(_interstielAdModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task InterstielAdModel_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateInterstielAdModelCommand();
            command.AdvStrategies = new AdvStrategy[] { };
            command.IsAdvSettingsActive = true;
            command.Name = "Test";
            command.ProjectId = "121212";
            command.Version = 1;


            _interstielAdModelRepository.Setup(x => x.Any(It.IsAny<Expression<Func<InterstielAdModel, bool>>>()))
                .Returns(true);

            _interstielAdModelRepository.Setup(x => x.Add(It.IsAny<InterstielAdModel>()));

            var handler =
                new CreateInterstielAdModelCommandHandler(_interstielAdModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.AlreadyExist);
        }

        [Test]
        public async Task InterstielAdModel_UpdateCommand_NoContent()
        {
            //Arrange
            var command = new UpdateInterstielAdModelCommand();
            command.IsAdvSettingsActive = false;
            command.Name = "Test1";
            command.ProjectId = "121212";
            command.Version = 1;

            _interstielAdModelRepository.Setup(x => x.Any(It.IsAny<Expression<Func<InterstielAdModel, bool>>>()))
                .Returns(false);


            var handler =
                new UpdateInterstielAdModelCommandHandler(_interstielAdModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new CancellationToken());


            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NoContent);
        }

        [Test]
        public async Task InterstielAdModel_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateInterstielAdModelCommand();
            command.IsAdvSettingsActive = false;
            command.Name = "Test1";
            command.ProjectId = "121212";
            command.Version = 1;

            _interstielAdModelRepository.Setup(x => x.Any(
                    It.IsAny<Expression<Func<InterstielAdModel, bool>>>()))
                .Returns(true);


            _interstielAdModelRepository.Setup(x => x.GetByFilterAsync(
                    It.IsAny<Expression<Func<InterstielAdModel, bool>>>()))
                .ReturnsAsync(new InterstielAdModel());


            _mediator.Setup(x => x.Send(new object(),
                new CancellationToken())).ReturnsAsync(new SuccessResult(Messages.Added));

            var handler =
                new UpdateInterstielAdModelCommandHandler(_interstielAdModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task InterstielAdModel_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteInterstielAdModelCommand();
            command.Name = "Test";
            command.ProjectId = "121212";
            command.Version = 1;

            _interstielAdModelRepository.Setup(x =>
                x.DeleteAsync(It.IsAny<InterstielAdModel>()));

            var handler =
                new DeleteInterstielAdModelCommandHandler(_interstielAdModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}