using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Business.Constants;
using Business.Handlers.RemoteOfferModels.Commands;
using Business.Handlers.RemoteOfferModels.Queries;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using FluentAssertions;
using MediatR;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using static Business.Handlers.RemoteOfferModels.Queries.GetRemoteOfferModelsByProjectIdQuery;
using static Business.Handlers.RemoteOfferModels.Commands.CreateRemoteOfferModelCommand;
using static Business.Handlers.RemoteOfferModels.Commands.UpdateRemoteOfferModelCommand;
using static Business.Handlers.RemoteOfferModels.Commands.DeleteRemoteOfferModelCommand;

namespace Tests.Business.Handlers
{
    [TestFixture]
    public class RemoteOfferModelHandlerTests
    {
        Mock<IRemoteOfferModelRepository> _remoteOfferModelRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _remoteOfferModelRepository = new Mock<IRemoteOfferModelRepository>();
            _mediator = new Mock<IMediator>();
        }

     
        [Test]
        public async Task RemoteOfferModel_GetByProjectIdQueries_Success()
        {
            //Arrange
            var query = new GetRemoteOfferModelsByProjectIdQuery();
            query.ProjectId = "121212";

            _remoteOfferModelRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<RemoteOfferModel, bool>>>()))
                        .ReturnsAsync(new List<RemoteOfferModel> { new RemoteOfferModel()
                        {
                            Version = 1,
                            FinishTime = DateTime.Now.Ticks,
                            FirstPrice = 12,
                            GiftTexture = new Byte[]{},
                            Id = new ObjectId(),
                            IsActive = true,
                            IsGift = true,
                            LastPrice = 10,
                            Name = "Test",
                            PlayerPercent = 20,
                            ProductList = new ProductModel[]{},
                            ProjectId = "121212",
                            StartTime = DateTime.Now.Ticks,
                            ValidityPeriod = 24

                        },new RemoteOfferModel()
                        {
                            Version = 4,
                            FinishTime = DateTime.Now.Ticks,
                            FirstPrice = 12,
                            GiftTexture = new Byte[]{},
                            Id = new ObjectId(),
                            IsActive = true,
                            IsGift = true,
                            LastPrice = 10,
                            Name = "Test",
                            PlayerPercent = 20,
                            ProductList = new ProductModel[]{},
                            ProjectId = "121212",
                            StartTime = DateTime.Now.Ticks,
                            ValidityPeriod = 24

                        },new RemoteOfferModel()
                        {
                            Version = 2,
                            FinishTime = DateTime.Now.Ticks,
                            FirstPrice = 12,
                            GiftTexture = new Byte[]{},
                            Id = new ObjectId(),
                            IsActive = true,
                            IsGift = true,
                            LastPrice = 10,
                            Name = "Test",
                            PlayerPercent = 20,
                            ProductList = new ProductModel[]{},
                            ProjectId = "121212",
                            StartTime = DateTime.Now.Ticks,
                            ValidityPeriod = 24

                        },

                        }.AsQueryable());

            var handler = new GetRemoteOfferModelsByProjectIdQueryHandler(_remoteOfferModelRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            x.Data.ToList().Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task RemoteOfferModel_CreateCommand_Success()
        {
            //Arrange
            var command = new CreateRemoteOfferModelCommand();
            command.FinishTime = new DateTime().Ticks;
            command.FirstPrice = 12;
            command.GiftTexture = new byte[] { };
            command.IsActive = true;
            command.IsGift = true;
            command.LastPrice = 2;
            command.Name = "Test";
            command.PlayerPercent = 15;
            command.ProductList = new ProductModel[] { };
            command.ProjectId = "121212";
            command.StartTime = DateTime.Now.Ticks;

            _remoteOfferModelRepository.Setup(x => 
                    x.Any(It.IsAny<Expression<Func<RemoteOfferModel, bool>>>()))
                        .Returns(false);

            _remoteOfferModelRepository.Setup(x => x.Add(It.IsAny<RemoteOfferModel>()));

            var handler = new CreateRemoteOfferModelCommandHandler(_remoteOfferModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task RemoteOfferModel_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateRemoteOfferModelCommand();
            command.FinishTime = new DateTime().Ticks;
            command.FirstPrice = 12;
            command.GiftTexture = new byte[] { };
            command.IsActive = true;
            command.IsGift = true;
            command.LastPrice = 2;
            command.Name = "Test";
            command.PlayerPercent = 15;
            command.ProductList = new ProductModel[] { };
            command.ProjectId = "121212";
            command.StartTime = DateTime.Now.Ticks;

            _remoteOfferModelRepository.Setup(x =>
                    x.Any(It.IsAny<Expression<Func<RemoteOfferModel, bool>>>()))
                .Returns(true);

            _remoteOfferModelRepository.Setup(x => 
                x.Add(It.IsAny<RemoteOfferModel>()));

            var handler = new CreateRemoteOfferModelCommandHandler(_remoteOfferModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task RemoteOfferModel_UpdateCommand_NoContent()
        {
            //Arrange
            var command = new UpdateRemoteOfferModelCommand();
            command.IsActive = true;
            command.Name = "Test";
            command.Version = 1;
            command.playerPercent = 20;
            command.ProjectId = "121212";

            _remoteOfferModelRepository.Setup(x =>
                    x.Any(It.IsAny<Expression<Func<RemoteOfferModel, bool>>>()))
                    .Returns(false);

            _remoteOfferModelRepository.Setup(x =>
                    x.GetByFilterAsync(It.IsAny<Expression<Func<RemoteOfferModel, bool>>>()))
                .ReturnsAsync(new RemoteOfferModel());

            _remoteOfferModelRepository.Setup(x => x.UpdateAsync(It.IsAny<ObjectId>(), It.IsAny<RemoteOfferModel>()));

            var handler = new UpdateRemoteOfferModelCommandHandler(_remoteOfferModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());


            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NoContent);
        }

        [Test]
        public async Task RemoteOfferModel_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateRemoteOfferModelCommand();
            command.IsActive = true;
            command.Name = "Test";
            command.Version = 1;
            command.playerPercent = 20;
            command.ProjectId = "121212";

            _remoteOfferModelRepository.Setup(x =>
                    x.Any(It.IsAny<Expression<Func<RemoteOfferModel, bool>>>()))
                    .Returns(true);
            
            _remoteOfferModelRepository.Setup(x => 
                x.GetByFilterAsync(It.IsAny<Expression<Func<RemoteOfferModel, bool>>>()))
                .ReturnsAsync(new RemoteOfferModel());

            _remoteOfferModelRepository.Setup(x => x.UpdateAsync(It.IsAny<ObjectId>(), It.IsAny<RemoteOfferModel>()));

            _mediator.Setup(x => x.Send(new object(), new System.Threading.CancellationToken()))
                .ReturnsAsync(new SuccessResult(Messages.Added));

            var handler = new UpdateRemoteOfferModelCommandHandler(_remoteOfferModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task RemoteOfferModel_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteRemoteOfferModelCommand();
            command.Name = "Test";
            command.ProjectId = "121212";
            command.Version = 1;

            _remoteOfferModelRepository.Setup(x => 
                x.DeleteAsync(It.IsAny<RemoteOfferModel>()));

            var handler = new DeleteRemoteOfferModelCommandHandler(_remoteOfferModelRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

