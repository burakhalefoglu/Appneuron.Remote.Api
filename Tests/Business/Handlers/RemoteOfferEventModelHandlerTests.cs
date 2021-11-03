
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Entities.Concrete;
using static Business.Handlers.RemoteOfferEventModels.Commands.CreateRemoteOfferEventModelCommand;
using Business.Handlers.RemoteOfferEventModels.Commands;
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
    public class RemoteOfferEventModelHandlerTests
    {
        Mock<IRemoteOfferEventModelRepository> _remoteOfferEventModelRepository;
        Mock<IMediator> _mediator;
        private Mock<IKafkaMessageBroker> _kafka;


        [SetUp]
        public void Setup()
        { 
            _remoteOfferEventModelRepository = new Mock<IRemoteOfferEventModelRepository>();
            _mediator = new Mock<IMediator>();
            _kafka = new Mock<IKafkaMessageBroker>();

        }

        [Test]
        public async Task RemoteOfferEventModel_CreateCommand_Success()
        {
            //Arrange
            var command = new CreateRemoteOfferEventModelCommand();
            command.ClientIdList = new string[] { };
            command.FinishTime = new DateTime().Ticks;
            command.FirstPrice = 12;
            command.GiftTexture = new byte[]{};
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

            var x = await handler.Handle(command, new System.Threading.CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }
    }
}

