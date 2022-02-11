using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Business.Constants;
using Business.Handlers.InterstitialAdEventModels.Commands;
using Business.MessageBrokers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using static Business.Handlers.InterstitialAdEventModels.Commands.CreateInterstitialAdEventModelCommand;

namespace Tests.Business.Handlers
{
    [TestFixture]
    public class InterstitialAdEventModelHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            _interstitialAdEventModelRepository = new Mock<IInterstitialAdEventModelRepository>();
            _mediator = new Mock<IMediator>();
            _kafka = new Mock<IMessageBroker>();
        }

        private Mock<IInterstitialAdEventModelRepository> _interstitialAdEventModelRepository;
        private Mock<IMediator> _mediator;
        private Mock<IMessageBroker> _kafka;


        [Test]
        public async Task InterstitialAdEventModel_CreateCommand_Success()
        {
            //Arrange
            var command = new CreateInterstitialAdEventModelCommand();
            command.AdvFrequencyStrategies = new Dictionary<string, int>();
            command.ClientIdList = new string[] { };
            command.IsAdvSettingsActive = true;
            command.ProjectId = 1;


            _interstitialAdEventModelRepository.Setup(
                x => x.Add(It.IsAny<InterstitialAdEventModel>()));

            _kafka.Setup(x => x.SendMessageAsync(new object()))
                .ReturnsAsync(new SuccessResult());

            var handler = new CreateInterstitialAdEventModelCommandHandler(
                _interstitialAdEventModelRepository.Object, _mediator.Object, _kafka.Object);
            var x = await handler.Handle(command, new CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }
    }
}