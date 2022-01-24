using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Business.Constants;
using Business.Handlers.InterstielAdHistoryModels.Commands;
using Business.Handlers.InterstielAdHistoryModels.Queries;
using DataAccess.Abstract;
using Entities.Concrete;
using FluentAssertions;
using MediatR;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using static Business.Handlers.InterstielAdHistoryModels.Queries.GetInterstielAdHistoryModelByProjectIdQuery;
using static Business.Handlers.InterstielAdHistoryModels.Commands.CreateInterstielAdHistoryModelCommand;

namespace Tests.Business.Handlers
{
    [TestFixture]
    public class InterstielAdHistoryModelHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            _interstielAdHistoryModelRepository = new Mock<IInterstielAdHistoryModelRepository>();
            _mediator = new Mock<IMediator>();
        }

        private Mock<IInterstielAdHistoryModelRepository> _interstielAdHistoryModelRepository;
        private Mock<IMediator> _mediator;


        [Test]
        public async Task InterstielAdHistoryModel_GetQueries_Success()
        {
            //Arrange
            var query = new GetInterstielAdHistoryModelByProjectIdQuery();
            query.ProjectId = "121212";

            _interstielAdHistoryModelRepository.Setup(x =>
                    x.GetListAsync(It.IsAny<Expression<Func<InterstielAdHistoryModel, bool>>>()))
                .ReturnsAsync(new List<InterstielAdHistoryModel>
                {
                    new()
                    {
                        ProjectId = "121212",
                        IsAdvSettingsActive = true,
                        Id = new ObjectId(),
                        Name = "test",
                        StarTime = DateTime.Now,
                        Version = 1,
                        playerPercent = 10
                    },
                    new()
                    {
                        ProjectId = "121212",
                        IsAdvSettingsActive = true,
                        Id = new ObjectId(),
                        Name = "test",
                        StarTime = DateTime.Now,
                        Version = 2,
                        playerPercent = 20
                    }
                }.AsQueryable());

            var handler =
                new GetInterstielAdHistoryModelByProjectIdQueryHandler(_interstielAdHistoryModelRepository.Object,
                    _mediator.Object);

            //Act
            var x = await handler.Handle(query, new CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            x.Data.ToList().Count.Should().BeGreaterThan(1);
            x.Data.ToList().Count.Should().Be(2);
        }

        [Test]
        public async Task InterstielAdHistoryModel_CreateCommand_Success()
        {
            //Arrange
            var command = new CreateInterstielAdHistoryModelCommand();
            command.ProjectId = "121212";
            command.IsAdvSettingsActive = true;
            command.Name = "test";
            command.playerPercent = 20;

            _interstielAdHistoryModelRepository.Setup(x =>
                x.AddAsync(It.IsAny<InterstielAdHistoryModel>()));

            var handler =
                new CreateInterstielAdHistoryModelCommandHandler(_interstielAdHistoryModelRepository.Object,
                    _mediator.Object);
            var x = await handler.Handle(command, new CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }
    }
}