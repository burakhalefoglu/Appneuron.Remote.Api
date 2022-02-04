using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Business.Constants;
using Business.Handlers.InterstielAdHistoryModels.Queries;
using Business.Handlers.InterstitialAdHistoryModels.Commands;
using DataAccess.Abstract;
using Entities.Concrete;
using FluentAssertions;
using MediatR;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using static Business.Handlers.InterstielAdHistoryModels.Queries.GetInterstielAdHistoryModelByProjectIdQuery;
using static Business.Handlers.InterstitialAdHistoryModels.Commands.CreateInterstitialAdHistoryModelCommand;

namespace Tests.Business.Handlers
{
    [TestFixture]
    public class InterstitialAdHistoryModelHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            _interstitialAdHistoryModelRepository = new Mock<IInterstielAdHistoryModelRepository>();
            _mediator = new Mock<IMediator>();
        }

        private Mock<IInterstielAdHistoryModelRepository> _interstitialAdHistoryModelRepository;
        private Mock<IMediator> _mediator;


        [Test]
        public async Task InterstitialAdHistoryModel_GetQueries_Success()
        {
            //Arrange
            var query = new GetInterstielAdHistoryModelByProjectIdQuery
            {
                ProjectId = "121212"
            };

            _interstitialAdHistoryModelRepository.Setup(x =>
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
                        Version = "1",
                        playerPercent = 10
                    },
                    new()
                    {
                        ProjectId = "121212",
                        IsAdvSettingsActive = true,
                        Id = new ObjectId(),
                        Name = "test",
                        StarTime = DateTime.Now,
                        Version = "2",
                        playerPercent = 20
                    }
                }.AsQueryable());

            var handler =
                new GetInterstielAdHistoryModelByProjectIdQueryHandler(_interstitialAdHistoryModelRepository.Object,
                    _mediator.Object);

            //Act
            var x = await handler.Handle(query, new CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            x.Data.ToList().Count.Should().BeGreaterThan(1);
            x.Data.ToList().Count.Should().Be(2);
        }

        [Test]
        public async Task InterstitialAdHistoryModel_CreateCommand_Success()
        {
            //Arrange
            var command = new CreateInterstitialAdHistoryModelCommand
            {
                ProjectId = "121212",
                IsAdvSettingsActive = true,
                Name = "test",
                PlayerPercent = 20
            };

            _interstitialAdHistoryModelRepository.Setup(x =>
                x.AddAsync(It.IsAny<InterstielAdHistoryModel>()));

            var handler =
                new CreateInterstitialAdHistoryModelCommandHandler(_interstitialAdHistoryModelRepository.Object);
            var x = await handler.Handle(command, new CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }
    }
}