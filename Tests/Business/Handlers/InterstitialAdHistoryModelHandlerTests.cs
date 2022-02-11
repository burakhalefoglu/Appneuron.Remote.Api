using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Business.Constants;
using Business.Handlers.InterstitialAdHistoryModels.Commands;
using Business.Handlers.InterstitialAdHistoryModels.Queries;
using DataAccess.Abstract;
using Entities.Concrete;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using static Business.Handlers.InterstitialAdHistoryModels.Queries.GetInterstielAdHistoryModelByProjectIdQuery;
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
                ProjectId = 1
            };

            _interstitialAdHistoryModelRepository.Setup(x =>
                    x.GetListAsync(It.IsAny<Expression<Func<InterstitialAdHistoryModel, bool>>>()))
                .ReturnsAsync(new List<InterstitialAdHistoryModel>
                {
                    new()
                    {
                        ProjectId = 12,
                        IsAdvSettingsActive = true,
                        Id = 1,
                        Name = "test",
                        StarTime = DateTime.Now,
                        Version = "2",
                        PlayerPercent = 20
                    },
                    new()
                    {
                        ProjectId = 13,
                        IsAdvSettingsActive = true,
                        Id = 1,
                        Name = "test",
                        StarTime = DateTime.Now,
                        Version = "2",
                        PlayerPercent = 20
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
                ProjectId = 2,
                IsAdvSettingsActive = true,
                Name = "test",
                PlayerPercent = 20
            };

            _interstitialAdHistoryModelRepository.Setup(x =>
                x.AddAsync(It.IsAny<InterstitialAdHistoryModel>()));

            var handler =
                new CreateInterstitialAdHistoryModelCommandHandler(_interstitialAdHistoryModelRepository.Object);
            var x = await handler.Handle(command, new CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }
    }
}