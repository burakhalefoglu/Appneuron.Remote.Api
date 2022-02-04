using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Business.Constants;
using Business.Handlers.RemoteOfferHistoryModels.Commands;
using Business.Handlers.RemoteOfferHistoryModels.Queries;
using DataAccess.Abstract;
using Entities.Concrete;
using FluentAssertions;
using MediatR;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using static Business.Handlers.RemoteOfferHistoryModels.Queries.GetOfferHistoryModelsByProjectIdQuery;
using static Business.Handlers.RemoteOfferHistoryModels.Commands.CreateRemoteOfferHistoryModelCommand;

namespace Tests.Business.Handlers
{
    [TestFixture]
    public class RemoteOfferHistoryModelHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            _remoteOfferHistoryModelRepository = new Mock<IRemoteOfferHistoryModelRepository>();
            _mediator = new Mock<IMediator>();
        }

        private Mock<IRemoteOfferHistoryModelRepository> _remoteOfferHistoryModelRepository;
        private Mock<IMediator> _mediator;


        [Test]
        public async Task RemoteOfferHistoryModel_GetByProjectIdQueries_Success()
        {
            //Arrange
            var query = new GetOfferHistoryModelsByProjectIdQuery
            {
                ProjectId = "121212"
            };

            _remoteOfferHistoryModelRepository.Setup(x =>
                    x.GetListAsync(It.IsAny<Expression<Func<RemoteOfferHistoryModel, bool>>>()))
                .ReturnsAsync(new List<RemoteOfferHistoryModel>
                {
                    new()
                    {
                        Version = "1",
                        FinishTime = DateTime.Now.Ticks,
                        FirstPrice = 12,
                        GiftTexture = Array.Empty<byte>(),
                        Id = new ObjectId(),
                        IsActive = true,
                        IsGift = true,
                        LastPrice = 10,
                        Name = "Test",
                        PlayerPercent = 20,
                        ProductList = Array.Empty<ProductModel>(),
                        ProjectId = "121212",
                        StartTime = DateTime.Now.Ticks,
                        ValidityPeriod = 24
                    },
                    new()
                    {
                        Version = "2",
                        FinishTime = DateTime.Now.Ticks,
                        FirstPrice = 12,
                        GiftTexture = Array.Empty<byte>(),
                        Id = new ObjectId(),
                        IsActive = true,
                        IsGift = true,
                        LastPrice = 0,
                        Name = "Test",
                        PlayerPercent = 20,
                        ProductList = Array.Empty<ProductModel>(),
                        ProjectId = "121212",
                        StartTime = DateTime.Now.Ticks,
                        ValidityPeriod = 24
                    },
                    new()
                    {
                        Version = "3",
                        FinishTime = DateTime.Now.Ticks,
                        FirstPrice = 12,
                        GiftTexture = Array.Empty<byte>(),
                        Id = new ObjectId(),
                        IsActive = true,
                        IsGift = true,
                        LastPrice = 4,
                        Name = "Test",
                        PlayerPercent = 20,
                        ProductList = Array.Empty<ProductModel>(),
                        ProjectId = "121212",
                        StartTime = DateTime.Now.Ticks,
                        ValidityPeriod = 48
                    }
                }.AsQueryable());

            var handler =
                new GetOfferHistoryModelsByProjectIdQueryHandler(_remoteOfferHistoryModelRepository.Object,
                    _mediator.Object);

            //Act
            var x = await handler.Handle(query, new CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            x.Data.ToList().Count.Should().BeGreaterThan(1);
        }

        [Test]
        public async Task RemoteOfferHistoryModel_CreateCommand_Success()
        {
            //Arrange
            var command = new CreateRemoteOfferHistoryModelCommand
            {
                FinishTime = new DateTime().Ticks,
                FirstPrice = 12,
                GiftTexture = Array.Empty<byte>(),
                IsActive = true,
                IsGift = true,
                LastPrice = 2,
                Name = "Test",
                PlayerPercent = 15,
                ProjectId = "121212",
                StartTime = DateTime.Now.Ticks
            };


            _remoteOfferHistoryModelRepository.Setup(x => x.Add(It.IsAny<RemoteOfferHistoryModel>()));

            var handler =
                new CreateRemoteOfferHistoryModelCommandHandler(_remoteOfferHistoryModelRepository.Object,
                    _mediator.Object);
            var x = await handler.Handle(command, new CancellationToken());


            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }
    }
}