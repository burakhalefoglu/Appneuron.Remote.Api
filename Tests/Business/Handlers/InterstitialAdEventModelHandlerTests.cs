
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Entities.Concrete;
using static Business.Handlers.InterstitialAdEventModels.Commands.CreateInterstitialAdEventModelCommand;
using Business.Handlers.InterstitialAdEventModels.Commands;
using Business.Constants;
using MediatR;
using System.Linq;
using Business.MessageBrokers.Kafka;
using Core.Utilities.Results;
using FluentAssertions;
using MongoDB.Bson;

namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class InterstitialAdEventModelHandlerTests
    {
        Mock<IInterstitialAdEventModelRepository> _interstitialAdEventModelRepository;
        Mock<IMediator> _mediator;
        private Mock<IKafkaMessageBroker> _kafka;

        [SetUp]
        public void Setup()
        {
            _interstitialAdEventModelRepository = new Mock<IInterstitialAdEventModelRepository>();
            _mediator = new Mock<IMediator>();
            _kafka = new Mock<IKafkaMessageBroker>();
        }

       
        [Test]
        public async Task InterstitialAdEventModel_CreateCommand_Success()
        {
            //Arrange
            var command = new CreateInterstitialAdEventModelCommand();
            command.AdvFrequencyStrategies = new Dictionary<string, int>();
            command.ClientIdList = new string[] { };
            command.IsAdvSettingsActive = true;
            command.ProjectId = "121212";


            _interstitialAdEventModelRepository.Setup(
                x => x.Add(It.IsAny<InterstitialAdEventModel>()));

            _kafka.Setup(x => x.SendMessageAsync(new object()))
                .ReturnsAsync(new SuccessResult());

            var handler = new CreateInterstitialAdEventModelCommandHandler(
                _interstitialAdEventModelRepository.Object, _mediator.Object, _kafka.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }
    }
}

