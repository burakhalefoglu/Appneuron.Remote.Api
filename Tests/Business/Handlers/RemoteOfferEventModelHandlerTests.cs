using System;
using System.Threading;
using System.Threading.Tasks;
using Business.Constants;
using Business.Handlers.RemoteOfferEventModels.Commands;
using Business.MessageBrokers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using static Business.Handlers.RemoteOfferEventModels.Commands.CreateRemoteOfferEventModelCommand;

namespace Tests.Business.Handlers
{
    [TestFixture]
    public class RemoteOfferEventModelHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            _remoteOfferEventModelRepository = new Mock<IRemoteOfferEventModelRepository>();
            _mediator = new Mock<IMediator>();
            _kafka = new Mock<IMessageBroker>();
        }

        private Mock<IRemoteOfferEventModelRepository> _remoteOfferEventModelRepository;
        private Mock<IMediator> _mediator;
        private Mock<IMessageBroker> _kafka;

        [Test]
        public async Task RemoteOfferEventModel_CreateCommand_Success()
        {
            //Arrange
            var command = new CreateRemoteOfferEventModelCommand();
            command.ClientIdList = new string[] { };
            command.FinishTime = new DateTime().Ticks;
            command.FirstPrice = 12;
            command.GiftTexture = new byte[] { };
            command.IsActive = false;
            command.IsGift = true;
            command.LastPrice = 8;
            command.Name = "test";
            command.PlayerPercent = 20;
            command.ProjectId = "121212";
            command.ProductList = new ProductModel[] { };


            _remoteOfferEventModelRepository.Setup(x => x.Add(It.IsAny<RemoteOfferEventModel>()));

            _kafka.Setup(x => x.SendMessageAsync(new object()))
                .ReturnsAsync(new SuccessResult());

            var handler = new CreateRemoteOfferEventModelCommandHandler(
                _remoteOfferEventModelRepository.Object,
                _mediator.Object, _kafka.Object);

            var x = await handler.Handle(command, new CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }
    }
}