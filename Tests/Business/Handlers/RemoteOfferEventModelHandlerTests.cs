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
            _kafka = new Mock<IMessageBroker>();
        }

        private Mock<IRemoteOfferEventModelRepository> _remoteOfferEventModelRepository;
        private Mock<IMessageBroker> _kafka;

        [Test]
        public async Task RemoteOfferEventModel_CreateCommand_Success()
        {
            //Arrange
            var command = new CreateRemoteOfferEventModelCommand
            {
                ClientIdList = Array.Empty<string>(),
                FinishTime = new DateTime().Ticks,
                FirstPrice = 12,
                GiftTexture = Array.Empty<byte>(),
                IsActive = false,
                IsGift = true,
                LastPrice = 8,
                Name = "test",
                PlayerPercent = 20,
                ProjectId = "121212",
                ProductList = Array.Empty<ProductModel>()
            };


            _remoteOfferEventModelRepository.Setup(x => x.Add(It.IsAny<RemoteOfferEventModel>()));

            _kafka.Setup(x => x.SendMessageAsync(new object()))
                .ReturnsAsync(new SuccessResult());

            var handler = new CreateRemoteOfferEventModelCommandHandler(
                _remoteOfferEventModelRepository.Object, _kafka.Object);

            var x = await handler.Handle(command, new CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }
    }
}