using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Business.Constants;
using Business.Handlers.InterstitialAdModels.Commands;
using Business.Handlers.InterstitialAdModels.Queries;
using Core.Utilities.MessageBrokers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using static Business.Handlers.InterstitialAdModels.Commands.CreateInterstitialsAdModelCommand;
using static Business.Handlers.InterstitialAdModels.Queries.GetInterstitialAdModelsQuery;
using static Business.Handlers.InterstitialAdModels.Commands.UpdateInterstitialAdModelCommand;
using static Business.Handlers.InterstitialAdModels.Commands.DeleteInterstitialAdModelCommand;

namespace Tests.Business.Handlers
{
    [TestFixture]
    public class InterstitialAdModelHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            _interstitialAdModelRepository = new Mock<IInterstielAdModelRepository>();
            _mediator = new Mock<IMediator>();
            _messageBroker = new Mock<IMessageBroker>();
            
        }

        private Mock<IInterstielAdModelRepository> _interstitialAdModelRepository;
        private Mock<IMediator> _mediator;
        private Mock<IMessageBroker> _messageBroker;
        

        [Test]
        public async Task InterstitialAdModel_GetByIdQueries_Success()
        {
            //Arrange
            var query = new GetInterstitialAdModelsQuery
            {
                ProjectId = 1
            };

            _interstitialAdModelRepository
                .Setup(x 
                    => x.GetListAsync(It.IsAny<Expression<Func<InterstitialAdModel, bool>>>()))
                .ReturnsAsync(new List<InterstitialAdModel>
                {
                    new()
                    {
                        ProjectId = 1,
                        Version = "1",
                        Id = 1,
                        Name = "Test"
                    },
                    new()
                    {
                        ProjectId = 2,
                        Version = "3",
                        Id = 2,
                        Name = "Test"
                    },
                    new()
                    {
                        ProjectId =3,
                        Version = "2",
                        Id = 3,
                        Name = "Test"
                    }
                }.AsQueryable());

            var handler =
                new InterstitialAdModelsQueryHandler(_interstitialAdModelRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            x.Data.ToList().Count.Should().BeGreaterThan(1);
        }

        [Test]
        public async Task InterstitialAdModel_CreateCommand_Success()
        {
            //Arrange
            var command = new CreateInterstitialsAdModelCommand
            {
                AdvStrategyDtos = Array.Empty<AdvStrategyDto>(),
                Name = "Test",
                ProjectId = 1,
                Version = "1"
            };


            _interstitialAdModelRepository.Setup(x => x.Any(It.IsAny<Expression<Func<InterstitialAdModel, bool>>>()))
                .Returns(false);

            _interstitialAdModelRepository.Setup(x => x.Add(It.IsAny<InterstitialAdModel>()));

            var handler =
                new CreateInterstitialAdModelCommandHandler(_interstitialAdModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task InterstitialAdModel_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateInterstitialsAdModelCommand
            {
                AdvStrategyDtos = Array.Empty<AdvStrategyDto>(),
                Name = "Test",
                ProjectId = 1,
                Version = "1"
            };


            _interstitialAdModelRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<InterstitialAdModel, bool>>>()))
                .ReturnsAsync(true);

            _interstitialAdModelRepository.Setup(x => x.Add(It.IsAny<InterstitialAdModel>()));

            var handler =
                new CreateInterstitialAdModelCommandHandler(_interstitialAdModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.AlreadyExist);
        }

        [Test]
        public async Task InterstitialAdModel_UpdateCommand_NoContent()
        {
            //Arrange
            var command = new UpdateInterstitialAdModelCommand
            {
                Name = "Test1",
                ProjectId = 1,
                Version = "1"
            };

            _interstitialAdModelRepository.Setup(x => x.Any(It.IsAny<Expression<Func<InterstitialAdModel, bool>>>()))
                .Returns(false);


            var handler =
                new UpdateInterstitialAdModelCommandHandler(_interstitialAdModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new CancellationToken());


            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NoContent);
        }

        [Test]
        public async Task InterstitialAdModel_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateInterstitialAdModelCommand
            {
                Name = "Test1",
                ProjectId = 1,
                Version = "1"
            };

            _interstitialAdModelRepository.Setup(x => x.AnyAsync(
                    It.IsAny<Expression<Func<InterstitialAdModel, bool>>>()))
                .ReturnsAsync(true);


            _interstitialAdModelRepository.Setup(x => x.GetAsync(
                    It.IsAny<Expression<Func<InterstitialAdModel, bool>>>()))
                .ReturnsAsync(new InterstitialAdModel());


            _mediator.Setup(x => x.Send(new object(),
                new CancellationToken())).ReturnsAsync(new SuccessResult(Messages.Added));

            var handler =
                new UpdateInterstitialAdModelCommandHandler(_interstitialAdModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task InterstitialAdModel_DeleteCommand_Success()
        {
         
      
            //Arrange
            var command = new DeleteInterstitialAdModelCommand
            {
                Name = "Test",
                ProjectId = 2,
                Version = "1"
            };
      _interstitialAdModelRepository.Setup(x =>
                x.GetAsync(It.IsAny<Expression<Func<InterstitialAdModel,bool>>>())).ReturnsAsync(new InterstitialAdModel());
            
            _interstitialAdModelRepository.Setup(x =>
                x.UpdateAsync(It.IsAny<InterstitialAdModel>()));

            var handler =
                new DeleteInterstitialAdModelCommandHandler(_interstitialAdModelRepository.Object,_mediator.Object);
            var x = await handler.Handle(command, new CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}